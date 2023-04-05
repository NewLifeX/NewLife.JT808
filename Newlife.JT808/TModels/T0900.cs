using Newlife.JT808.Protocols;
using Newlife.JT808.UtilTool;

namespace Newlife.JT808.Models;

/// <summary>
/// 数据上行透传
/// </summary>
[MessageKind(MessageKinds.数据上行透传)]
public class T0900 //: IAccessor
{
    /// <summary>
    /// 透传消息类型
    /// </summary>
    public DataKinds Kind { get; set; }

    /// <summary>
    /// 透传数据
    /// </summary>
    [FullBytes]
    public Byte[] Data { get; set; }
}