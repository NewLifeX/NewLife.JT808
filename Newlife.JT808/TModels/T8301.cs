using NewLife.JT808.Protocols;

namespace NewLife.JT808.Models
{
    /// <summary>
    /// 事件设置
    /// 0x8301
    /// 2019版本已作删除
    /// </summary>
    [MessageKind(MessageKinds.事件设置)]
    public class T8301
    {
        /// <summary>
        /// 设置类型
        /// <see cref="JT808.Protocol.Enums.JT808EventSettingType"/>
        /// </summary>
        public Byte SettingType { get; set; }

        /// <summary>设置总数</summary>
        public Byte SettingCount { get; set; }
    }
}