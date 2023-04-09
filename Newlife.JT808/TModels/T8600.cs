using Newlife.JT808.Protocols;

namespace Newlife.JT808.Models
{
    /// <summary>
    /// 设置圆形区域
    /// 0x8600
    /// 注：本条消息协议支持周期时间范围，如要限制每天的8:30-18:00，起始/结束时间设为：00-00-00-08-30-00/00-00-00-18-00-00，其他以此类推
    /// </summary>
    [MessageKind(MessageKinds.设置圆形区域)]
    public class T8600
    {
        /// <summary>设置属性</summary>
        public Byte SettingAreaProperty { get; set; }
    }
}