using NewLife.JT808.Protocols;

namespace NewLife.JT808.Models
{
    /// <summary>
    /// 信息点播/取消
    /// 0x0303
    /// 2019版本已作删除
    /// </summary>
    [MessageKind(MessageKinds.信息点播_取消)]
    public class T0303
    {
        /// <summary>信息类型</summary>
        public Byte InformationType { get; set; }

        /// <summary>点播/取消标志</summary>
        public Byte Flag { get; set; }
    }
}