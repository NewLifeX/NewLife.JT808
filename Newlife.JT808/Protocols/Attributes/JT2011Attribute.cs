using NewLife.Serialization;

namespace NewLife.JT808.Protocols;

/// <summary>仅 JT/T 808-2011 版本使用的字段</summary>
/// <remarks>
/// 标记该特性的字段在非 2011 版本（Version != "2011"）时自动跳过。
/// 与 <see cref="JT2019Attribute"/> 对应，用于处理三个版本的字段差异。
/// </remarks>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class JT2011Attribute : AccessorAttribute
{
    /// <summary>从数据流中读取消息</summary>
    /// <param name="formatter">序列化</param>
    /// <param name="context">上下文</param>
    /// <returns>是否成功</returns>
    public override Boolean Read(IFormatterX formatter, AccessorContext context) => formatter is Binary bn && bn.Version != "2011";

    /// <summary>把消息写入到数据流中</summary>
    /// <param name="formatter">序列化</param>
    /// <param name="context">上下文</param>
    public override Boolean Write(IFormatterX formatter, AccessorContext context) => formatter is Binary bn && bn.Version != "2011";
}
