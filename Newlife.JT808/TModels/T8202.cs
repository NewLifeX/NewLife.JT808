using Newlife.JT808.Protocols;

namespace Newlife.JT808.Models
{
    /// <summary>临时位置跟踪控制</summary>
    [MessageKind(MessageKinds.临时位置跟踪控制)]
    public class T8202
    {
        /// <summary>
        /// 时间间隔
        /// 单位为秒（s），0 则停止跟踪。停止跟踪无需带后继字段
        /// </summary>
        public UInt16 Interval { get; set; }

        /// <summary>
        /// 位置跟踪有效期
        /// 单位为秒（s），终端在接收到位置跟踪控制消息后，在有效期截止时间之前，依据消息中的时间间隔发送位置汇报
        /// </summary>
        public Int32 LocationTrackingValidity { get; set; }
    }
}