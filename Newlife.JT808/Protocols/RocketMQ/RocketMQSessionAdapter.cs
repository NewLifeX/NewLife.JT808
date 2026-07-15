using NewLife.Log;
using NewLife.RocketMQ;
using NewLife.Serialization;

namespace NewLife.JT808.Protocols.RocketMQ;

/// <summary>基于 RocketMQ 的会话通知适配器</summary>
/// <remarks>
/// 将终端的上下线事件通过 RocketMQ 主题广播。
/// 上线通知发送 "online" 消息，下线通知发送 "offline" 消息。
/// </remarks>
public class RocketMQSessionAdapter : ISessionProducer, IDisposable
{
    #region 属性
    /// <summary>RocketMQ 生产者实例</summary>
    public Producer Producer { get; }

    /// <summary>会话通知主题</summary>
    public String Topic { get; set; } = "SessionEvent";
    #endregion

    #region 构造
    /// <summary>实例化 RocketMQ 会话通知适配器</summary>
    /// <param name="nameServer">NameServer 地址</param>
    /// <param name="topic">会话通知主题</param>
    /// <param name="group">生产者组</param>
    /// <param name="cloudProvider">云厂商适配器。阿里云传 <c>new AliyunProvider{AccessKey=...,SecretKey=...,InstanceId=...}</c>；华为云传 <c>new HuaweiProvider{...}</c>；腾讯云传 <c>new TencentProvider{...}</c>；ACL 传 <c>new AclProvider{...}</c></param>
    public RocketMQSessionAdapter(String nameServer, String? topic = null, String? group = null, NewLife.RocketMQ.ICloudProvider? cloudProvider = null)
    {
        if (topic != null) Topic = topic;
        Producer = new Producer
        {
            NameServerAddress = nameServer,
            Topic = Topic,
            Group = group ?? "PID_SessionEvent",
            CloudProvider = cloudProvider,
        };
        Producer.Start();
    }
    #endregion

    #region 方法
    /// <summary>通知终端上线</summary>
    public async Task ProduceOnlineAsync(String mobile, CancellationToken cancellationToken = default)
    {
        await Producer.PublishAsync(new { mobile, action = "online", time = DateTime.UtcNow }, null, mobile);
    }

    /// <summary>通知终端离线</summary>
    public async Task ProduceOfflineAsync(String mobile, CancellationToken cancellationToken = default)
    {
        await Producer.PublishAsync(new { mobile, action = "offline", time = DateTime.UtcNow }, null, mobile);
    }

    /// <summary>释放资源</summary>
    public void Dispose()
    {
        if (Producer?.Active == true)
            Producer.Stop();
    }
    #endregion
}
