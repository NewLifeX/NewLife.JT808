using NewLife.JT808.Protocols;
using NewLife.Serialization;

namespace NewLife.JT808.Models
{
    /// <summary>
    /// 信息服务
    /// 0x8304
    /// 2019版本已作删除
    /// </summary>
    [MessageKind(MessageKinds.信息服务)]
    public class T8304
    {
        /// <summary>类型</summary>
        public Byte Type { get; set; }

        /// <summary>长度</summary>
        public UInt16 Length { get; set; }

        /// <summary>
        /// 信息内容
        /// 经 GBK 编码
        /// </summary>
        [FieldSize(nameof(Length))]
        public String Content { get; set; }
    }
}