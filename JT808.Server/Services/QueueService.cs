using System.Collections.Concurrent;
using JT808.Server.Messaging;
using NewLife.JT808.Protocols;
using NewLife.Log;

namespace JT808.Server.Services;

/// <summary>设备数据消息生产服务</summary>
/// <remarks>
/// 通过 <see cref="IMessageBus"/> 把位置、传感器、事件与各类报警数据投递到主题：
/// 内部固定走 Redis（Worker 消费），启用外部集成时额外镜像到 RocketMQ/Kafka。
/// 外部镜像失败只记录日志，不影响设备接入与内部处理。
/// </remarks>
public class QueueService
{
    private readonly MessagingRuntime _runtime;
    private readonly ConcurrentDictionary<String, IMsgProducer<Object>> _internal = new(StringComparer.Ordinal);
    private readonly ConcurrentDictionary<String, IMsgProducer<Object>> _external = new(StringComparer.Ordinal);
    private volatile Boolean _externalFailed;

    /// <summary>实例化</summary>
    /// <param name="runtime">消息总线运行时</param>
    public QueueService(MessagingRuntime runtime) => _runtime = runtime;

    /// <summary>推送位置数据</summary>
    /// <param name="data">位置数据</param>
    public void AddPosition(Object data) => Publish("PositionData", data);

    /// <summary>推送传感器数据</summary>
    /// <param name="data">传感器数据</param>
    public void AddSensor(Object data) => Publish("SensorData", data);

    /// <summary>推送事件数据</summary>
    /// <param name="data">事件数据</param>
    public void AddEvent(Object data) => Publish("EventData", data);

    /// <summary>推送 ADAS 报警</summary>
    /// <param name="data">报警数据</param>
    public void AddAdasAlarm(Object data) => Publish("ADASAlarm", data);

    /// <summary>推送 DSM 报警</summary>
    /// <param name="data">报警数据</param>
    public void AddDsmAlarm(Object data) => Publish("DSMAlarm", data);

    /// <summary>推送 BSD 报警</summary>
    /// <param name="data">报警数据</param>
    public void AddBsdAlarm(Object data) => Publish("BSDAlarm", data);

    /// <summary>推送原始报文</summary>
    /// <param name="data">原始报文</param>
    public void AddRawData(Object data) => Publish("RawData", data);

    private void Publish(String topic, Object data, String? mobile = null)
    {
        try
        {
            var producer = GetProducer(_internal, _runtime.Internal, topic);
            _ = producer.ProduceAsync(mobile ?? String.Empty, data);
        }
        catch (Exception ex)
        {
            XTrace.WriteException(ex);
        }

        if (_runtime.External == null || _externalFailed) return;

        try
        {
            var producer = GetProducer(_external, _runtime.External, topic);
            _ = producer.ProduceAsync(mobile ?? String.Empty, data);
        }
        catch (Exception ex)
        {
            // 外部集成故障不影响设备接入，只在本进程内熔断，避免刷屏重试
            _externalFailed = true;
            XTrace.WriteLine("外部消息镜像已停用：{0}", ex.Message);
        }
    }

    private static IMsgProducer<Object> GetProducer(ConcurrentDictionary<String, IMsgProducer<Object>> cache, IMessageBus bus, String topic) =>
        cache.GetOrAdd(topic, e => bus.CreateProducer<Object>(e));
}