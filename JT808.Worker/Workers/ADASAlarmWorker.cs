using JT808.Data.Safety;
using NewLife;
using NewLife.JT808.Protocols;
using NewLife.Log;
using NewLife.Serialization;

namespace JT808.Worker.Workers;

/// <summary>ADAS 报警消费 Worker</summary>
/// <remarks>消费 ADASAlarm 主题并持久化。</remarks>
public class ADASAlarmWorker : StreamConsumerWorker
{
    /// <summary>实例化</summary>
    /// <param name="bus">消息总线</param>
    public ADASAlarmWorker(IMessageBus bus) : base(bus, "ADASAlarm") { }

    /// <summary>处理 ADAS 报警</summary>
    /// <param name="mobile">终端手机号</param>
    /// <param name="data">报警数据</param>
    protected override async Task ProcessAsync(String mobile, Object data)
    {
        XTrace.WriteLine("消费 ADAS 报警：{0}", data);

        if (data is not String json) return;

        var alarm = json.ToJsonEntity<ADASAlarm>();
        if (alarm == null) return;

        await alarm.InsertAsync();
        XTrace.WriteLine("ADAS 报警已持久化：DeviceId={0}, AlarmType={1}", alarm.DeviceId, alarm.AlarmType);
    }
}