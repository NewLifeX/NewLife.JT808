using NewLife.Log;

namespace JT808.Server.Services;

/// <summary>报警事件聚合处理服务</summary>
public class AlarmEventService
{
    public AlarmEventService()
    {
    }

    /// <summary>处理报警事件</summary>
    public void ProcessAlarm(Int32 deviceId, Int32 alarmType, DateTime alarmTime, Object? extraData)
    {
        XTrace.WriteLine("报警事件：DeviceId={0}, AlarmType={1}, Time={2}", deviceId, alarmType, alarmTime);
        // TODO: 报警事件聚合、持久化、推送
    }
}
