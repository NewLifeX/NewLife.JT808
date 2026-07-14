using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting;
using NewLife;
using NewLife.Log;
using NewLife.Threading;

namespace JT808.Server.HostedServices;

/// <summary>缓存清理服务</summary>
/// <remarks>
/// 定期清理实体缓存和运行时缓存，释放内存。
/// </remarks>
public class ClearHostedService : IHostedService
{
    private TimerX? _timer;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // 每 30 分钟执行一次
        _timer = new TimerX(DoClear, null, 30 * 60_000, 30 * 60_000) { Async = true };
        XTrace.WriteLine("缓存清理服务已启动");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer.TryDispose();
        _timer = null;
        return Task.CompletedTask;
    }

    private void DoClear(Object? state)
    {
        try
        {
            // 触发 GC 释放内存
            GC.Collect();
            GC.WaitForPendingFinalizers();
            XTrace.WriteLine("缓存清理完成");
        }
        catch (Exception ex)
        {
            XTrace.WriteLine("缓存清理异常：{0}", ex.Message);
        }
    }
}
