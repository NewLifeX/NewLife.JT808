using NewLife.JT808.Protocols;
using NewLife.Serialization;

namespace NewLife.JT808.Models
{
    /// <summary>文本标志</summary>
    [Flags]
    public enum TextFlags : Byte
    {
        /// <summary>服务。2011/2013版为紧急</summary>
        Service = 1,

        /// <summary>紧急。服务+紧急=通知</summary>
        Urgent = 2,

        /// <summary>终端显示器错误</summary>
        ScreenError = (1 << 2),

        /// <summary>终端TTS播读</summary>
        TTS = (1 << 3),

        /// <summary>广告牌显示</summary>
        AdvertisingScreen = (1 << 4),

        /// <summary>0:中心导航信息。1:CAN故障码信息</summary>
        NavigationOrCANError = (1 << 5),

        /// <summary>保留</summary>
        Keep6 = (1 << 6),

        /// <summary>保留</summary>
        Keep7 = (1 << 7),
    }

    /// <summary>文本类型</summary>
    public enum TextKinds : Byte
    {
        /// <summary>通知</summary>
        Notice = 1,

        /// <summary>服务</summary>
        Service = 2,
    }

    /// <summary>文本信息下发</summary>
    [MessageKind(MessageKinds.文本信息下发)]
    public class T8300 //: IAccessor
    {
        /// <summary>
        /// 文本信息标志位含义见 表 38
        /// </summary>
        public TextFlags Flag { get; set; }

        /// <summary>
        /// 文本类型
        /// 1=通知,2=服务
        /// 2019版本
        /// </summary>
        [JT2019]
        public TextKinds Kind { get; set; }

        /// <summary>
        /// 文本信息
        /// 最长为 1024 字节，经GBK编码
        /// </summary>
        [FullString]
        public String Text { get; set; }
    }
}