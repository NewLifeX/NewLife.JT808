using NewLife.JT808.Protocols;

namespace NewLife.JT808.JT1078;

/// <summary>实时音视频传输请求</summary>
/// <remarks>
/// 平台向终端请求建立实时音视频传输，指定服务器地址、端口、通道和码流类型。
/// 消息ID：0x9101
/// </remarks>
[MessageKind(MessageKinds.音视频传输请求)]
public class T9101
{
    /// <summary>服务器IP</summary>
    public String? Server { get; set; }

    /// <summary>TCP端口</summary>
    public UInt16 TcpPort { get; set; }

    /// <summary>UDP端口</summary>
    public UInt16 UdpPort { get; set; }

    /// <summary>逻辑通道号</summary>
    public Byte Channel { get; set; }

    /// <summary>数据类型</summary>
    public MediaKinds MediaKind { get; set; }

    /// <summary>码流类型</summary>
    public CodeStreams CodeStream { get; set; }
}
