using System.Collections.Concurrent;

namespace NewLife.JT808.Protocols;

/// <summary>基于进程内内存队列的消息总线</summary>
/// <remarks>
/// 单机部署或无外部中间件时使用，生产与消费须在同一进程内。
/// 同一主题（且消息类型相同）的生产者与消费者共享同一个 <see cref="DefaultMsgQueue{T}"/> 实例。
/// </remarks>
public class MemoryMessageBus : IMessageBus
{
    private readonly ConcurrentDictionary<String, Object> _queues = new(StringComparer.Ordinal);

    /// <summary>会话事件主题</summary>
    public String SessionTopic { get; set; } = "SessionEvent";

    /// <summary>提供方</summary>
    public MessagingProviders Provider => MessagingProviders.Memory;

    /// <summary>创建消息生产者</summary>
    /// <typeparam name="T">消息类型</typeparam>
    /// <param name="topic">主题</param>
    /// <returns>生产者</returns>
    public IMsgProducer<T> CreateProducer<T>(String topic) => GetQueue<T>(topic);

    /// <summary>创建消息消费者</summary>
    /// <typeparam name="T">消息类型</typeparam>
    /// <param name="topic">主题</param>
    /// <returns>消费者</returns>
    public IMsgConsumer<T> CreateConsumer<T>(String topic) => GetQueue<T>(topic);

    /// <summary>创建会话通知生产者</summary>
    /// <returns>会话通知生产者</returns>
    public ISessionProducer CreateSessionProducer() => GetSessionChannel();

    /// <summary>创建会话通知消费者</summary>
    /// <returns>会话通知消费者</returns>
    public ISessionConsumer CreateSessionConsumer() => GetSessionChannel();

    /// <summary>创建外部下行指令处理器</summary>
    /// <returns>内存总线不支持外部下行，返回 null</returns>
    public IDownMessageHandler? CreateDownHandler() => null;

    /// <summary>获取指定主题的内存队列</summary>
    /// <typeparam name="T">消息类型</typeparam>
    /// <param name="topic">主题</param>
    /// <returns>队列实例</returns>
    public DefaultMsgQueue<T> GetQueue<T>(String topic)
    {
        if (topic.IsNullOrEmpty()) throw new ArgumentException(L.T("Topic is required."), nameof(topic));

        var key = topic + "|" + typeof(T).FullName;
        return (DefaultMsgQueue<T>)_queues.GetOrAdd(key, _ => new DefaultMsgQueue<T>(topic));
    }

    private MemorySessionChannel GetSessionChannel()
    {
        // 会话通道使用独立键，避免与普通主题队列共用实例造成重复创建
        var key = SessionTopic + "|Session|" + typeof(SessionEvent).FullName;
        return (MemorySessionChannel)_queues.GetOrAdd(key, _ => new MemorySessionChannel(SessionTopic));
    }

    /// <summary>内存会话通知通道</summary>
    private sealed class MemorySessionChannel : ISessionProducer, ISessionConsumer, IDisposable
    {
        private readonly DefaultMsgQueue<SessionEvent> _queue;

        public MemorySessionChannel(String topic) => _queue = new DefaultMsgQueue<SessionEvent>(topic);

        /// <summary>节点标识</summary>
        public String? Node { get; set; }

        public Task ProduceOnlineAsync(String mobile, CancellationToken cancellationToken = default) => PublishAsync(mobile, true);

        public Task ProduceOfflineAsync(String mobile, CancellationToken cancellationToken = default) => PublishAsync(mobile, false);

        public Task SubscribeAsync(Func<String, Task> onOnline, Func<String, Task> onOffline, CancellationToken cancellationToken = default) =>
            _queue.SubscribeAsync(async (mobile, e) =>
            {
                if (e == null || e.Mobile.IsNullOrEmpty()) return;

                if (e.Online)
                    await onOnline(e.Mobile).ConfigureAwait(false);
                else
                    await onOffline(e.Mobile).ConfigureAwait(false);
            }, cancellationToken);

        public Task UnsubscribeAsync(CancellationToken cancellationToken = default) => _queue.UnsubscribeAsync(cancellationToken);

        public void Dispose() => _queue.Dispose();

        private Task PublishAsync(String mobile, Boolean online)
        {
            if (mobile.IsNullOrEmpty()) return Task.FromResult(0);

            return _queue.ProduceAsync(mobile, new SessionEvent { Mobile = mobile, Online = online, Node = Node, Time = DateTime.Now });
        }
    }
}
