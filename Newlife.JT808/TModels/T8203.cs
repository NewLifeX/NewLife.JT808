using Newlife.JT808.Protocols;

namespace Newlife.JT808.Models;

/// <summary>
/// 人工确认报警消息
/// 0x8203
/// </summary>
[MessageKind(MessageKinds.人工确认报警消息)]
public class T8203
{
    /// <summary>
    /// 报警消息流水号
    /// 需人工确认的报警消息流水号，0 表示该报警类型所有消息
    /// </summary>
    public UInt16 AlarmMsgNum { get; set; }

    /// <summary>
    /// 人工确认报警类型
    /// </summary>
    public UInt32 ManualConfirmAlarmType { get; set; }
}