using JT808.Server.Services;
using NewLife;
using NewLife.JT808.Protocols;
using NewLife.Log;
using NewLife.Threading;

namespace JT808.Server.HostedServices;

/// <summary>JT808 TCP 网关服务生命周期管理</summary>
/// <remarks>
/// 启动 <see cref="JT808Server"/> 并管理其生命周期，
/// 同时消费本节点指令队列，处理 Web 端下发的指令与强制下线请求。
/// 队列主题按节点隔离，Web 依据共享会话状态把指令投递到终端所在节点。
/// </remarks>
public class GpsHostedService : IHostedService
{
    private readonly JT808Server _server;
    private readonly ClusterState _cluster;
    private readonly CommandExecutor _executor;
    private TimerX? _commandTimer;

    /// <summary>实例化</summary>
    /// <param name="server">网关服务</param>
    /// <param name="cluster">集群共享状态</param>
    /// <param name="executor">指令执行器</param>
    public GpsHostedService(JT808Server server, ClusterState cluster, CommandExecutor executor)
    {
        _server = server;
        _cluster = cluster;
        _executor = executor;
    }

    /// <summary>启动</summary>
    /// <param name="cancellationToken">取消令牌</param>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _server.Start();
        XTrace.WriteLine("JT808 网关服务已启动，节点：{0}，端口：{1}", _cluster.Node, _server.Port);

        // 启动本节点指令队列消费（每 1 秒轮询）
        _commandTimer = new TimerX(DoConsumeCommand, null, 5_000, 1_000) { Async = true };
        XTrace.WriteLine("节点指令队列消费已启动：{0}", ClusterState.GetCommandTopic(_cluster.Node));

        return Task.CompletedTask;
    }

    /// <summary>停止</summary>
    /// <param name="cancellationToken">取消令牌</param>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _commandTimer.TryDispose();
        _commandTimer = null;
        _server.Stop("服务停止");
        XTrace.WriteLine("JT808 网关服务已停止");

        return Task.CompletedTask;
    }

    /// <summary>消费本节点指令队列</summary>
    private void DoConsumeCommand(Object? state)
    {
        try
        {
            var queue = _cluster.GetCommandQueue();
            while (queue.Count > 0)
            {
                var cmd = queue.TakeOne();
                if (cmd == null) break;

                _executor.Execute(cmd);
            }
        }
        catch (Exception ex)
        {
            XTrace.WriteException(ex);
        }
    }
}