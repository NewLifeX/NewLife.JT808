#if !NET45
using Confluent.Kafka;
using NewLife.Log;
using NewLife.Serialization;

namespace NewLife.JT808.Protocols.Kafka;

/// <summary>基于 Kafka 的消息生产者适配器</summary>
/// <remarks>
/// 将 Confluent.Kafka 的 IProducer 包装为 JT808 的 IMsgProducer 接口。
/// 消息体被序列化为 JSON 字符串后发送，mobile 编码到消息的 Key 中。
/// 需要 .NET Standard 2.0+ 或 .NET Framework 4.6.1+ 运行时支持。
/// </remarks>
/// <typeparam name="TMessage">消息类型</typeparam>
public class KafkaProducer<TMessage> : IMsgProducer<TMessage>, IDisposable
{
    #region 属性
    /// <summary>Kafka 生产者实例</summary>
    public IProducer<String, String> Producer { get; }

    /// <summary>主题名称</summary>
    public String Topic { get; }

    /// <summary>是否已启动</summary>
    public Boolean Active => _active;
    private Boolean _active;
    #endregion

    #region 构造
    /// <summary>实例化 Kafka 生产者适配器</summary>
    /// <param name="bootstrapServers">Kafka 服务器地址（如 localhost:9092）</param>
    /// <param name="topic">主题名称</param>
    /// <param name="config">额外生产者配置。若为 null 则使用默认配置</param>
    public KafkaProducer(String bootstrapServers, String topic, ProducerConfig? config = null)
    {
        Topic = topic;

        var cfg = config ?? new ProducerConfig();
        cfg.BootstrapServers = bootstrapServers;

        Producer = new ProducerBuilder<String, String>(cfg).Build();
        _active = true;
    }

    /// <summary>实例化 Kafka 生产者适配器</summary>
    /// <param name="producer">已创建的 IProducer 实例</param>
    /// <param name="topic">主题名称</param>
    public KafkaProducer(IProducer<String, String> producer, String topic)
    {
        Producer = producer;
        Topic = topic;
        _active = true;
    }
    #endregion

    #region 方法
    /// <summary>生产消息到 Kafka</summary>
    /// <param name="mobile">终端手机号，编码到消息 Key 中</param>
    /// <param name="message">消息对象</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否成功</returns>
    public async Task<Boolean> ProduceAsync(String mobile, TMessage message, CancellationToken cancellationToken = default)
    {
        try
        {
            var json = JsonHelper.ToJson(message);
            var result = await Producer.ProduceAsync(Topic, new Message<String, String>
            {
                Key = mobile,
                Value = json,
            }, cancellationToken);

            return result.Status == PersistenceStatus.Persisted;
        }
        catch (Exception ex)
        {
            Log?.Error("Kafka produce failed: {0}", ex.Message);
            return false;
        }
    }

    /// <summary>释放资源</summary>
    public void Dispose()
    {
        if (_active)
        {
            _active = false;
            Producer.Flush();
            Producer.Dispose();
        }
    }

    /// <summary>日志</summary>
    public ILog Log { get; set; } = Logger.Null;
    #endregion
}
#endif
