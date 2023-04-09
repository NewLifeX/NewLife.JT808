using Newlife.JT808.Protocols;

namespace Newlife.JT808.Models
{
    /// <summary>
    /// 设置多边形区域
    /// 0x8604
    /// </summary>
    [MessageKind(MessageKinds.设置多边形区域)]
    public class T8604
    {
        /// <summary>
        /// 区域 ID
        /// </summary>
        public UInt32 AreaId { get; set; }

        /// <summary>
        /// 区域属性
        /// </summary>
        public UInt16 AreaProperty { get; set; }

        /// <summary>
        /// 起始时间
        /// YY-MM-DD-hh-mm-ss，若区域属性 0 位为 0 则没有该字段
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// YY-MM-DD-hh-mm-ss，若区域属性 0 位为 0 则没有该字段
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 最高速度
        /// Km/h，若区域属性 1 位为 0 则没有该字段
        /// </summary>
        public UInt16? HighestSpeed { get; set; }

        /// <summary>
        /// 超速持续时间 
        /// 单位为秒（s）（类似表述，同前修改），若区域属性 1 位为 0 则没有该字段
        /// </summary>
        public Byte? OverspeedDuration { get; set; }

        /// <summary>
        /// 区域总顶点数
        /// </summary>
        public UInt16 PeakCount { get; set; }

        ///// <summary>
        ///// 顶点项
        ///// </summary>
        //public List<PeakProperty> PeakItems { get; set; }

        /// <summary>
        /// 夜间最高速度
        /// 2019版本
        /// </summary>
        public UInt16 NightMaximumSpeed { get; set; }

        /// <summary>
        /// 名称长度
        /// 2019版本
        /// </summary>
        public UInt16 NameLength { get; set; }

        /// <summary>
        /// 区域名称
        /// 2019版本
        /// </summary>
        public String AreaName { get; set; }
    }
}