using Newlife.JT808.Protocols;
using NewLife.Serialization;

namespace Newlife.JT808.Models;

/// <summary>
/// 电子运单上报
/// 0x0701
/// </summary>
[MessageKind(MessageKinds.电子运单上报)]
public class T0701
{
    /// <summary>长度</summary>
    public UInt32 Length { get; set; }

    /// <summary>内容</summary>
    [FieldSize(nameof(Length))]
    public Byte[] Body { get; set; }
}