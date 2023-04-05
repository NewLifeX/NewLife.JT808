using Newlife.JT808.Protocols;

namespace Newlife.JT808.Models;

/// <summary>
/// 删除矩形区域
/// 0x8603
/// </summary>
[MessageKind(MessageKinds.删除矩形区域)]
public class T8603 
{
    /// <summary>
    /// 区域ID集合
    /// </summary>
    public List<UInt32> AreaIds { get; set; }
}