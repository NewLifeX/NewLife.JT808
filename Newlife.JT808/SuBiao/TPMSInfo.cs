using NewLife.JT808.Protocols;

namespace NewLife.JT808.SuBiao
{
    /// <summary>胎压监测系统报警信息</summary>
    public class TPMSInfo
    {
        #region 属性
        /// <summary>报警ID。按照报警先后，从 0 开始循环累加，不区分报警类型。</summary>
        public Int32 Id { get; set; }

        /// <summary>标志状态</summary>
        public Byte Flag { get; set; }

        /// <summary>车速</summary>
        public Byte Speed { get; set; }

        /// <summary>海拔高度，单位为米（m）</summary>
        public Int16 Altitude { get; set; }

        /// <summary>纬度。以度为单位的纬度值乘以 10 的 6 次方，精确到百万分之一度</summary>
        public Int32 Latitude { get; set; }

        /// <summary>经度。以度为单位的纬度值乘以 10 的 6 次方，精确到百万分之一度</summary>
        public Int32 Longitude { get; set; }

        /// <summary>YY-MM-DD-hh-mm-ss（GMT+8 时间）</summary>
        [BCDTime(6, "yyMMddHHmmss")]
        public DateTime Time { get; set; }

        /// <summary>车辆状态</summary>
        public CarStatus Status { get; set; }

        /// <summary>报警标识号</summary>
        public AlarmDevice Device { get; set; }

        /// <summary>报警/事件 列表总数</summary>
        public Byte Count { get; set; }

        /// <summary>报警/事件 信息列表</summary>
        public TPMSItem[] Items { get; set; }
        #endregion
    }

    /// <summary>胎压监测系统报警/事件信息列表</summary>
    public class TPMSItem
    {
        /// <summary>胎压报警位置</summary>
        public UInt16 Id { get; set; }

        /// <summary>报警/事件类型</summary>
        public UInt16 Kind { get; set; }

        /// <summary>胎压。单位 Kpa</summary>
        public UInt16 Pressure { get; set; }

        /// <summary>胎温。单位 °C</summary>
        public UInt16 Temperature { get; set; }

        /// <summary>电量。电池电量，单位 %</summary>
        public UInt16 Battery { get; set; }
    }
}