using JT808.Data;
using NewLife;
using NewLife.JT808.Protocols;
using NewLife.Log;
using NewLife.Serialization;

namespace JT808.Worker.Workers;

/// <summary>事件数据消费 Worker</summary>
/// <remarks>消费 EventData 主题并写入报警表。</remarks>
public class EventWorker : StreamConsumerWorker
{
    /// <summary>实例化</summary>
    /// <param name="bus">消息总线</param>
    public EventWorker(IMessageBus bus) : base(bus, "EventData") { }

    /// <summary>处理事件数据</summary>
    /// <param name="mobile">终端手机号</param>
    /// <param name="data">事件数据</param>
    protected override async Task ProcessAsync(String mobile, Object data)
    {
        XTrace.WriteLine("消费事件数据：{0}", data);

        if (data is not String json) return;

        var alarm = json.ToJsonEntity<Alarm>();
        if (alarm == null) return;

        await alarm.InsertAsync();
        XTrace.WriteLine("事件数据已持久化：DeviceId={0}, AlarmType={1}", alarm.DeviceId, alarm.AlarmType);
    }
}