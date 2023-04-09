using Newlife.JT808.Protocols;

namespace Newlife.JT808.Models
{
    /// <summary>
    /// 存储多媒体数据上传命令
    /// 0x8803
    /// </summary>
    [MessageKind(MessageKinds.存储多媒体数据上传)]
    public class T8803
    {
        /// <summary>
        /// 多媒体类型 
        /// 0：图像；1：音频；2：视频
        /// </summary>
        public Byte MultimediaType { get; set; }

        /// <summary>通道 ID</summary>
        public Byte ChannelId { get; set; }

        /// <summary>
        /// 事件项编码
        /// 0：平台下发指令；1：定时动作；2：抢劫报警触发；3：碰撞侧翻报警触发；其他保留 
        /// </summary>
        public Byte EventItemCoding { get; set; }

        /// <summary>
        /// 起始时间
        /// YY-MM-DD-hh-mm-ss
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// YY-MM-DD-hh-mm-ss
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>
        public Byte MultimediaDeleted { get; set; }
    }
}