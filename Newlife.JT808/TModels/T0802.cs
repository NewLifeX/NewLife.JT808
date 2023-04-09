using NewLife.JT808.Protocols;
using NewLife.JT808.UtilTool;

namespace NewLife.JT808.Models
{
    /// <summary>
    /// 存储多媒体数据检索应答
    /// 0x0802
    /// </summary>
    [MessageKind(MessageKinds.存储多媒体数据检索应答)]
    public class T0802
    {
        /// <summary>
        /// 应答流水号
        /// 对应的多媒体数据检索消息的流水号
        /// </summary>
        public UInt16 MsgNum { get; set; }

        ///// <summary>
        ///// 多媒体数据总项数
        ///// 满足检索条件的多媒体数据总项数
        ///// </summary>
        //public UInt16 MultimediaItemCount { get; set; }

        /// <summary>
        /// 检索项集合
        /// </summary>
        public List<MediaSearchProperty> MediaProperties { get; set; }
    }
}