using JT808.Server.Messaging.RocketMQ;
using NewLife;
using NewLife.JT808.Protocols;
using NewLife.RocketMQ;

namespace JT808.Server.Messaging;

/// <summary>基于 RocketMQ 的外部消息总线</summary>
/// <remarks>
/// 复用 <see cref="RocketMQProducer{TMessage}"/>、<see cref="RocketMQConsumer{TMessage}"/>、
/// <see cref="RocketMQSessionAdapter"/>、<see cref="RocketMQDownHandler"/> 适配器，
/// 支持阿里云/华为云/腾讯云/ACL 四种云厂商认证。
/// </remarks>
public class RocketMQMessageBus : IMessageBus
{
    private readonly MessagingOptions _options;

    /// <summary>提供方</summary>
    public MessagingProviders Provider => MessagingProviders.RocketMQ;

    /// <summary>NameServer 地址</summary>
    public String NameServer => _options.NameServer ?? throw new InvalidOperationException("RocketMQ NameServer is not configured.");

    /// <summary>分组前缀</summary>
    public String Group => _options.ExternalGroup;

    /// <summary>云厂商适配器</summary>
    public ICloudProvider? CloudProvider { get; }

    /// <summary>实例化</summary>
    /// <param name="options">配置</param>
    public RocketMQMessageBus(MessagingOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        CloudProvider = CreateCloudProvider(options);
    }

    /// <summary>创建消息生产者</summary>
    /// <typeparam name="T">消息类型</typeparam>
    /// <param name="topic">主题</param>
    /// <returns>生产者</returns>
    public IMsgProducer<T> CreateProducer<T>(String topic) => new RocketMQProducer<T>(NameServer, topic, $"{Group}_PID_{topic}", CloudProvider);

    /// <summary>创建消息消费者</summary>
    /// <typeparam name="T">消息类型</typeparam>
    /// <param name="topic">主题</param>
    /// <returns>消费者</returns>
    public IMsgConsumer<T> CreateConsumer<T>(String topic) => new RocketMQConsumer<T>(NameServer, topic, $"{Group}_CID_{topic}", CloudProvider);

    /// <summary>创建会话通知生产者</summary>
    /// <returns>会话通知生产者</returns>
    public ISessionProducer CreateSessionProducer() => new RocketMQSessionAdapter(NameServer, _options.SessionTopic, $"{Group}_PID_{_options.SessionTopic}", CloudProvider);

    /// <summary>创建会话通知消费者</summary>
    /// <returns>RocketMQ 未提供会话消费者适配器，返回 null</returns>
    public ISessionConsumer? CreateSessionConsumer() => null;

    /// <summary>创建外部下行指令处理器</summary>
    /// <returns>下行指令处理器</returns>
    public IDownMessageHandler CreateDownHandler() => new RocketMQDownHandler(NameServer, _options.DownTopic, $"{Group}_CID_{_options.DownTopic}", CloudProvider);

    private static ICloudProvider? CreateCloudProvider(MessagingOptions options)
    {
        if (options.Cloud.IsNullOrEmpty() || options.Cloud.EqualIgnoreCase("None")) return null;

        if (options.Cloud.EqualIgnoreCase("Aliyun"))
            return new AliyunProvider { AccessKey = options.AccessKey, SecretKey = options.SecretKey, InstanceId = options.InstanceId };
        if (options.Cloud.EqualIgnoreCase("Huawei"))
            return new HuaweiProvider { AccessKey = options.AccessKey, SecretKey = options.SecretKey, InstanceId = options.InstanceId };
        if (options.Cloud.EqualIgnoreCase("Tencent"))
            return new TencentProvider { AccessKey = options.AccessKey, SecretKey = options.SecretKey, Namespace = options.InstanceId };
        if (options.Cloud.EqualIgnoreCase("Acl"))
            return new AclProvider { AccessKey = options.AccessKey, SecretKey = options.SecretKey };

        throw new InvalidOperationException($"Unsupported RocketMQ cloud provider: {options.Cloud}.");
    }
}
