using NewLife.JT808.Protocols;

namespace NewLife.JT808.Models
{
    /// <summary>
    /// 设置矩形区域
    /// 0x8602
    /// </summary>
    [MessageKind(MessageKinds.设置矩形区域)]
    public class T8602
    {
        /// <summary>设置属性</summary>
        public Byte SettingAreaProperty { get; set; }
    }
}