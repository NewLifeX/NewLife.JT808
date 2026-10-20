using JT808.Data;
using NewLife;
using NewLife.JT808.Protocols;
using NewLife.Log;
using NewLife.Serialization;

namespace JT808.Worker.Workers;

/// <summary>位置数据消费 Worker</summary>
/// <remarks>消费 PositionData 主题，处理里程计算、坐标转换。</remarks>
public class PositionWorker : StreamConsumerWorker
{
    /// <summary>实例化</summary>
    /// <param name="bus">消息总线</param>
    public PositionWorker(IMessageBus bus) : base(bus, "PositionData") { }

    /// <summary>处理位置数据</summary>
    /// <param name="mobile">终端手机号</param>
    /// <param name="data">位置数据</param>
    protected override async Task ProcessAsync(String mobile, Object data)
    {
        XTrace.WriteLine("消费位置数据：{0}", data);

        // 解析位置数据（期望 JSON 字符串或 PositionData 对象）
        PositionData? pos = null;
        if (data is String json)
            pos = json.ToJsonEntity<PositionData>();
        else if (data is PositionData p)
            pos = p;

        if (pos == null) return;

        await pos.InsertAsync();
        XTrace.WriteLine("位置数据已持久化：DeviceId={0}, Lat={1}, Lng={2}", pos.DeviceId, pos.Latitude, pos.Longitude);
    }
}