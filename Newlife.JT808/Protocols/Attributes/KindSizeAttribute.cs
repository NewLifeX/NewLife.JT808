namespace NewLife.JT808.Protocols;

/// <summary>标注 TLV/KLV 枚举字段对应的字节大小</summary>
/// <remarks>
/// 在 TLV/KLV 的 <c>Convert&lt;TKind&gt;()</c> 中用于确定值类型的字节长度。
/// </remarks>
public class KindSizeAttribute : Attribute
{
    /// <summary>字节大小</summary>
    public Int32 Size { get; set; }

    /// <summary>指定枚举字段对应的字节大小</summary>
    /// <param name="size">字节数</param>
    public KindSizeAttribute(Int32 size) => Size = size;
}
