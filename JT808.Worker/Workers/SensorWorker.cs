using JT808.Data;
using NewLife;
using NewLife.JT808.Protocols;
using NewLife.Log;
using NewLife.Serialization;

namespace JT808.Worker.Workers;

/// <summary>传感器数据消费 Worker</summary>
/// <remarks>消费 SensorData 主题并持久化。</remarks>
public class SensorWorker : StreamConsumerWorker
{
    /// <summary>实例化</summary>
    /// <param name="bus">消息总线</param>
    public SensorWorker(IMessageBus bus) : base(bus, "SensorData") { }

    /// <summary>处理传感器数据</summary>
    /// <param name="mobile">终端手机号</param>
    /// <param name="data">传感器数据</param>
    protected override async Task ProcessAsync(String mobile, Object data)
    {
        XTrace.WriteLine("消费传感器数据：{0}", data);

        if (data is not String json) return;

        var pos = json.ToJsonEntity<PositionData>();
        if (pos == null) return;

        await pos.InsertAsync();
        XTrace.WriteLine("传感器数据已持久化：DeviceId={0}", pos.DeviceId);
    }
}