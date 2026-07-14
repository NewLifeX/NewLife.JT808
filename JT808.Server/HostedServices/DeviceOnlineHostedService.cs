using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting;
using NewLife;
using NewLife.Log;
using NewLife.Threading;

namespace JT808.Server.HostedServices;

/// <summary>设备在线状态维护服务</summary>
/// <remarks>
/// 定时检查设备心跳超时，更新 DeviceOnline 在线状态。
/// </remarks>
public class DeviceOnlineHostedService : IHostedService
{
    private TimerX? _timer;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // 每 60 秒检查一次心跳超时
        _timer = new TimerX(DoCheck, null, 60_000, 60_000) { Async = true };
        XTrace.WriteLine("设备在线状态维护服务已启动");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer.TryDispose();
        _timer = null;
        return Task.CompletedTask;
    }

    private void DoCheck(Object? state)
    {
        // TODO: 检查所有在线设备的心跳时间，超时则标记离线
        // var timeout = DateTime.Now.AddMinutes(-5);
        // DeviceOnline.SetOffline(timeout);
    }
}
