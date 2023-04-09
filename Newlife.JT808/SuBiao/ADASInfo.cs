using NewLife.JT808.Serialization;

namespace NewLife.JT808.SuBiao
{
    /// <summary>ADAS告警类型</summary>
    public enum ADASKinds : Byte
    {
        /// <summary>前向碰撞</summary>
        前向碰撞 = 0x01,

        /// <summary>车道偏离</summary>
        车道偏离 = 0x02,

        /// <summary>车距过近</summary>
        车距过近 = 0x03,

        /// <summary>行人碰撞</summary>
        行人碰撞 = 0x04,

        /// <summary>频繁变道</summary>
        频繁变道 = 0x05,

        /// <summary>道路标识超限</summary>
        道路标识超限 = 0x06,

        /// <summary>障碍物</summary>
        障碍物 = 0x07,

        /// <summary>道路标志识别</summary>
        道路标志识别 = 0x10,

        /// <summary>主动抓拍</summary>
        主动抓拍 = 0x11,
    }

    /// <summary>偏离类型</summary>
    public enum DeviateKinds : Byte
    {
        /// <summary>左侧偏离</summary>
        左侧偏离 = 0x01,

        /// <summary>右侧偏离</summary>
        右侧偏离 = 0x02,
    }

    /// <summary>道路标志识别类型</summary>
    public enum RoadFlags : Byte
    {
        /// <summary>限速</summary>
        限速 = 0x01,

        /// <summary>限高</summary>
        限高 = 0x02,

        /// <summary>限重</summary>
        限重 = 0x03,
    }

    /// <summary>报警状态标识</summary>
    public enum AlarmFlags : Byte
    {
        /// <summary>开始</summary>
        开始 = 0x01,

        /// <summary>结束</summary>
        结束 = 0x02,

        /// <summary>持续</summary>
        持续 = 0x03,
    }

    /// <summary>高级驾驶辅助报警信息</summary>
    public class ADASInfo
    {
        #region 属性
        /// <summary>报警ID。按照报警先后，从 0 开始循环累加，不区分报警类型。</summary>
        public Int32 Id { get; set; }

        /// <summary>标志状态</summary>
        public AlarmFlags Flag { get; set; }

        /// <summary>报警/事件类型</summary>
        public ADASKinds Kind { get; set; }

        /// <summary>报警级别。0x01 一级报警，0x02 二级报警</summary>
        public Byte Level { get; set; }

        /// <summary>前车车速。单位 km/h。范围 0-250，仅报警类型为 0x01 和 0x02 时有效</summary>
        public Byte FrontSpeed { get; set; }

        /// <summary>前车/行人 距离。单位 100ms，范围 0-100，仅报警类型为 0x01、0x02 和 0x04 时有效</summary>
        public Byte FrontDistance { get; set; }

        /// <summary>偏离类型。0x01左侧偏离，0x02右侧偏离</summary>
        public DeviateKinds DeviateKind { get; set; }

        /// <summary>道路标志识别类型</summary>
        public RoadFlags RoadFlag { get; set; }

        /// <summary>道路标志识别数据</summary>
        public Byte RoadData { get; set; }

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