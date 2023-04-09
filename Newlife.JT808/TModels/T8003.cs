using NewLife.JT808.Protocols;
using NewLife.Serialization;

namespace NewLife.JT808.Models
{
    /// <summary>补传分包请求</summary>
    [MessageKind(MessageKinds.补传分包请求)]
    public class T8003
    {
        /// <summary>
        /// 原始消息流水号
        /// 对应要求补传的原始消息第一包的消息流水号
        /// </summary>
        public UInt16 OriginalMsgNum { get; set; }

        /// <summary>
        /// 重传包总数
        /// 2013 byte
        /// 2019 ushort
        /// </summary>
        public UInt16 Count { get; set; }

        /// <summary>
        /// 重传包 ID 列表
        /// BYTE[2*n]
        /// 重传包序号顺序排列，如“包 ID1 包 ID2......包 IDn”。
        /// </summary>
        [FieldSize(0, nameof(Count))]
        public Byte[] Data { get; set; }
    }
}