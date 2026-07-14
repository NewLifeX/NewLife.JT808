using Microsoft.Extensions.Hosting;
using JT808.Data;
using Microsoft.Extensions.Hosting;
using NewLife.Log;

namespace JT808.Server.HostedServices;

/// <summary>启动预热服务</summary>
/// <remarks>
/// 在 JT808 网关服务启动前预热加载关键数据（产品、设备白名单等），
/// 防止连接冲击。参考 GPS808 的 PreheatHostedService。
/// </remarks>
public class PreheatHostedService : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        XTrace.WriteLine("开始预热加载数据...");
        try
        {
            // 预热产品缓存
            var products = Product.FindAllWithCache();
            XTrace.WriteLine("产品缓存预热完成：{0} 条", products.Count);

            // 预热在线设备
            var onlines = DeviceOnline.FindAllWithCache();
            XTrace.WriteLine("设备在线缓存预热完成：{0} 条", onlines.Count);
        }
        catch (Exception ex)
        {
            XTrace.WriteLine("预热加载异常（可忽略）：{0}", ex.Message);
        }
        XTrace.WriteLine("预热加载完成");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
