using JT808.Data;
using NewLife;
using NewLife.JT808.Protocols;
using NewLife.Log;
using NewLife.Serialization;

namespace JT808.Worker.Workers;

/// <summary>胎压数据消费 Worker</summary>
/// <remarks>消费 TirePressure 主题并持久化。</remarks>
public class TirePressureWorker : StreamConsumerWorker
{
    /// <summary>实例化</summary>
    /// <param name="bus">消息总线</param>
    public TirePressureWorker(IMessageBus bus) : base(bus, "TirePressure") { }

    /// <summary>处理胎压数据</summary>
    /// <param name="mobile">终端手机号</param>
    /// <param name="data">胎压数据</param>
    protected override async Task ProcessAsync(String mobile, Object data)
    {
        XTrace.WriteLine("消费胎压数据：{0}", data);

        if (data is not String json) return;

        var tp = json.ToJsonEntity<TirePressure>();
        if (tp == null) return;

        await tp.InsertAsync();
        XTrace.WriteLine("胎压数据已持久化：DeviceId={0}", tp.DeviceId);
    }
}