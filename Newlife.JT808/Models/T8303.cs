using NewLife.JT808.Protocols;

namespace NewLife.JT808.Models
{
    /// <summary>
    /// 信息点播菜单设置
    /// 0x8303
    /// </summary>
    [MessageKind(MessageKinds.信息点播菜单设置)]
    public class T8303
    {
        /// <summary>设置类型</summary>
        public Byte SettingType { get; set; }
    }
}