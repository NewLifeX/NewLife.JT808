using NewLife.JT808.Protocols;

namespace NewLife.JT808.TModels
{
    /// <summary>
    /// 提问应答
    /// 0x0302
    /// 2019版本已作删除
    /// </summary>
    [MessageKind(MessageKinds.提问应答)]
    public class T0302
    {
        /// <summary>
        /// 应答流水号
        /// 对应的提问下发消息的流水号
        /// </summary>
        public UInt16 ReplySNo { get; set; }

        /// <summary>
        /// 答案 ID 
        /// 提问下发中附带的答案 ID
        /// </summary>
        public Byte AnswerId { get; set; }
    }
}