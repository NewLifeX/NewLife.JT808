using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting;
using NewLife.Log;

namespace JT808.Server.HostedServices;

/// <summary>启动预热服务</summary>
/// <remarks>
/// 在 JT808 网关服务启动前预热加载关键数据（白名单、产品信息等），
/// 防止连接冲击。参考 GPS808 的 PreheatHostedService。
/// </remarks>
public class PreheatHostedService : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        XTrace.WriteLine("开始预热加载数据...");
        // TODO: 预热设备白名单缓存
        // TODO: 预热产品信息缓存
        XTrace.WriteLine("预热加载完成");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
