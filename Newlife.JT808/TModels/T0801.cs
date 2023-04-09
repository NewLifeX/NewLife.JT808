using NewLife.JT808.Protocols;
using NewLife.JT808.TModels;

namespace NewLife.JT808.Models
{
    /// <summary>
    /// 多媒体数据上传
    /// 0x0801
    /// </summary>
    [MessageKind(MessageKinds.多媒体数据上传)]
    public class T0801
    {
        /// <summary>多媒体 ID</summary>
        public UInt32 MultimediaId { get; set; }

        /// <summary>多媒体类型</summary>
        public Byte MultimediaType { get; set; }

        /// <summary>
        /// 多媒体格式编码 
        /// 0：JPEG；1：TIF；2：MP3；3：WAV；4：WMV；其他保留
        /// </summary>
        public Byte MultimediaCodingFormat { get; set; }

        /// <summary>事件项编码</summary>
        public Byte EventItemCoding { get; set; }

        /// <summary>通道 ID</summary>
        public Byte ChannelId { get; set; }

        /// <summary>定位错误</summary>
        public Boolean PositionError { get; set; }

        /// <summary>
        /// 位置信息汇报(0x0200)消息体
        /// 表示拍摄或录制的起始时刻的位置基本信息数据
        /// </summary>
        public T0200 Position { get; set; }

        /// <summary>多媒体数据包</summary>
        public Byte[] MultimediaDataPackage { get; set; }
    }
}