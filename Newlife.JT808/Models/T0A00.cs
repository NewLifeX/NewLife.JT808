using NewLife.JT808.Protocols;
using NewLife.Serialization;

namespace NewLife.JT808.Models;

/// <summary>终端RSA公钥</summary>
/// <remarks>
/// 终端上传的 RSA 公钥 {e, n}，用于平台对下行的关键数据进行加密。
/// 消息ID：0x0A00
/// </remarks>
[MessageKind(MessageKinds.终端RSA公钥)]
public class T0A00
{
    /// <summary>RSA 公钥中的 e</summary>
    public UInt32 E { get; set; }

    /// <summary>RSA 公钥中的 n（128字节）</summary>
    [FieldSize(128)]
    public Byte[] N { get; set; }
}
