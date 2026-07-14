using JT808.Data;
using JT808.Server.Services;
using NewLife;
using NewLife.Caching;
using NewLife.JT808.Models;
using NewLife.JT808.Protocols;
using NewLife.Log;
using NewLife.Serialization;
using NewLife.Threading;

namespace JT808.Server.HostedServices;

/// <summary>JT808 TCP 网关服务生命周期管理</summary>
/// <remarks>
/// 参考 GPS808 的 GpsHostedService：启动 JT808Server 并管理其生命周期。
/// 同时消费 Redis command 队列，处理 Web 端下发的指令。
/// </remarks>
public class GpsHostedService : IHostedService
{
    private readonly JT808Server _server;
    private readonly ICacheProvider _cacheProvider;
    private TimerX? _commandTimer;

    public GpsHostedService(ICacheProvider cacheProvider)
    {
        _cacheProvider = cacheProvider;

        _server = new JT808Server
        {
            Port = 808,
            Log = XTrace.Log,
            SessionLog = XTrace.Log,
            Tracer = DefaultTracer.Instance,
        };

        // 注册指令处理器
        _server.AddHandler(new Handlers.NormalController());
        _server.AddHandler(new Handlers.PositionController());
        _server.AddHandler(new Handlers.InfoController());
        _server.AddHandler(new Handlers.MediaController());
        _server.AddHandler(new Handlers.FileHandler());
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _server.Start();
        XTrace.WriteLine("JT808 网关服务已启动，端口：{0}", _server.Port);

        // 启动 Redis 指令队列消费（每 1 秒轮询）
        _commandTimer = new TimerX(DoConsumeCommand, null, 5_000, 1_000) { Async = true };
        XTrace.WriteLine("Redis 指令队列消费已启动");

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _commandTimer.TryDispose();
        _commandTimer = null;
        _server.Stop("服务停止");
        XTrace.WriteLine("JT808 网关服务已停止");
        return Task.CompletedTask;
    }

    /// <summary>消费 Redis command 队列，下发指令到终端</summary>
    private void DoConsumeCommand(Object? state)
    {
        try
        {
            var queue = _cacheProvider.GetQueue<CommandModel>("Command", "Group");
            if (queue == null) return;

            while (queue.Count > 0)
            {
                var cmd = queue.TakeOne();
                if (cmd == null) break;

                XTrace.WriteLine("收到指令队列消息：Mobile={0}, Type={1}", cmd.Mobile, cmd.BodyType);

                // 反序列化消息体
                if (cmd.BodyType.IsNullOrEmpty() || cmd.BodyData.IsNullOrEmpty()) continue;

                var type = Type.GetType(cmd.BodyType);
                if (type == null)
                {
                    XTrace.WriteLine("无法解析消息体类型：{0}", cmd.BodyType);
                    continue;
                }

                var body = JsonHelper.ToJsonEntity(cmd.BodyData, type);
                if (body == null)
                {
                    XTrace.WriteLine("无法反序列化消息体：{0}", cmd.BodyData);
                    continue;
                }

                // 查找会话并下发
                var session = _server.Manager.Find(cmd.Mobile) as JT808Session;
                if (session != null)
                {
                    // 使用 SendJT808 发送消息体（内部构造 JTMessage）
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
                else
                {
                    XTrace.WriteLine("设备不在线，无法下发指令：Mobile={0}", cmd.Mobile);
                }
            }
        }
        catch (Exception ex)
        {
            XTrace.WriteLine("指令队列消费异常：{0}", ex.Message);
        }
    }
}

/// <summary>指令模型。与 Web 端 CommandClient 保持一致</summary>
internal class CommandModel
{
    /// <summary>目标终端手机号</summary>
    public String Mobile { get; set; } = String.Empty;

    /// <summary>消息体类型全名</summary>
    public String BodyType { get; set; } = String.Empty;

    /// <summary>消息体 JSON 数据</summary>
    public String BodyData { get; set; } = String.Empty;

    /// <summary>来源</summary>
    public String? Source { get; set; }

    /// <summary>创建时间</summary>
    public DateTime CreateTime { get; set; }
}
