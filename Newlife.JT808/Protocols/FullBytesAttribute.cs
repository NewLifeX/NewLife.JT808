using NewLife;
using NewLife.Reflection;
using NewLife.Serialization;

namespace NewLife.JT808.Protocols;

/// <summary>完全字节数组序列化特性。指示数据流剩下部分全部作为字节数组读写</summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class FullBytesAttribute : AccessorAttribute
{
    public String Version { get; set; }

    /// <summary>从数据流中读取消息</summary>
    /// <param name="formatter">序列化</param>
    /// <param name="context">上下文</param>
    /// <returns>是否成功</returns>
    public override Boolean Read(IFormatterX formatter, AccessorContext context)
    {
        if (formatter is Binary bn && (Version.IsNullOrEmpty() || bn.Version == Version))
        {
            var buf = bn.Stream.ReadBytes(-1);

            context.Value.SetValue(context.Member, buf);

            return true;
        }

        return false;
    }

    /// <summary>把消息写入到数据流中</summary>
    /// <param name="formatter">序列化</param>
    /// <param name="context">上下文</param>
    public override Boolean Write(IFormatterX formatter, AccessorContext context)
    {
        if (formatter is Binary bn && (Version.IsNullOrEmpty() || bn.Version == Version))
        {
            if (context.Value.GetValue(context.Member) is Byte[] buf)
            {
                bn.Write(buf, 0, buf.Length);
            }

            return true;
        }

        return false;
    }

}
