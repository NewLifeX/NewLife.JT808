using NewLife.JT808.Protocols;

namespace NewLife.JT808.Models
{
    /// <summary>数据下行透传</summary>
    [MessageKind(MessageKinds.数据下行透传)]
    public class T8900 //:  IAccessor
    {
        /// <summary>
        /// 透传消息类型
        /// 透传消息类型定义见 表 93
        /// </summary>
        public Byte Type { get; set; }

        /// <summary>
        /// 数据下行透传数据
        /// </summary>
        [FullBytes]
        public Byte[] Data { get; set; }
    }
}