using Newlife.JT808.Protocols;

namespace Newlife.JT808.TModels;

/// <summary>查询区域或线路数据应答</summary>
[MessageKind(MessageKinds.查询区域或线路数据应答)]
public class T0608
{
    /// <summary>查询类型</summary>
    public Byte QueryType { get; set; }

    ///// <summary>
    ///// 查询的区域或线路的ID数量
    ///// </summary>
    //public UInt32 Count { get; set; }

    /// <summary>
    /// 查询的区域或线路的ID
    /// </summary>
    public List<UInt32> Ids { get; set; }
}