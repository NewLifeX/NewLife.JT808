using NewLife.JT808.Protocols;

namespace NewLife.JT808.Models
{
    /// <summary>查询区域或线路数据应答</summary>
    [MessageKind(MessageKinds.查询区域或线路数据应答)]
    public class T0608
    {
        /// <summary>查询类型</summary>
        public Byte QueryType { get; set; }

        /// <summary>查询的区域或线路的ID</summary>
        public List<UInt32> Ids { get; set; }
    }
}