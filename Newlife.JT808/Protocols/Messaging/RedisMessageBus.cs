using NewLife.Caching;
using NewLife.Log;

namespace NewLife.JT808.Protocols;

/// <summary>基于 Redis（ICacheProvider）的消息总线</summary>
/// <remarks>
/// 平台内部传输的默认实现：普通主题使用带消费组的 Redis Stream，会话事件使用独立主题。
/// 生产与消费都由 <see cref="ICacheProvider"/> 提供，不直接依赖 NewLife.Redis，便于单元测试替换为内存实现。
/// </remarks>
public class RedisMessageBus : IMessageBus
{
    #region 属性
    /// <summary>缓存服务</summary>
    public ICacheProvider CacheProvider { get; }

    /// <summary>消费组。同一主题被多个进程消费时应保持一致</summary>
    public String Group { get; set; } = "Group";

    /// <summary>会话事件主题</summary>
    public String SessionTopic { get; set; } = "SessionEvent";

    /// <summary>节点标识</summary>
    public String? Node { get; set; }

    /// <summary>提供方</summary>
    public MessagingProviders Provider => MessagingProviders.Redis;
    #endregion

    #region 构造
    /// <summary>实例化 Redis 消息总线</summary>
    /// <param name="cacheProvider">缓存服务</param>
    public RedisMessageBus(ICacheProvider cacheProvider) => CacheProvider = cacheProvider ?? throw new ArgumentNullException(nameof(cacheProvider));
    #endregion

    #region 方法
    /// <summary>创建消息生产者</summary>
    /// <typeparam name="T">消息类型</typeparam>
    /// <param name="topic">主题</param>
    /// <returns>生产者</returns>
    public IMsgProducer<T> CreateProducer<T>(String topic) => new RedisMsgProducer<T>(CacheProvider, topic, Group);

    /// <summary>创建消息消费者</summary>
    /// <typeparam name="T">消息类型</typeparam>
    /// <param name="topic">主题</param>
    /// <returns>消费者</returns>
    public IMsgConsumer<T> CreateConsumer<T>(String topic) => new RedisMsgConsumer<T>(CacheProvider, topic, Group);

    /// <summary>创建会话通知生产者</summary>
    /// <returns>会话通知生产者</returns>
    public ISessionProducer CreateSessionProducer() => new RedisSessionProducer(CacheProvider, SessionTopic, Group) { Node = Node };

    /// <summary>创建会话通知消费者</summary>
    /// <returns>会话通知消费者</returns>
    public ISessionConsumer CreateSessionConsumer() => new RedisSessionConsumer(CacheProvider, SessionTopic, Group);

    /// <summary>创建外部下行指令处理器</summary>
    /// <returns>Redis 下行走集群节点队列，此处返回 null</returns>
    public IDownMessageHandler? CreateDownHandler() => null;
    #endregion
}

/// <summary>基于 Redis Stream 的消息生产者</summary>
/// <typeparam name="T">消息类型</typeparam>
public class RedisMsgProducer<T> : IMsgProducer<T>
{
    private readonly ICacheProvider _cacheProvider;
    private readonly String _group;
    private IProducerConsumer<T>? _queue;

    /// <summary>主题名称</summary>
    public String Topic { get; }

    /// <summary>实例化</summary>
    /// <param name="cacheProvider">缓存服务</param>
    /// <param name="topic">主题</param>
    /// <param name="group">消费组</param>
    public RedisMsgProducer(ICacheProvider cacheProvider, String topic, String group = "Group")
    {
        _cacheProvider = cacheProvider;
        Topic = topic;
        _group = group;
    }

    /// <summary>生产消息到 Redis Stream</summary>
    /// <param name="mobile">终端手机号。Redis 载荷不单独保存，需包含在消息体内</param>
    /// <param name="message">消息对象</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否成功</returns>
    public Task<Boolean> ProduceAsync(String mobile, T message, CancellationToken cancellationToken = default)
    {
        _queue ??= _cacheProvider.GetQueue<T>(Topic, _group);

        return Task.FromResult(_queue.Add(message) > 0);
    }
}

/// <summary>基于 Redis Stream 的消息消费者</summary>
/// <typeparam name="T">消息类型</typeparam>
public class RedisMsgConsumer<T> : IMsgConsumer<T>, IDisposable
{
    private readonly ICacheProvider _cacheProvider;
    private readonly String _group;
    private readonly CancellationTokenSource _cts = new();
    private volatile Boolean _running;
    private Task? _task;

    /// <summary>主题名称</summary>
    public String Topic { get; }

    /// <summary>空闲轮询间隔（秒）</summary>
    public Int32 PollInterval { get; set; } = 1;

    /// <summary>日志</summary>
    public ILog Log { get; set; } = Logger.Null;

    /// <summary>实例化</summary>
    /// <param name="cacheProvider">缓存服务</param>
    /// <param name="topic">主题</param>
    /// <param name="group">消费组</param>
    public RedisMsgConsumer(ICacheProvider cacheProvider, String topic, String group = "Group")
    {
        _cacheProvider = cacheProvider;
        Topic = topic;
        _group = group;
    }

    /// <summary>订阅消息</summary>
    /// <param name="onMessage">消息处理回调</param>
    /// <param name="cancellationToken">取消令牌</param>
    public Task SubscribeAsync(Func<String, T, Task> onMessage, CancellationToken cancellationToken = default)
    {
        if (onMessage == null) throw new ArgumentNullException(nameof(onMessage));

        _running = true;
        _task = Task.Run(() => ConsumeAsync(onMessage, cancellationToken), cancellationToken);

        return Task.FromResult(0);
    }

    /// <summary>取消订阅</summary>
    /// <param name="cancellationToken">取消令牌</param>
    public async Task UnsubscribeAsync(CancellationToken cancellationToken = default)
    {
        _running = false;
        _cts.Cancel();

        var task = _task;
        _task = null;
        if (task != null)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (OperationCanceledException) { }
        }
    }

    private async Task ConsumeAsync(Func<String, T, Task> onMessage, CancellationToken cancellationToken)
    {
        using var source = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _cts.Token);
        var token = source.Token;
        var queue = _cacheProvider.GetQueue<T>(Topic, _group);

        while (_running && !token.IsCancellationRequested)
        {
            try
            {
                // 有界等待，保证取消订阅时能及时退出；Redis 实现不使用超时参数
                var message = queue.TakeOne(PollInterval);
                if (message == null)
                {
                    // 空闲时降低轮询频率，避免空转打满 Redis
                    await Task.Delay(PollInterval * 1000, token).ConfigureAwait(false);
                    continue;
                }

                await onMessage(String.Empty, message).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                Log?.Error("Consume error: Topic={0}, {1}", Topic, ex.Message);
                try
                {
                    await Task.Delay(PollInterval * 1000, token).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }
    }

    /// <summary>释放资源</summary>
    public void Dispose()
    {
        _running = false;
        _cts.Cancel();
    }
}

/// <summary>基于 Redis 的会话通知生产者</summary>
public class RedisSessionProducer : ISessionProducer
{
    private readonly ICacheProvider _cacheProvider;
    private IProducerConsumer<SessionEvent>? _queue;

    /// <summary>会话事件主题</summary>
    public String Topic { get; }

    /// <summary>消费组</summary>
    public String Group { get; }

    /// <summary>节点标识</summary>
    public String? Node { get; set; }

    /// <summary>实例化</summary>
    /// <param name="cacheProvider">缓存服务</param>
    /// <param name="topic">会话事件主题</param>
    /// <param name="group">消费组</param>
    public RedisSessionProducer(ICacheProvider cacheProvider, String topic = "SessionEvent", String group = "Group")
    {
        _cacheProvider = cacheProvider;
        Topic = topic;
        Group = group;
    }

    /// <summary>通知终端上线</summary>
    /// <param name="mobile">终端手机号</param>
    /// <param name="cancellationToken">取消令牌</param>
    public Task ProduceOnlineAsync(String mobile, CancellationToken cancellationToken = default) => PublishAsync(mobile, true);

    /// <summary>通知终端离线</summary>
    /// <param name="mobile">终端手机号</param>
    /// <param name="cancellationToken">取消令牌</param>
    public Task ProduceOfflineAsync(String mobile, CancellationToken cancellationToken = default) => PublishAsync(mobile, false);

    private Task PublishAsync(String mobile, Boolean online)
    {
        if (mobile.IsNullOrEmpty()) return Task.FromResult(0);

        _queue ??= _cacheProvider.GetQueue<SessionEvent>(Topic, Group);
        _queue.Add(new SessionEvent { Mobile = mobile, Online = online, Node = Node, Time = DateTime.Now });

        return Task.FromResult(0);
    }
}

/// <summary>基于 Redis 的会话通知消费者</summary>
public class RedisSessionConsumer : ISessionConsumer, IDisposable
{
    private readonly RedisMsgConsumer<SessionEvent> _consumer;

    /// <summary>会话事件主题</summary>
    public String Topic => _consumer.Topic;

    /// <summary>实例化</summary>
    /// <param name="cacheProvider">缓存服务</param>
    /// <param name="topic">会话事件主题</param>
    /// <param name="group">消费组</param>
    public RedisSessionConsumer(ICacheProvider cacheProvider, String topic = "SessionEvent", String group = "Group") =>
        _consumer = new RedisMsgConsumer<SessionEvent>(cacheProvider, topic, group);

    /// <summary>订阅会话通知</summary>
    /// <param name="onOnline">终端上线回调</param>
    /// <param name="onOffline">终端离线回调</param>
    /// <param name="cancellationToken">取消令牌</param>
    public Task SubscribeAsync(Func<String, Task> onOnline, Func<String, Task> onOffline, CancellationToken cancellationToken = default) =>
        _consumer.SubscribeAsync(async (mobile, e) =>
        {
            if (e == null || e.Mobile.IsNullOrEmpty()) return;

            if (e.Online)
                await onOnline(e.Mobile).ConfigureAwait(false);
            else
                await onOffline(e.Mobile).ConfigureAwait(false);
        }, cancellationToken);

    /// <summary>取消订阅</summary>
    /// <param name="cancellationToken">取消令牌</param>
    public Task UnsubscribeAsync(CancellationToken cancellationToken = default) => _consumer.UnsubscribeAsync(cancellationToken);

    /// <summary>释放资源</summary>
    public void Dispose() => _consumer.Dispose();
}
