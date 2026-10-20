#if !NET45
using Confluent.Kafka;
using NewLife.Log;
using NewLife.Serialization;

namespace NewLife.JT808.Protocols.Kafka;

/// <summary>基于 Kafka 的消息消费者适配器</summary>
/// <remarks>
/// 将 Confluent.Kafka 的 IConsumer 包装为 JT808 的 IMsgConsumer 接口。
/// 消费时反序列化为 TMessage，从消息 Key 中提取 mobile。
/// 需要 .NET Standard 2.0+ 或 .NET Framework 4.6.1+ 运行时支持。
/// </remarks>
/// <typeparam name="TMessage">消息类型</typeparam>
public class KafkaConsumer<TMessage> : IMsgConsumer<TMessage>, IDisposable
{
    #region 属性
    /// <summary>Kafka 消费者实例</summary>
    public IConsumer<String, String> Consumer { get; }

    /// <summary>主题名称</summary>
    public String Topic { get; }

    /// <summary>是否已启动</summary>
    public Boolean Active => _active;
    private Boolean _active;
    #endregion

    #region 构造
    /// <summary>实例化 Kafka 消费者适配器</summary>
    /// <param name="bootstrapServers">Kafka 服务器地址（如 localhost:9092）</param>
    /// <param name="topic">主题名称</param>
    /// <param name="group">消费组</param>
    /// <param name="config">额外消费者配置。若为 null 则使用默认配置</param>
    public KafkaConsumer(String bootstrapServers, String topic, String group, ConsumerConfig? config = null)
    {
        Topic = topic;

        var cfg = config ?? new ConsumerConfig();
        cfg.BootstrapServers = bootstrapServers;
        cfg.GroupId = group;
        cfg.AutoOffsetReset = AutoOffsetReset.Earliest;
        cfg.EnableAutoCommit = true;

        Consumer = new ConsumerBuilder<String, String>(cfg).Build();
    }

    /// <summary>实例化 Kafka 消费者适配器</summary>
    /// <param name="consumer">已创建的 IConsumer 实例</param>
    /// <param name="topic">主题名称</param>
    public KafkaConsumer(IConsumer<String, String> consumer, String topic)
    {
        Consumer = consumer;
        Topic = topic;
    }
    #endregion

    #region 方法
    /// <summary>订阅消息</summary>
    /// <param name="onMessage">消息处理回调：参数为 mobile 和消息体</param>
    /// <param name="cancellationToken">取消令牌</param>
    public Task SubscribeAsync(Func<String, TMessage, Task> onMessage, CancellationToken cancellationToken = default)
    {
        _active = true;

        Consumer.Subscribe(Topic);

        // 启动后台消费循环
        _ = ConsumeLoopAsync(onMessage, cancellationToken);

        return Task.CompletedTask;
    }

    private async Task ConsumeLoopAsync(Func<String, TMessage, Task> onMessage, CancellationToken cancellationToken)
    {
        while (_active && !cancellationToken.IsCancellationRequested)
        {
            try
            {
                var result = Consumer.Consume(cancellationToken);
                if (result == null) continue;

                var mobile = result.Message.Key ?? String.Empty;
                var value = result.Message.Value;

                var body = default(TMessage);
                if (value != null)
                {
                    if (typeof(TMessage) == typeof(String))
                        body = (TMessage)(Object)value;
                    else
                        body = JsonHelper.ToJsonEntity<TMessage>(value);
                }

                if (body != null)
                    await onMessage(mobile, body);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
            Log?.Error("Kafka consume error: {0}", ex.Message);
                await Task.Delay(1000, cancellationToken);
            }
        }
    }

    /// <summary>取消订阅</summary>
    public Task UnsubscribeAsync(CancellationToken cancellationToken = default)
    {
        _active = false;
        try
        {
            Consumer.Unsubscribe();
        }
        catch { }
        return Task.CompletedTask;
    }

    /// <summary>释放资源</summary>
    public void Dispose()
    {
        _active = false;
        try
        {
            Consumer.Unsubscribe();
            Consumer.Close();
            Consumer.Dispose();
        }
        catch { }
    }

    /// <summary>日志</summary>
    public ILog Log { get; set; } = Logger.Null;
    #endregion
}
#endif
