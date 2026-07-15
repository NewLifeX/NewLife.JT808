using NewLife.Log;
using NewLife.RocketMQ;
using NewLife.Serialization;
#if NET45
using TaskEx = System.Threading.Tasks.Task;
#endif

namespace NewLife.JT808.Protocols.RocketMQ;

/// <summary>基于 RocketMQ 的消息消费者适配器</summary>
/// <remarks>
/// 将 RocketMQ 的 Consumer 包装为 JT808 的 IMsgConsumer 接口。
/// 消费时反序列化为 TMessage，从消息 keys 中提取 mobile。
/// </remarks>
/// <typeparam name="TMessage">消息类型</typeparam>
public class RocketMQConsumer<TMessage> : IMsgConsumer<TMessage>, IDisposable
{
    #region 属性
    /// <summary>RocketMQ 消费者实例</summary>
    public Consumer Consumer { get; }

    /// <summary>主题名称</summary>
    public String Topic { get; }

    /// <summary>是否已启动</summary>
    public Boolean Active => Consumer?.Active ?? false;
    #endregion

    #region 构造
    /// <summary>实例化 RocketMQ 消费者适配器</summary>
    /// <param name="nameServer">NameServer 地址</param>
    /// <param name="topic">主题名称</param>
    /// <param name="group">消费组</param>
    /// <param name="cloudProvider">云厂商适配器。阿里云传 <c>new AliyunProvider{AccessKey=...,SecretKey=...,InstanceId=...}</c>；华为云传 <c>new HuaweiProvider{...}</c>；腾讯云传 <c>new TencentProvider{...}</c>；ACL 传 <c>new AclProvider{...}</c></param>
    public RocketMQConsumer(String nameServer, String topic, String group, NewLife.RocketMQ.ICloudProvider? cloudProvider = null)
    {
        Topic = topic;
        Consumer = new Consumer
        {
            NameServerAddress = nameServer,
            Topic = topic,
            Group = group,
            CloudProvider = cloudProvider,
        };
    }

    /// <summary>实例化 RocketMQ 消费者适配器</summary>
    /// <param name="consumer">已配置的 Consumer 实例（未启动）</param>
    /// <param name="topic">主题名称</param>
    public RocketMQConsumer(Consumer consumer, String topic)
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
        Consumer.OnConsumeAsync = async (queue, messages, ct) =>
        {
            foreach (var msg in messages)
            {
                try
                {
                    // 从 keys 中提取 mobile
                    var mobile = msg.Keys ?? String.Empty;

                    // 反序列化消息体
                    var body = default(TMessage);
                    if (msg.BodyString != null)
                    {
                        if (typeof(TMessage) == typeof(String))
                            body = (TMessage)(Object)msg.BodyString;
                        else
                            body = JsonHelper.ToJsonEntity<TMessage>(msg.BodyString);
                    }

                    if (body != null)
                        await onMessage(mobile, body);
                }
                catch (Exception ex)
                {
                    Log?.Error("RocketMQ 消费异常: {0}", ex.Message);
                    return false;
                }
            }
            return true;
        };

        Consumer.Start();
        return Task.FromResult(0);
    }

    /// <summary>取消订阅</summary>
    public Task UnsubscribeAsync(CancellationToken cancellationToken = default)
    {
        if (Consumer?.Active == true)
            Consumer.Stop();

        return Task.FromResult(0);
    }

    /// <summary>释放资源</summary>
    public void Dispose()
    {
        if (Consumer?.Active == true)
            Consumer.Stop();
    }

    /// <summary>日志</summary>
    public ILog Log { get; set; } = Logger.Null;
    #endregion
}
