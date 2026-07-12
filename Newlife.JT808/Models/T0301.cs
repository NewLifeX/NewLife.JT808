using NewLife.JT808.Protocols;

namespace NewLife.JT808.Models
{
    /// <summary>
    /// 事件报告
    /// 0x0301
    /// 2019版本已作删除
    /// </summary>
    [MessageKind(MessageKinds.事件报告)]
    public class T0301
    {
        /// <summary>事件编号</summary>
        public byte EventId { get; set; }
    }
}