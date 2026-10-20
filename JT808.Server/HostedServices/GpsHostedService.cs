using JT808.Data;
using NewLife;
using NewLife.JT808.Protocols;
using NewLife.Log;
using NewLife.Serialization;
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
    private TimerX? _commandTimer;

    /// <summary>实例化</summary>
    /// <param name="server">网关服务</param>
    /// <param name="cluster">集群共享状态</param>
    public GpsHostedService(JT808Server server, ClusterState cluster)
    {
        _server = server;
        _cluster = cluster;
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

                ProcessCommand(cmd);
            }
        }
        catch (Exception ex)
        {
            XTrace.WriteException(ex);
        }
    }

    /// <summary>执行指令。设备不在本节点时直接忽略</summary>
    /// <param name="cmd">指令</param>
    private void ProcessCommand(JTCommand cmd)
    {
        if (cmd.Mobile.IsNullOrEmpty()) return;

        var session = _server.Manager.Find(cmd.Mobile) as JT808Session;
        if (session == null)
        {
            XTrace.WriteLine("设备不在线，忽略指令：Mobile={0}, Action={1}", cmd.Mobile, cmd.Action);
            return;
        }

        if (cmd.Action == JTCommandActions.Kick)
        {
            _server.Manager.Remove(cmd.Mobile);
            _cluster.RemoveSession(cmd.Mobile);
            session.Close("管理员强制断开");
            XTrace.WriteLine("会话已强制断开：Mobile={0}", cmd.Mobile);
            return;
        }

        XTrace.WriteLine("收到指令队列消息：Mobile={0}, Type={1}", cmd.Mobile, cmd.BodyType);

        if (cmd.BodyType.IsNullOrEmpty() || cmd.BodyData.IsNullOrEmpty()) return;

        var type = MessageFactory.FindType(cmd.BodyType);
        if (type == null)
        {
            XTrace.WriteLine("无法解析消息体类型：{0}", cmd.BodyType);
            return;
        }

        var body = JsonHelper.ToJsonEntity(cmd.BodyData, type, null);
        if (body == null)
        {
            XTrace.WriteLine("无法反序列化消息体：{0}", cmd.BodyData);
            return;
        }

        session.SendJT808(body);
        XTrace.WriteLine("指令已下发到终端：Mobile={0}, Type={1}", cmd.Mobile, cmd.BodyType);

        // 记录指令
        var device = Device.FindByMobile(cmd.Mobile);
        if (device != null)
        {
            var cmdRecord = new DeviceCommand
            {
                DeviceId = device.Id,
                Mobile = cmd.Mobile,
                Kind = 0,
                Status = 1, // 已下发
                BodyType = cmd.BodyType,
                BodyData = cmd.BodyData,
                CreateTime = DateTime.Now,
            };
            _ = Task.Run(() => cmdRecord.InsertAsync());
        }
    }
}
