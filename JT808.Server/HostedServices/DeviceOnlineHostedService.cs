using Microsoft.Extensions.Hosting;
using JT808.Data;
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
    private volatile Boolean _running;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _running = true;
        // 每 60 秒检查一次心跳超时
        _timer = new TimerX(DoCheck, null, 60_000, 60_000) { Async = true };
        XTrace.WriteLine("设备在线状态维护服务已启动");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _running = false;
        _timer.TryDispose();
        _timer = null;
        return Task.CompletedTask;
    }

    private void DoCheck(Object? state)
    {
        try
        {
            // 心跳超时 5 分钟设为离线
            var timeout = DateTime.Now.AddMinutes(-5);
            var list = DeviceOnline.FindAll(DeviceOnline._.Online == true, null, null, 0, 0);
            foreach (var online in list)
            {
                if (online.HeartbeatTime < timeout)
                {
                    online.Online = false;
                    online.Update();

                    // 记录下线路历史
                    var history = new DeviceHistory
                    {
                        DeviceId = online.DeviceId,
                        Mobile = online.Mobile,
                        Action = "下线",
                        Remark = "心跳超时",
                        CreateTime = DateTime.Now,
                    };
                    history.Insert();

                    XTrace.WriteLine("设备心跳超时离线：Mobile={0}", online.Mobile);
                }
            }
        }
        catch (Exception ex)
        {
            XTrace.WriteLine("在线状态检查异常：{0}", ex.Message);
        }
    }
}
