using Newlife.JT808.Protocols;
using NewLife.Serialization;

namespace Newlife.JT808.Models;

/// <summary>
/// 平台RSA公钥
/// 0x8A00
/// </summary>
[MessageKind(MessageKinds.平台RSA公钥)]
public class T8A00
{
    /// <summary>
    /// e
    /// 平台 RSA 公钥{e,n}中的 e
    /// </summary>
    public UInt32 E { get; set; }

    /// <summary>
    /// n
    /// RSA 公钥{e,n}中的 n
    /// </summary>
    [FieldSize(128)]
    public Byte[] N { get; set; }
}