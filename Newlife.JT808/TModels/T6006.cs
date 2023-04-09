using NewLife.JT808.Protocols;
using NewLife.Serialization;

namespace NewLife.JT808.Models
{
    /// <summary>文本信息下发响应</summary>
    [MessageKind(MessageKinds.文本信息下发响应)]
    public class T6006 //: IAccessor
    {
        /// <summary>结果。0:成功</summary>
        public Byte Result { get; set; }

        /// <summary>
        /// 文本信息
        /// 最长为 1024 字节，经GBK编码
        /// </summary>
        [FullString]
        public String Text { get; set; }
    }
}