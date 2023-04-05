using Newlife.JT808.Protocols;

namespace JT808.Models;

/// <summary>
/// 设置终端参数
/// </summary>
[MessageKind(MessageKinds.设置终端参数)]
public class T8103
{
    /// <summary>
    /// 参数列表
    /// </summary>
    public List<TLV> Items { get; set; }
}