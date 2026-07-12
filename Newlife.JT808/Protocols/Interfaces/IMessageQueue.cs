namespace NewLife.JT808.Protocols;

/// <summary>消息生产者接口，用于将设备上行消息生产到队列</summary>
/// <typeparam name="TMessage">消息类型</typeparam>
public interface IMsgProducer<in TMessage>
{
    /// <summary>生产消息到队列</summary>
    /// <param name="mobile">终端手机号</param>
    /// <param name="message">消息对象</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否成功</returns>
    Task<Boolean> ProduceAsync(String mobile, TMessage message, CancellationToken cancellationToken = default);

    /// <summary>主题名称</summary>
    String Topic { get; }
}

/// <summary>消息消费者接口，用于消费队列中的设备上行消息</summary>
/// <typeparam name="TMessage">消息类型</typeparam>
public interface IMsgConsumer<TMessage>
{
    /// <summary>订阅消息</summary>
    /// <param name="onMessage">消息处理回调</param>
    /// <param name="cancellationToken">取消令牌</param>
    Task SubscribeAsync(Func<String, TMessage, Task> onMessage, CancellationToken cancellationToken = default);

    /// <summary>取消订阅</summary>
    Task UnsubscribeAsync(CancellationToken cancellationToken = default);

    /// <summary>主题名称</summary>
    String Topic { get; }
}

/// <summary>会话通知生产者接口</summary>
public interface ISessionProducer
{
    /// <summary>通知终端上线</summary>
    /// <param name="mobile">终端手机号</param>
    /// <param name="cancellationToken">取消令牌</param>
    Task ProduceOnlineAsync(String mobile, CancellationToken cancellationToken = default);

    /// <summary>通知终端离线</summary>
    /// <param name="mobile">终端手机号</param>
    /// <param name="cancellationToken">取消令牌</param>
    Task ProduceOfflineAsync(String mobile, CancellationToken cancellationToken = default);
}

/// <summary>会话通知消费者接口</summary>
public interface ISessionConsumer
{
    /// <summary>订阅会话通知</summary>
    /// <param name="onOnline">终端上线回调</param>
    /// <param name="onOffline">终端离线回调</param>
    /// <param name="cancellationToken">取消令牌</param>
    Task SubscribeAsync(Func<String, Task> onOnline, Func<String, Task> onOffline, CancellationToken cancellationToken = default);

    /// <summary>取消订阅</summary>
    Task UnsubscribeAsync(CancellationToken cancellationToken = default);
}

/// <summary>下行消息处理接口</summary>
/// <remarks>
/// 用于处理外部系统通过消息队列下发的指令。
/// 接收到指令后在本服务器上查找终端会话并下发。
/// </remarks>
public interface IDownMessageHandler
{
    /// <summary>处理下行消息</summary>
    /// <param name="mobile">目标终端手机号</param>
    /// <param name="data">消息体序列化数据</param>
    /// <returns>处理结果</returns>
    Task<Byte[]> HandleAsync(String mobile, Byte[] data);
}

/// <summary>消息日志接口</summary>
public interface IMsgLogging
{
    /// <summary>记录上行消息日志</summary>
    Task LogUpAsync(String mobile, Byte[] data);

    /// <summary>记录下行消息日志</summary>
    Task LogDownAsync(String mobile, Byte[] data);

    /// <summary>记录下行应答日志</summary>
    Task LogReplyAsync(String mobile, Byte[] data);
}
