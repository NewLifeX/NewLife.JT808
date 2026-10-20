using NewLife;
using NewLife.JT808.Protocols;

namespace JT808.Server.Messaging;

/// <summary>消息总线配置</summary>
/// <remarks>
/// 平台内部传输固定使用 Redis（在线会话与指令路由都依赖它）；
/// <see cref="External"/> 用于接入 RocketMQ/Kafka，把设备数据镜像到外部系统、广播上下线事件并接收外部下行指令。
/// 配置节：<c>JT808:Messaging</c>。
/// </remarks>
/// <example>
/// <code>
/// {
///   "JT808": {
///     "Messaging": {
///       "External": "RocketMQ",
///       "NameServer": "127.0.0.1:9876",
///       "Group": "JT808",
///       "SessionEvents": true,
///       "Downlink": true
///     }
///   }
/// }
/// </code>
/// </example>
public class MessagingOptions
{
    /// <summary>外部集成提供方。None 表示关闭（默认）</summary>
    public MessagingProviders External { get; set; } = MessagingProviders.None;

    /// <summary>RocketMQ NameServer 地址</summary>
    public String? NameServer { get; set; }

    /// <summary>Kafka 服务器地址</summary>
    public String? BootstrapServers { get; set; }

    /// <summary>外部集成分组前缀。生产者组/消费组在此基础上追加主题与用途，不影响 Redis 内部消费组</summary>
    public String ExternalGroup { get; set; } = "JT808";

    /// <summary>RocketMQ 云厂商。None/Aliyun/Huawei/Tencent/Acl</summary>
    public String? Cloud { get; set; }

    /// <summary>云厂商 AccessKey</summary>
    public String? AccessKey { get; set; }

    /// <summary>云厂商 SecretKey</summary>
    public String? SecretKey { get; set; }

    /// <summary>云厂商实例编号</summary>
    public String? InstanceId { get; set; }

    /// <summary>会话事件主题</summary>
    public String SessionTopic { get; set; } = "JT808Session";

    /// <summary>外部下行指令主题</summary>
    public String DownTopic { get; set; } = "JT808Command";

    /// <summary>是否广播终端上下线事件</summary>
    public Boolean SessionEvents { get; set; } = true;

    /// <summary>是否消费外部下行指令</summary>
    public Boolean Downlink { get; set; } = true;

    /// <summary>从配置读取</summary>
    /// <param name="configuration">配置</param>
    /// <param name="section">配置节，默认 JT808:Messaging</param>
    /// <returns>配置实例</returns>
    public static MessagingOptions Load(IConfiguration configuration, String section = "JT808:Messaging")
    {
        var options = new MessagingOptions();
        if (configuration == null) return options;

        var provider = configuration[$"{section}:External"];
        if (!provider.IsNullOrEmpty() && Enum.TryParse<MessagingProviders>(provider, true, out var external))
            options.External = external;

        options.NameServer = configuration[$"{section}:NameServer"];
        options.BootstrapServers = configuration[$"{section}:BootstrapServers"];
        options.Cloud = configuration[$"{section}:Cloud"];
        options.AccessKey = configuration[$"{section}:AccessKey"];
        options.SecretKey = configuration[$"{section}:SecretKey"];
        options.InstanceId = configuration[$"{section}:InstanceId"];

        var group = configuration[$"{section}:ExternalGroup"];
        if (!group.IsNullOrEmpty()) options.ExternalGroup = group;

        var sessionTopic = configuration[$"{section}:SessionTopic"];
        if (!sessionTopic.IsNullOrEmpty()) options.SessionTopic = sessionTopic;

        var downTopic = configuration[$"{section}:DownTopic"];
        if (!downTopic.IsNullOrEmpty()) options.DownTopic = downTopic;

        options.SessionEvents = configuration[$"{section}:SessionEvents"].ToBoolean(true);
        options.Downlink = configuration[$"{section}:Downlink"].ToBoolean(true);

        return options;
    }

    /// <summary>校验配置</summary>
    /// <returns>错误信息，配置有效时返回 null</returns>
    public String? Validate()
    {
        switch (External)
        {
            case MessagingProviders.None:
            case MessagingProviders.Redis:
            case MessagingProviders.Memory:
                return null;
            case MessagingProviders.RocketMQ:
                return NameServer.IsNullOrEmpty() ? "JT808:Messaging:NameServer is required for RocketMQ." : null;
            case MessagingProviders.Kafka:
                return BootstrapServers.IsNullOrEmpty() ? "JT808:Messaging:BootstrapServers is required for Kafka." : null;
            default:
                return $"Unsupported external provider: {External}.";
        }
    }
}
