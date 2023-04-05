using Newlife.JT808.Protocols;

namespace Newlife.JT808.Models;

/// <summary>
/// 单条存储多媒体数据检索上传命令
/// </summary>
[MessageKind(MessageKinds.单条存储多媒体数据检索上传命令)]
public class T8805
{
    /// <summary>
    /// 多媒体ID
    /// </summary>
    public UInt32 Id { get; set; }
    /// <summary>
    /// 删除标志
    /// </summary>
    public Byte Deleted { get; set; }
}