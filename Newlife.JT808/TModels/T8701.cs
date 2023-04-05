using Newlife.JT808.Protocols;

namespace Newlife.JT808.Models;

/// <summary>
/// 行驶记录参数下传命令
/// </summary>
[MessageKind(MessageKinds.行驶记录参数下传命令)]
public class T8701
{
    /// <summary>
    /// 命令Id
    /// </summary>
    public Byte CommandId { get; set; }

    /// <summary>
    /// 行驶记录参数下传命令包
    /// </summary>
    public Byte[] Data { get; set; }
}