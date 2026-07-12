namespace NewLife.JT808.Protocols;

/// <summary>跨服务器指令下发事件模型</summary>
/// <remarks>
/// 当终端设备未连接到当前服务器时，通过事件总线广播到集群中所有服务器，
/// 由目标设备所连接的服务器负责执行实际的指令下发。
/// </remarks>
public class CommandEvent
{
    /// <summary>目标终端手机号</summary>
    public String? Mobile { get; set; }

    /// <summary>消息体类型全名</summary>
    public String? BodyType { get; set; }

    /// <summary>消息体 JSON 数据</summary>
    public String? BodyData { get; set; }

    /// <summary>来源服务器标识</summary>
    public String? Source { get; set; }
}
