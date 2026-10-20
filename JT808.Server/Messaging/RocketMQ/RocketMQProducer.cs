using NewLife.JT808.Protocols;
using NewLife.Log;
using NewLife.RocketMQ;

namespace JT808.Server.Messaging.RocketMQ;

/// <summary>基于 RocketMQ 的消息生产者适配器</summary>
/// <remarks>
/// 将 RocketMQ 的 Producer 包装为 JT808 的 IMsgProducer 接口。
/// 消息体被序列化为 JSON 字符串后发送，mobile 编码到消息的 keys 中。
/// </remarks>
/// <typeparam name="TMessage">消息类型</typeparam>
public class RocketMQProducer<TMessage> : IMsgProducer<TMessage>, IDisposable
{
    #region 属性
    /// <summary>RocketMQ 生产者实例</summary>
    public Producer Producer { get; }

    /// <summary>主题名称</summary>
    public String Topic { get; }

    /// <summary>是否已启动</summary>
    public Boolean Active => Producer?.Active ?? false;
    #endregion

    #region 构造
    /// <summary>实例化 RocketMQ 生产者适配器</summary>
    /// <param name="nameServer">NameServer 地址</param>
    /// <param name="topic">主题名称</param>
    /// <param name="group">生产者组</param>
    /// <param name="cloudProvider">云厂商适配器。阿里云传 <c>new AliyunProvider{AccessKey=...,SecretKey=...,InstanceId=...}</c>；华为云传 <c>new HuaweiProvider{...}</c>；腾讯云传 <c>new TencentProvider{...}</c>；ACL 传 <c>new AclProvider{...}</c></param>
    public RocketMQProducer(String nameServer, String topic, String? group = null, NewLife.RocketMQ.ICloudProvider? cloudProvider = null)
    {
        Topic = topic;
        Producer = new Producer
        {
            NameServerAddress = nameServer,
            Topic = topic,
            Group = group ?? $"PID_{topic}",
            CloudProvider = cloudProvider,
        };
        Producer.Start();
    }

    /// <summary>实例化 RocketMQ 生产者适配器</summary>
    /// <param name="producer">已启动的 Producer 实例</param>
    /// <param name="topic">主题名称</param>
    public RocketMQProducer(Producer producer, String topic)
    {
        Producer = producer;
        Topic = topic;
    }
    #endregion

    #region 方法
    /// <summary>生产消息到 RocketMQ</summary>
    /// <param name="mobile">终端手机号，编码到消息 keys 中</param>
    /// <param name="message">消息对象</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否成功</returns>
    public async Task<Boolean> ProduceAsync(String mobile, TMessage message, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await Producer.PublishAsync(message, null, mobile);
            return result != null;
        }
        catch (Exception ex)
        {
            Log?.Error("RocketMQ produce failed: {0}", ex.Message);
            return false;
        }
    }

    /// <summary>释放资源</summary>
    public void Dispose()
    {
        if (Producer?.Active == true)
            Producer.Stop();
    }

    /// <summary>日志</summary>
    public ILog Log { get; set; } = Logger.Null;
    #endregion
}
