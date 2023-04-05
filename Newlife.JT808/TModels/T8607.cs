using Newlife.JT808.Protocols;

namespace Newlife.JT808.Models;

/// <summary>
/// 删除路线
/// 0x8607
/// </summary>
[MessageKind(MessageKinds.删除路线)]
public class T8607
{
    /// <summary>
    /// 区域ID集合
    /// </summary>
    public List<UInt32> AreaIds { get; set; }
}