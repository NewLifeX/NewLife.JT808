using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting;
using NewLife;
using NewLife.Log;
using NewLife.Threading;

namespace JT808.Server.HostedServices;

/// <summary>数据过期清理服务</summary>
/// <remarks>
/// 定期清理过期位置数据、原始报文、报警数据，可配置保留天数。
/// </remarks>
public class DataRetentionService : IHostedService
{
    private TimerX? _timer;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // 每天凌晨 3 点执行一次
        var now = DateTime.Now;
        var first = now.Date.AddDays(1).AddHours(3) - now;
        _timer = new TimerX(DoClean, null, (Int32)first.TotalMilliseconds, 86_400_000) { Async = true };
        XTrace.WriteLine("数据过期清理服务已启动");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer.TryDispose();
        _timer = null;
        return Task.CompletedTask;
    }

    private void DoClean(Object? state)
    {
        // TODO: 删除超过保留天数的 PositionData / RawData / Alarm
        // var keepDays = 90;
        // var expire = DateTime.Now.AddDays(-keepDays);
        // PositionData.DeleteBefore(expire);
    }
}
