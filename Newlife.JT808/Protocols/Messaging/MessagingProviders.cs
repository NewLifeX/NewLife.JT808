namespace NewLife.JT808.Protocols;

/// <summary>消息传输提供方</summary>
public enum MessagingProviders
{
    /// <summary>未启用</summary>
    None = 0,

    /// <summary>Redis（平台内部传输，默认）</summary>
    Redis = 1,

    /// <summary>进程内内存队列（单机/开发测试）</summary>
    Memory = 2,

    /// <summary>RocketMQ（外部集成）</summary>
    RocketMQ = 3,

    /// <summary>Kafka（外部集成）</summary>
    Kafka = 4,
}

/// <summary>终端上下线事件</summary>
/// <remarks>网关把终端上线/下线通知投递到会话主题，供外部系统或集群其它节点消费。</remarks>
public class SessionEvent
{
    /// <summary>终端手机号</summary>
    public String? Mobile { get; set; }

    /// <summary>是否上线</summary>
    public Boolean Online { get; set; }

    /// <summary>节点标识</summary>
    public String? Node { get; set; }

    /// <summary>事件时间</summary>
    public DateTime Time { get; set; }
}
