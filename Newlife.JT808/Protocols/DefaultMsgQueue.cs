using NewLife.Log;
using NewLife.Threading;
#if NET45
using TaskEx = System.Threading.Tasks.Task;
#endif

namespace NewLife.JT808.Protocols;

/// <summary>基于内存队列的默认消息生产者/消费者实现</summary>
/// <remarks>
/// 无需外部依赖（如 Kafka），适用于单机部署或开发测试。
/// 生产环境中可替换为 Kafka、RabbitMQ 等实现。
/// </remarks>
/// <typeparam name="TMessage">消息类型</typeparam>
public class DefaultMsgQueue<TMessage> : IMsgProducer<TMessage>, IMsgConsumer<TMessage>, IDisposable
{
    #region 属性
    /// <summary>主题名称</summary>
    public String Topic { get; }

    /// <summary>日志</summary>
    public ILog Log { get; set; } = Logger.Null;

    /// <summary>最大队列长度</summary>
    public Int32 MaxQueueLength { get; set; } = 10000;

    private readonly Queue<(String, TMessage)> _queue = new();
    private Func<String, TMessage, Task>? _handler;
    private TimerX? _timer;
    private volatile Boolean _running;
    #endregion

    #region 构造
    /// <summary>实例化默认内存队列</summary>
    /// <param name="topic">主题名称</param>
    public DefaultMsgQueue(String topic) => Topic = topic;
    #endregion

    #region IMsgProducer
    /// <summary>生产消息到内存队列</summary>
    public Task<Boolean> ProduceAsync(String mobile, TMessage message, CancellationToken cancellationToken = default)
    {
        if (_queue.Count >= MaxQueueLength)
        {
            Log?.Warn("队列已满，丢弃消息：Topic={0}, Mobile={1}", Topic, mobile);
            return Task.FromResult(false);
        }

        lock (_queue)
        {
            _queue.Enqueue((mobile, message));
        }

        return Task.FromResult(true);
    }
    #endregion

    #region IMsgConsumer
    /// <summary>订阅消息</summary>
    public Task SubscribeAsync(Func<String, TMessage, Task> onMessage, CancellationToken cancellationToken = default)
    {
        _handler = onMessage;
        _running = true;

        // 启动定时处理
        _timer ??= new TimerX(DoProcess, null, 0, 100) { Async = true };

        return Task.FromResult(0);
    }

    /// <summary>取消订阅</summary>
    public Task UnsubscribeAsync(CancellationToken cancellationToken = default)
    {
        _running = false;
        _handler = null;
        _timer.TryDispose();
        _timer = null;

        return Task.FromResult(0);
    }
    #endregion

    #region 处理
    private async Task DoProcess(Object state)
    {
        while (_running && _queue.Count > 0)
        {
            (String mobile, TMessage message) item;
            lock (_queue)
            {
                if (_queue.Count == 0) break;
                item = _queue.Dequeue();
            }

            try
            {
                if (_handler != null)
                    await _handler(item.mobile, item.message);
            }
            catch (Exception ex)
            {
                Log?.Error("消息处理异常：Topic={0}, Mobile={1}", Topic, item.mobile, ex);
            }
        }
    }
    #endregion

    #region IDisposable
    /// <summary>销毁</summary>
    public void Dispose()
    {
        _running = false;
        _timer.TryDispose();
        _timer = null;
        _handler = null;
        lock (_queue) _queue.Clear();
    }
    #endregion
}
