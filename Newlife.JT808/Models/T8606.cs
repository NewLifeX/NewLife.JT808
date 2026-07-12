using NewLife.JT808.Protocols;

namespace NewLife.JT808.Models
{
    /// <summary>
    /// 设置路线
    /// 0x8606
    /// </summary>
    [MessageKind(MessageKinds.设置路线)]
    public class T8606
    {
        /// <summary>路线 ID</summary>
        public UInt32 RouteId { get; set; }

        /// <summary>
        /// 路线属性
        /// 路线属性数据格式见 表 67
        /// </summary>
        public UInt16 RouteProperty { get; set; }

        /// <summary>
        /// 起始时间
        /// YY-MM-DD-hh-mm-ss，若区域属性 0 位为 0 则没有该字段
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// YY-MM-DD-hh-mm-ss，若区域属性 0 位为 0 则没有该字段
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>路线总拐点数</summary>
        public UInt16 InflectionPointCount { get; set; }

        /// <summary>路线名称</summary>
        public String RouteName { get; set; }
    }
}