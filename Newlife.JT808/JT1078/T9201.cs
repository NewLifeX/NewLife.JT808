using NewLife.JT808.Protocols;

namespace NewLife.JT808.JT1078;

/// <summary>平台下发远程录像回放请求</summary>
/// <remarks>
/// 平台请求终端上传指定条件的录像文件。
/// 消息ID：0x9201
/// </remarks>
[MessageKind(MessageKinds.录像回放请求)]
public class T9201
{
    /// <summary>服务器IP</summary>
    public String? Server { get; set; }

    /// <summary>TCP端口</summary>
    public UInt16 TcpPort { get; set; }

    /// <summary>UDP端口</summary>
    public UInt16 UdpPort { get; set; }

    /// <summary>逻辑通道号</summary>
    public Byte Channel { get; set; }

    /// <summary>音视频资源类型</summary>
    public MediaFileKinds FileKind { get; set; }

    /// <summary>码流类型</summary>
    public CodeStreams2 CodeStream { get; set; }

    /// <summary>存储器类型</summary>
    public StorageKinds StorageKind { get; set; }

    /// <summary>回放方式。0：正常；1：快进；2：关键帧快退；3：关键帧播放；4：单帧上传</summary>
    public Byte Mode { get; set; }

    /// <summary>快进或快退倍数。回放方式为1和2时有效：0无效，1~5：1/2/4/8/16倍</summary>
    public Byte Speed { get; set; }

    /// <summary>开始时间</summary>
    [BCDTime(6, "yyMMddHHmmss")]
    public DateTime StartTime { get; set; }

    /// <summary>结束时间</summary>
    [BCDTime(6, "yyMMddHHmmss")]
    public DateTime EndTime { get; set; }

    /// <summary>报警标志</summary>
    public UInt64 AlarmFlags { get; set; }
}
