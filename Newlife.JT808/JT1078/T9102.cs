using NewLife.JT808.Protocols;

namespace NewLife.JT808.JT1078;

/// <summary>音视频实时传输控制</summary>
/// <remarks>
/// 平台对终端的实时音视频流进行控制：关闭、切换码流、暂停/恢复、关闭对讲。
/// 消息ID：0x9102
/// </remarks>
[MessageKind(MessageKinds.音视频传输控制)]
public class T9102
{
    /// <summary>逻辑通道号</summary>
    public Byte Channel { get; set; }

    /// <summary>控制指令。0：关闭；1：切换码流；2：暂停；3：恢复；4：关闭双向对讲</summary>
    public StreamCommands Command { get; set; }

    /// <summary>关闭音视频类型。0：全部；1：仅音频；2：仅视频</summary>
    public CloseKinds CloseKind { get; set; }

    /// <summary>切换码流类型。0：主码流；1：子码流</summary>
    public CodeStreams CodeStream { get; set; }
}
