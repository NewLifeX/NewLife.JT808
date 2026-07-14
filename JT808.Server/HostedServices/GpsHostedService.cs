using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NewLife.JT808.Protocols;
using NewLife.Log;

namespace JT808.Server.HostedServices;

/// <summary>JT808 TCP 网关服务生命周期管理</summary>
/// <remarks>
/// 参考 GPS808 的 GpsHostedService：启动 JT808Server 并管理其生命周期。
/// </remarks>
public class GpsHostedService : IHostedService
{
    private readonly JT808Server _server;
    private readonly ILogger<GpsHostedService> _logger;

    public GpsHostedService(ILogger<GpsHostedService> logger)
    {
        _logger = logger;

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

        // TODO: 注册 Redis 指令队列消费
        // 启动时从 Redis command 队列消费消息，下发到终端
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _server.Start();
        _logger.LogInformation("JT808 网关服务已启动，端口：{Port}", _server.Port);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _server.Stop("服务停止");
        _logger.LogInformation("JT808 网关服务已停止");
        return Task.CompletedTask;
    }
}
