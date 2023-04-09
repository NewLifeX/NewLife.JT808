using NewLife.JT808.Protocols;

namespace NewLife.JT808.Models
{
    /// <summary>
    /// 行驶记录仪数据上传
    /// 0x0700
    /// </summary>
    [MessageKind(MessageKinds.行驶记录数据上传)]
    public class T0700
    {
        /// <summary>应答流水号</summary>
        public UInt16 Sequence { get; set; }

        /// <summary>命令字</summary>
        public MessageKinds Kind { get; set; }

        /// <summary>
        /// 行车记录仪上行数据包
        /// </summary>
        [FullBytes]
        public Byte[] Data { get; set; }
    }
}