#if !NET45
using Confluent.Kafka;
using NewLife.Log;
using NewLife.Serialization;

namespace NewLife.JT808.Protocols.Kafka;

/// <summary>基于 Kafka 的会话通知适配器</summary>
/// <remarks>
/// 将终端的上下线事件通过 Kafka 主题广播。
/// 上线通知发送 "online" 消息，下线通知发送 "offline" 消息。
/// 需要 .NET Standard 2.0+ 或 .NET Framework 4.6.1+ 运行时支持。
/// </remarks>
public class KafkaSessionAdapter : ISessionProducer, IDisposable
{
    #region 属性
    /// <summary>Kafka 生产者实例</summary>
    public IProducer<String, String> Producer { get; }

    /// <summary>会话通知主题</summary>
    public String Topic { get; set; } = "SessionEvent";
    #endregion

    #region 构造
    /// <summary>实例化 Kafka 会话通知适配器</summary>
    /// <param name="bootstrapServers">Kafka 服务器地址（如 localhost:9092）</param>
    /// <param name="topic">会话通知主题</param>
    /// <param name="config">额外生产者配置。若为 null 则使用默认配置</param>
    public KafkaSessionAdapter(String bootstrapServers, String? topic = null, ProducerConfig? config = null)
    {
        if (topic != null) Topic = topic;

        var cfg = config ?? new ProducerConfig();
        cfg.BootstrapServers = bootstrapServers;

        Producer = new ProducerBuilder<String, String>(cfg).Build();
    }
    #endregion

    #region 方法
    /// <summary>通知终端上线</summary>
    public async Task ProduceOnlineAsync(String mobile, CancellationToken cancellationToken = default)
    {
        var json = JsonHelper.ToJson(new { mobile, action = "online", time = DateTime.UtcNow });
        await Producer.ProduceAsync(Topic, new Message<String, String>
        {
            Key = mobile,
            Value = json,
        }, cancellationToken);
    }

    /// <summary>通知终端离线</summary>
    public async Task ProduceOfflineAsync(String mobile, CancellationToken cancellationToken = default)
    {
        var json = JsonHelper.ToJson(new { mobile, action = "offline", time = DateTime.UtcNow });
        await Producer.ProduceAsync(Topic, new Message<String, String>
        {
            Key = mobile,
            Value = json,
        }, cancellationToken);
    }

    /// <summary>释放资源</summary>
    public void Dispose()
    {
        try
        {
            Producer.Flush();
            Producer.Dispose();
        }
        catch { }
    }

    /// <summary>日志</summary>
    public ILog Log { get; set; } = Logger.Null;
    #endregion
}
#endif
