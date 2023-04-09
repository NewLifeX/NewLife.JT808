using Newlife.JT808.Protocols;

namespace Newlife.JT808.Models
{
    /// <summary>
    /// 多媒体事件信息上传
    /// 0x0800
    /// </summary>
    [MessageKind(MessageKinds.多媒体事件信息上传)]
    public class T0800
    {
        /// <summary>
        /// 多媒体数据 ID
        /// </summary>
        public UInt32 MultimediaId { get; set; }

        /// <summary>
        /// 多媒体类型
        /// 0：图像；1：音频；2：视频；
        /// </summary>
        public Byte MultimediaType { get; set; }

        /// <summary>
        /// 多媒体格式编码 
        /// 0：JPEG；1：TIF；2：MP3；3：WAV；4：WMV；其他保留
        /// </summary>
        public Byte MultimediaCodingFormat { get; set; }

        /// <summary>
        /// 事件项编码
        /// 0：平台下发指令；
        /// 1：定时动作；
        /// 2：抢劫报警触发；
        /// 3：碰撞侧翻报警触发；
        /// 4：门开拍照；
        /// 5：门关拍照；
        /// 6：车门由开变关，时速从＜20 公里到超过 20 公里；
        /// 7：定距拍照；
        /// 其他保留
        /// </summary>
        public Byte EventItemCoding { get; set; }

        /// <summary>
        /// 通道 ID
        /// </summary>
        public Byte ChannelId { get; set; }
    }
}