using NewLife.JT808.Protocols;

namespace NewLife.JT808.JT1078;

/// <summary>平台下发远程录像回放控制</summary>
/// <remarks>
/// 平台对正在回放的录像进行控制：开始/暂停/结束/快进/快退/拖动/关键帧播放。
/// 消息ID：0x9202
/// </remarks>
[MessageKind(MessageKinds.录像回放控制)]
public class T9202
{
    /// <summary>逻辑通道号</summary>
    public Byte Channel { get; set; }

    /// <summary>回放控制。0：开始；1：暂停；2：结束；3：快进；4：关键帧快退；5：拖动；6：关键帧播放</summary>
    public Byte Control { get; set; }

    /// <summary>快进或快退倍数。Control为3或4时有效：0无效，1~5：1/2/4/8/16倍</summary>
    public Byte Speed { get; set; }

    /// <summary>拖动回放位置（单位为秒）</summary>
    public UInt32 DragPos { get; set; }
}
