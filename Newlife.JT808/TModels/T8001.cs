using Newlife.JT808.Protocols;
using Newlife.JT808.TModels;

namespace Newlife.JT808.Models;

/// <summary>
/// 平台通用应答
/// </summary>
[MessageKind(MessageKinds.平台通用应答)]
public class T8001
{
    /// <summary>
    /// 应答消息流水号
    /// </summary>
    public UInt16 Sequence { get; set; }

    /// <summary>
    /// 应答消息Id
    /// </summary>
    public MessageKinds Kind { get; set; }

    /// <summary>
    /// 返回结果
    /// </summary>
    public ResultKinds Result { get; set; }

    /// <summary>已重载。</summary>
    /// <returns></returns>
    public override String ToString() => $"Response [{Kind}] {Result}";
}