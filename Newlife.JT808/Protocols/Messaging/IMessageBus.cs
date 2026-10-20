namespace NewLife.JT808.Protocols;

/// <summary>消息总线工厂</summary>
/// <remarks>
/// 把"设备数据生产/消费、上下线通知、外部下行指令"统一抽象为可按配置替换的总线。
/// 平台内部默认使用 Redis 实现（<see cref="RedisMessageBus"/>），
/// 网关侧可再接入 RocketMQ/Kafka 实现（位于 JT808.Server），用于与外部系统集成。
/// </remarks>
/// <example>
/// <code>
/// var bus = new RedisMessageBus(cacheProvider) { Group = "Group" };
///
/// // 生产
/// var producer = bus.CreateProducer&lt;Object&gt;("PositionData");
/// await producer.ProduceAsync("13800138000", position);
///
/// // 消费
/// var consumer = bus.CreateConsumer&lt;Object&gt;("PositionData");
/// await consumer.SubscribeAsync((mobile, data) => { /* 处理 */ return Task.CompletedTask; });
/// </code>
/// </example>
public interface IMessageBus
{
    /// <summary>提供方</summary>
    MessagingProviders Provider { get; }

    /// <summary>创建消息生产者</summary>
    /// <typeparam name="T">消息类型</typeparam>
    /// <param name="topic">主题</param>
    /// <returns>生产者</returns>
    IMsgProducer<T> CreateProducer<T>(String topic);

    /// <summary>创建消息消费者</summary>
    /// <typeparam name="T">消息类型</typeparam>
    /// <param name="topic">主题</param>
    /// <returns>消费者</returns>
    IMsgConsumer<T> CreateConsumer<T>(String topic);

    /// <summary>创建会话通知生产者</summary>
    /// <returns>会话通知生产者，不支持时返回 null</returns>
    ISessionProducer? CreateSessionProducer();

    /// <summary>创建会话通知消费者</summary>
    /// <returns>会话通知消费者，不支持时返回 null</returns>
    ISessionConsumer? CreateSessionConsumer();

    /// <summary>创建外部下行指令处理器</summary>
    /// <returns>下行指令处理器，不支持时返回 null</returns>
    IDownMessageHandler? CreateDownHandler();
}
