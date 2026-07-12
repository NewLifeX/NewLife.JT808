using NewLife.JT808.Protocols;

namespace NewLife.JT808.SuBiao
{
    /// <summary>BSD告警类型</summary>
    public enum BSDKinds : Byte
    {
        /// <summary>后方接近</summary>
        后方接近 = 0x01,

        /// <summary>左侧后方接近</summary>
        左侧后方接近 = 0x02,

        /// <summary>右侧后方接近</summary>
        右侧后方接近 = 0x03,
    }

    /// <summary>盲区监测系统报警定义</summary>
    public class BSDInfo
    {
        #region 属性
        /// <summary>报警ID。按照报警先后，从 0 开始循环累加，不区分报警类型。</summary>
        public Int32 Id { get; set; }

        /// <summary>标志状态</summary>
        public AlarmFlags Flag { get; set; }

        /// <summary>报警/事件类型</summary>
        public BSDKinds Kind { get; set; }

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
        #endregion
    }
}