using NewLife.Caching;
using NewLife.JT808.Protocols;
using NewLife.Log;

namespace JT808.Server.Messaging;

/// <summary>消息总线运行时</summary>
/// <remarks>
/// 内部传输固定使用 Redis（与在线会话、指令路由共用一套 Redis），
/// 外部集成按 <see cref="MessagingOptions.External"/> 选择 RocketMQ/Kafka。
/// 外部集成从三个方面接入：
/// <list type="bullet">
/// <item>上行镜像：设备数据在生产到 Redis 的同时写入外部主题</item>
/// <item>会话事件：终端上下线广播到外部主题</item>
/// <item>外部下行：从外部主题消费指令并在本地会话执行</item>
/// </list>
/// </remarks>
public class MessagingRuntime : IDisposable
{
    #region 属性
    /// <summary>配置</summary>
    public MessagingOptions Options { get; }

    /// <summary>内部消息总线（Redis）</summary>
    public IMessageBus Internal { get; }

    /// <summary>外部消息总线。未启用时为 null</summary>
    public IMessageBus? External { get; }

    /// <summary>会话事件生产者。未启用时为 null</summary>
    public ISessionProducer? SessionProducer { get; }

    /// <summary>外部下行指令处理器。未启用时为 null</summary>
    public IDownMessageHandler? DownHandler { get; }

    /// <summary>外部下行指令订阅者。未启用时为 null</summary>
    public IDownMessageSubscriber? DownSubscriber { get; }
    #endregion

    #region 构造
    /// <summary>实例化消息总线运行时</summary>
    /// <param name="cacheProvider">缓存服务，用于内部 Redis 传输</param>
    /// <param name="options">配置</param>
    public MessagingRuntime(ICacheProvider cacheProvider, MessagingOptions options)
    {
        Options = options ?? throw new ArgumentNullException(nameof(options));

        var error = options.Validate();
        if (error != null) throw new InvalidOperationException(error);

        Internal = new RedisMessageBus(cacheProvider);
        External = CreateExternal(options);

        if (External != null)
        {
            if (options.SessionEvents) SessionProducer = External.CreateSessionProducer();
            if (options.Downlink)
            {
                DownHandler = External.CreateDownHandler();
                DownSubscriber = DownHandler as IDownMessageSubscriber;
            }

            XTrace.WriteLine("外部消息集成已启用：{0}，会话事件={1}，外部下行={2}", options.External, options.SessionEvents, options.Downlink);
        }
        else
        {
            XTrace.WriteLine("外部消息集成未启用，仅使用 Redis 内部传输");
        }
    }

    private static IMessageBus? CreateExternal(MessagingOptions options) => options.External switch
    {
        MessagingProviders.RocketMQ => new RocketMQMessageBus(options),
        MessagingProviders.Kafka => new KafkaMessageBus(options),
        _ => null,
    };
    #endregion

    #region 方法
    /// <summary>释放资源</summary>
    public void Dispose()
    {
        (DownHandler as IDisposable)?.Dispose();
        (SessionProducer as IDisposable)?.Dispose();
    }
    #endregion
}
