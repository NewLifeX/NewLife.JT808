#if !NET45
using Confluent.Kafka;
using NewLife.Log;

namespace NewLife.JT808.Protocols.Kafka;

/// <summary>基于 Kafka 的下行指令处理器</summary>
/// <remarks>
/// 消费 Kafka 指令队列中的下行指令，将字符串类型的 BodyData 转换为 Byte[]。
/// 配合 CommandClient 使用，CommandClient 向 Kafka 发送指令，此处理器接收并转换。
/// 需要 .NET Standard 2.0+ 或 .NET Framework 4.6.1+ 运行时支持。
/// </remarks>
public class KafkaDownHandler : IDownMessageHandler, IDisposable
{
    #region 属性
    /// <summary>Kafka 消费者实例</summary>
    public IConsumer<String, String> Consumer { get; }

    /// <summary>指令主题</summary>
    public String Topic { get; }

    /// <summary>是否已启动</summary>
    public Boolean Active => _active;
    private Boolean _active;

    private Func<String, Byte[], Task<Byte[]>>? _handler;
    #endregion

    #region 构造
    /// <summary>实例化 Kafka 下行指令处理器</summary>
    /// <param name="bootstrapServers">Kafka 服务器地址（如 localhost:9092）</param>
    /// <param name="topic">指令主题</param>
    /// <param name="group">消费组</param>
    /// <param name="config">额外消费者配置。若为 null 则使用默认配置</param>
    public KafkaDownHandler(String bootstrapServers, String topic, String group, ConsumerConfig? config = null)
    {
        Topic = topic;

        var cfg = config ?? new ConsumerConfig();
        cfg.BootstrapServers = bootstrapServers;
        cfg.GroupId = group;
        cfg.AutoOffsetReset = AutoOffsetReset.Earliest;
        cfg.EnableAutoCommit = true;

        Consumer = new ConsumerBuilder<String, String>(cfg).Build();
    }
    #endregion

    #region 方法
    /// <summary>处理下行消息</summary>
    /// <param name="mobile">目标终端手机号</param>
    /// <param name="data">消息体序列化数据</param>
    /// <returns>处理结果</returns>
    public Task<Byte[]> HandleAsync(String mobile, Byte[] data)
    {
        return Task.FromResult(data);
    }

    /// <summary>开始消费指令队列</summary>
    /// <param name="onMessage">消费回调：mobile, data → 结果</param>
    public void Start(Func<String, Byte[], Task<Byte[]>> onMessage)
    {
        _handler = onMessage;

        _active = true;
        Consumer.Subscribe(Topic);

        _ = ConsumeLoopAsync();
    }

    private async Task ConsumeLoopAsync()
    {
        while (_active)
        {
            try
            {
                var result = Consumer.Consume(TimeSpan.FromMilliseconds(500));
                if (result == null) continue;

                var mobile = result.Message.Key ?? String.Empty;
                var data = result.Message.Value != null ?
                    System.Text.Encoding.UTF8.GetBytes(result.Message.Value) :
                    [];

                if (_handler != null)
                    await _handler(mobile, data);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                Log?.Error("Kafka command consume error: {0}", ex.Message);
                await Task.Delay(1000);
            }
        }
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
