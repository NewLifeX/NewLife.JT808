using NewLife.JT808.Protocols;

namespace NewLife.JT808.JT1078;

/// <summary>查询资源列表</summary>
/// <remarks>
/// 平台查询终端指定通道、时间段、报警类型的音视频资源列表。
/// 消息ID：0x9205
/// </remarks>
[MessageKind(MessageKinds.查询资源列表)]
public class T9205
{
    /// <summary>逻辑通道号</summary>
    public Byte Channel { get; set; }

    /// <summary>开始时间</summary>
    [BCDTime(6, "yyMMddHHmmss")]
    public DateTime StartTime { get; set; }

    /// <summary>结束时间</summary>
    [BCDTime(6, "yyMMddHHmmss")]
    public DateTime EndTime { get; set; }

    /// <summary>报警标志</summary>
    public UInt64 AlarmFlags { get; set; }

    /// <summary>音视频资源类型</summary>
    public MediaFileKinds FileKind { get; set; }

    /// <summary>码流类型</summary>
    public CodeStreams2 CodeStream { get; set; }

    /// <summary>存储器类型</summary>
    public StorageKinds StorageKind { get; set; }
}
