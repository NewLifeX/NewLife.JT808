using NewLife.JT808.Serialization;

namespace NewLife.JT808.SuBiao
{
    /// <summary>DSM告警类型</summary>
    public enum DSMKinds : Byte
    {
        /// <summary>疲劳驾驶</summary>
        疲劳驾驶 = 0x01,

        /// <summary>拨打电话</summary>
        拨打电话 = 0x02,

        /// <summary>抽烟</summary>
        抽烟 = 0x03,

        /// <summary>分神驾驶，长时间不目视前方报警</summary>
        分神驾驶 = 0x04,

        /// <summary>驾驶员异常，未检测到驾驶员报警</summary>
        驾驶员异常 = 0x05,

        /// <summary>探头遮挡，粤标。苏标，双手同时脱离方向盘报警</summary>
        探头遮挡 = 0x06,

        /// <summary>打哈欠，锐明。苏标，驾驶员行为监测功能失效报警</summary>
        打哈欠 = 0x07,

        /// <summary>超时驾驶。粤标</summary>
        超时驾驶 = 0x08,

        /// <summary>未系安全带。粤标</summary>
        未系安全带 = 0x0A,

        /// <summary>红外阻断型墨镜失效。粤标</summary>
        红外阻断 = 0x0B,

        /// <summary>双手同时脱离方向盘。粤标</summary>
        双手脱把 = 0x0C,

        /// <summary>玩手机。粤标</summary>
        玩手机 = 0x0D,

        /// <summary>自动抓拍</summary>
        自动抓拍 = 0x10,

        /// <summary>驾驶员变更</summary>
        驾驶员变更 = 0x11,

        /// <summary>阻挡摄像头。锐明</summary>
        阻挡摄像头 = 0x13,
    }

    /// <summary>驾驶状态监测系统报警信息</summary>
    public class DSMInfo
    {
        #region 属性
        /// <summary>报警ID。按照报警先后，从 0 开始循环累加，不区分报警类型。</summary>
        public Int32 Id { get; set; }

        /// <summary>标志状态</summary>
        public AlarmFlags Flag { get; set; }

        /// <summary>报警/事件类型</summary>
        public DSMKinds Kind { get; set; }

        /// <summary>报警级别。0x01 一级报警，0x02 二级报警</summary>
        public Byte Level { get; set; }

        /// <summary>疲劳程度。范围 1~10。数值越大表示疲劳程度越严重，仅在报警类型为 0x01 时有效</summary>
        public Byte Fatigue { get; set; }

        /// <summary>预留</summary>
        public Int32 Reserved { get; set; }

        /// <summary>车速。单位 Km/h。范围 0~250</summary>
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