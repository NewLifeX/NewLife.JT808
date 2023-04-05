using Newlife.JT808.Protocols;

namespace Newlife.JT808.Models;

/// <summary>
/// 多媒体数据上传应答
/// 0x8800
/// </summary>
[MessageKind(MessageKinds.多媒体数据上传应答)]
public class T8800
{
    /// <summary>
    /// 多媒体ID
    /// </summary>
    public UInt32 MultimediaId { get; set; }

    /// <summary>
    /// 重传包总数
    /// </summary>
    public Byte RetransmitPackageCount { get; set; }

    /// <summary>
    /// 重传包 ID 列表
    /// 重传包序号顺序排列，如“包 ID1 包 ID2......包 IDn”。
    /// </summary>
    public Byte[] RetransmitPackageIds { get; set; }
}
