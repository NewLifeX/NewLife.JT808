using NewLife.JT808.Protocols;
using JT808.Server.Messaging.Kafka;

namespace JT808.Server.Messaging;

/// <summary>基于 Kafka 的外部消息总线</summary>
/// <remarks>
/// 复用 <see cref="KafkaProducer{TMessage}"/>、<see cref="KafkaConsumer{TMessage}"/>、
/// <see cref="KafkaSessionAdapter"/>、<see cref="KafkaDownHandler"/> 适配器，消费组按主题拆分。
/// </remarks>
public class KafkaMessageBus : IMessageBus
{
    private readonly MessagingOptions _options;

    /// <summary>提供方</summary>
    public MessagingProviders Provider => MessagingProviders.Kafka;

    /// <summary>Kafka 服务器地址</summary>
    public String BootstrapServers => _options.BootstrapServers ?? throw new InvalidOperationException("Kafka BootstrapServers is not configured.");

    /// <summary>分组前缀</summary>
    public String Group => _options.ExternalGroup;

    /// <summary>实例化</summary>
    /// <param name="options">配置</param>
    public KafkaMessageBus(MessagingOptions options) => _options = options ?? throw new ArgumentNullException(nameof(options));

    /// <summary>创建消息生产者</summary>
    /// <typeparam name="T">消息类型</typeparam>
    /// <param name="topic">主题</param>
    /// <returns>生产者</returns>
    public IMsgProducer<T> CreateProducer<T>(String topic) => new KafkaProducer<T>(BootstrapServers, topic);

    /// <summary>创建消息消费者</summary>
    /// <typeparam name="T">消息类型</typeparam>
    /// <param name="topic">主题</param>
    /// <returns>消费者</returns>
    public IMsgConsumer<T> CreateConsumer<T>(String topic) => new KafkaConsumer<T>(BootstrapServers, topic, $"{Group}_{topic}");

    /// <summary>创建会话通知生产者</summary>
    /// <returns>会话通知生产者</returns>
    public ISessionProducer CreateSessionProducer() => new KafkaSessionAdapter(BootstrapServers, _options.SessionTopic);

    /// <summary>创建会话通知消费者</summary>
    /// <returns>Kafka 未提供会话消费者适配器，返回 null</returns>
    public ISessionConsumer? CreateSessionConsumer() => null;

    /// <summary>创建外部下行指令处理器</summary>
    /// <returns>下行指令处理器</returns>
    public IDownMessageHandler CreateDownHandler() => new KafkaDownHandler(BootstrapServers, _options.DownTopic, $"{Group}_{_options.DownTopic}");
}
