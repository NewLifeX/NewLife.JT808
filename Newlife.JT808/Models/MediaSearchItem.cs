using NewLife.JT808.Protocols;

namespace NewLife.JT808.Models
{
    /// <summary>多媒体检索项</summary>
    /// <remarks>0x0802 存储多媒体数据检索应答的检索项，含多媒体标识与对应位置数据。</remarks>
    public class MediaSearchItem
    {
        /// <summary>多媒体ID</summary>
        public UInt32 MediaId { get; set; }

        /// <summary>多媒体类型</summary>
        public MediaTypes MediaType { get; set; }

        /// <summary>通道ID</summary>
        public Byte ChannelId { get; set; }

        /// <summary>
        /// 事件项编码
        /// 0：平台下发指令；1：定时动作；2：抢劫报警触发；3：碰撞侧翻报警触发；其他保留
        /// </summary>
        public Byte EventItemCoding { get; set; }

        /// <summary>位置数据</summary>
        public T0200? Position { get; set; }
    }
}
