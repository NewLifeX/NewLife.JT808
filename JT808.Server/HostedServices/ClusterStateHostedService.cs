using JT808.Server.Messaging;
using NewLife;
using NewLife.JT808.Protocols;
using NewLife.Log;
using NewLife.Threading;

namespace JT808.Server.HostedServices;

/// <summary>集群共享状态同步服务</summary>
/// <remarks>
/// 定时把本节点在线会话、节点心跳写入分布式缓存，并从缓存同步黑名单。
/// Web 管理接口据此得到真实在线会话、节点列表和黑名单，实现跨进程共享。
/// 启用外部集成时，会话变化还会广播到外部主题（<see cref="SessionEvent"/>）。
/// 会话与节点记录都带过期时间，本节点异常退出后由过期时间兜底清理。
/// </remarks>
public class ClusterStateHostedService : IHostedService
{
    private readonly JT808Server _server;
    private readonly ClusterState _cluster;
    private readonly MessagingRuntime _runtime;

    private TimerX? _timer;
    private readonly Object _lock = new();
    private readonly DateTime _startTime = DateTime.Now;
    private readonly Dictionary<String, DateTime> _loginTimes = new(StringComparer.Ordinal);
    private readonly HashSet<String> _published = new(StringComparer.Ordinal);

    /// <summary>实例化</summary>
    /// <param name="server">网关服务</param>
    /// <param name="cluster">集群共享状态</param>
    /// <param name="runtime">消息总线运行时</param>
    public ClusterStateHostedService(JT808Server server, ClusterState cluster, MessagingRuntime runtime)
    {
        _server = server;
        _cluster = cluster;
        _runtime = runtime;

        _server.Blacklist.Log = XTrace.Log;
    }

    /// <summary>启动</summary>
    /// <param name="cancellationToken">取消令牌</param>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new TimerX(DoPublish, null, 1_000, 5_000) { Async = true };
        XTrace.WriteLine("集群共享状态同步已启动，节点：{0}", _cluster.Node);

        return Task.CompletedTask;
    }

    /// <summary>停止</summary>
    /// <param name="cancellationToken">取消令牌</param>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer.TryDispose();
        _timer = null;

        // 节点正常退出，立即清理本节点共享状态
        try
        {
            lock (_lock)
            {
                foreach (var mobile in _published)
                {
                    _cluster.RemoveSession(mobile);
                }
                _published.Clear();
            }
            _cluster.RemoveNode(_cluster.Node);
        }
        catch (Exception ex)
        {
            XTrace.WriteException(ex);
        }

        return Task.CompletedTask;
    }

    private void DoPublish(Object? state)
    {
        try
        {
            SyncBlacklist();
            PublishSessions();
            PublishNode();
        }
        catch (Exception ex)
        {
            XTrace.WriteException(ex);
        }
    }

    /// <summary>从共享缓存同步黑名单。缓存为权威数据源</summary>
    private void SyncBlacklist()
    {
        var mobiles = _cluster.GetBlacklist();
        if (_server.Blacklist.ReplaceAll(mobiles))
            XTrace.WriteLine("黑名单已同步：{0} 个", mobiles.Length);
    }

    /// <summary>发布本节点在线会话，并清理已断开的会话</summary>
    private void PublishSessions()
    {
        var now = DateTime.Now;
        var current = new HashSet<String>(StringComparer.Ordinal);
        var online = new List<String>();
        var offline = new List<String>();

        foreach (var item in _server.Manager.GetAll())
        {
            var mobile = item.Key;
            if (mobile.IsNullOrEmpty() || item.Value == null) continue;

            current.Add(mobile);
            if (!_loginTimes.TryGetValue(mobile, out var loginTime))
            {
                _loginTimes[mobile] = loginTime = now;
            }

            var info = new JTSessionInfo
            {
                Mobile = mobile,
                Node = _cluster.Node,
                SessionId = item.Value.ID + "",
                Remote = item.Value.Remote?.ToString(),
                LoginTime = loginTime,
                LastActive = now,
            };
            _cluster.PublishSession(info);
        }

        lock (_lock)
        {
            foreach (var mobile in current)
            {
                if (!_published.Contains(mobile)) online.Add(mobile);
            }

            foreach (var mobile in _published)
            {
                if (current.Contains(mobile)) continue;

                _cluster.RemoveSession(mobile);
                _loginTimes.Remove(mobile);
                offline.Add(mobile);
            }

            _published.Clear();
            foreach (var mobile in current) _published.Add(mobile);
        }

        PublishSessionEvents(online, true);
        PublishSessionEvents(offline, false);
    }

    /// <summary>广播会话事件。未启用外部集成时忽略</summary>
    private void PublishSessionEvents(IList<String> mobiles, Boolean online)
    {
        var producer = _runtime.SessionProducer;
        if (producer == null || mobiles.Count == 0) return;

        foreach (var mobile in mobiles)
        {
            try
            {
                if (online)
                    _ = producer.ProduceOnlineAsync(mobile);
                else
                    _ = producer.ProduceOfflineAsync(mobile);
            }
            catch (Exception ex)
            {
                XTrace.WriteLine("会话事件广播失败：Mobile={0}, {1}", mobile, ex.Message);
            }
        }
    }

    /// <summary>发布节点心跳</summary>
    private void PublishNode()
    {
        Int32 count;
        lock (_lock)
        {
            count = _published.Count;
        }

        _cluster.PublishNode(new JTNodeInfo
        {
            Node = _cluster.Node,
            Port = _server.Port,
            StartTime = _startTime,
            LastActive = DateTime.Now,
            Sessions = count,
        });
    }
}
