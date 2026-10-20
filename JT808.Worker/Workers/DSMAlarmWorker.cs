using JT808.Data.Safety;
using NewLife;
using NewLife.JT808.Protocols;
using NewLife.Log;
using NewLife.Serialization;

namespace JT808.Worker.Workers;

/// <summary>DSM 报警消费 Worker</summary>
/// <remarks>消费 DSMAlarm 主题并持久化。</remarks>
public class DSMAlarmWorker : StreamConsumerWorker
{
    /// <summary>实例化</summary>
    /// <param name="bus">消息总线</param>
    public DSMAlarmWorker(IMessageBus bus) : base(bus, "DSMAlarm") { }

    /// <summary>处理 DSM 报警</summary>
    /// <param name="mobile">终端手机号</param>
    /// <param name="data">报警数据</param>
    protected override async Task ProcessAsync(String mobile, Object data)
    {
        XTrace.WriteLine("消费 DSM 报警：{0}", data);

        if (data is not String json) return;

        var alarm = json.ToJsonEntity<DSMAlarm>();
        if (alarm == null) return;

        await alarm.InsertAsync();
        XTrace.WriteLine("DSM 报警已持久化：DeviceId={0}, AlarmType={1}", alarm.DeviceId, alarm.AlarmType);
    }
}