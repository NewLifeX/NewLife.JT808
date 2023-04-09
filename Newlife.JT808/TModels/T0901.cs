using Newlife.JT808.Protocols;
using NewLife.Serialization;

namespace Newlife.JT808.Models
{
    /// <summary>
    /// 数据压缩上报
    /// 0x0901
    /// </summary>
    [MessageKind(MessageKinds.数据压缩上报)]
    public class M0901
    {
        /// <summary>未压缩消息长度</summary>
        public UInt32 UnCompressMessageLength { get; set; }

        /// <summary>
        /// 未压缩消息体
        /// 压缩消息体为需要压缩的消息经过 GZIP 压缩算法后的消息
        /// </summary>
        [FieldSize(nameof(UnCompressMessageLength))]
        public Byte[] UnCompressMessage { get; set; }
    }
}