using NewLife.JT808.Protocols;

namespace Client.Avalonia.Models;

/// <summary>消息日志条目</summary>
public class MessageLogEntry
{
    /// <summary>时间</summary>
    public DateTime Time { get; set; }

    /// <summary>方向。↑ 发送 / ↓ 接收</summary>
    public String Direction { get; set; } = "";

    /// <summary>消息类型</summary>
    public MessageKinds Kind { get; set; }

    /// <summary>消息名称</summary>
    public String KindName { get; set; } = "";

    /// <summary>终端手机号</summary>
    public String Mobile { get; set; } = "";

    /// <summary>流水号</summary>
    public UInt16 Sequence { get; set; }

    /// <summary>消息长度</summary>
    public Int32 Length { get; set; }

    /// <summary>十六进制转储</summary>
    public String HexDump { get; set; } = "";

    /// <summary>JSON 详情</summary>
    public String JsonBody { get; set; } = "";

    /// <summary>原始消息帧</summary>
    public JTMessage? Message { get; set; }
}
