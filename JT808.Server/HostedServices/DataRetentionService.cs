using Microsoft.Extensions.Hosting;
using JT808.Data;
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
    private Int32 _keepDays = 90;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // 每天凌晨 3 点执行一次
        var now = DateTime.Now;
        var first = now.Date.AddDays(1).AddHours(3) - now;
        _timer = new TimerX(DoClean, null, (Int32)first.TotalMilliseconds, 86_400_000) { Async = true };
        XTrace.WriteLine("数据过期清理服务已启动，保留 {0} 天", _keepDays);
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
        try
        {
            var expire = DateTime.Now.AddDays(-_keepDays);
            XTrace.WriteLine("开始清理 {0} 之前的数据...", expire.ToString("yyyy-MM-dd"));

            // 清理过期位置数据
            var delPos = PositionData.Delete(PositionData._.CreateTime < expire);
            XTrace.WriteLine("清理位置数据：{0} 条", delPos);

            // 清理过期原始报文
            var delRaw = RawData.Delete(RawData._.CreateTime < expire);
            XTrace.WriteLine("清理原始报文：{0} 条", delRaw);

            // 清理过期报警
            var delAlarm = Alarm.Delete(Alarm._.CreateTime < expire);
            XTrace.WriteLine("清理报警数据：{0} 条", delAlarm);

            XTrace.WriteLine("数据清理完成");
        }
        catch (Exception ex)
        {
            XTrace.WriteLine("数据清理异常：{0}", ex.Message);
        }
    }
}
