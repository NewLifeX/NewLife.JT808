using JT808.Data.Safety;
using NewLife;
using NewLife.Log;

namespace JT808.Server.Services;

/// <summary>报警事件聚合处理服务</summary>
public class AlarmEventService
{
    /// <summary>处理报警事件</summary>
    /// <param name="deviceId">设备ID</param>
    /// <param name="kind">报警大类 1-ADAS 2-DSM 3-BSD 4-TPMS</param>
    /// <param name="code">报警事件编码</param>
    /// <param name="level">报警级别 0-紧急 1-警告</param>
    public void ProcessAlarm(Int32 deviceId, Int32 kind, Int32 code, Int32 level)
    {
        XTrace.WriteLine("报警事件：DeviceId={0}, Kind={1}, Code={2}, Level={3}", deviceId, kind, code, level);

        try
        {
            // 查找或创建报警事件（按设备+编码聚合）
            var alarmEvent = AlarmEvent.Find(AlarmEvent._.DeviceId == deviceId & AlarmEvent._.Code == code);

            if (alarmEvent == null)
            {
                alarmEvent = new AlarmEvent
                {
                    DeviceId = deviceId,
                    Kind = kind,
                    Code = code,
                    Level = level,
                    Times = 1,
                    CreateTime = DateTime.Now,
                };
                alarmEvent.Insert();
            }
            else
            {
                alarmEvent.Times++;
                alarmEvent.Update();
            }
        }
        catch (Exception ex)
        {
            XTrace.WriteLine("报警事件处理异常：{0}", ex.Message);
        }
    }
}
