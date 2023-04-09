using Newlife.JT808.Protocols;

namespace Newlife.JT808.Models
{
    /// <summary>
    /// 摄像头立即拍摄命令
    /// 0x8801
    /// </summary>
    [MessageKind(MessageKinds.摄像头立即拍摄命令)]
    public class T8801
    {
        /// <summary>通道 ID</summary>
        public Byte ChannelId { get; set; }

        /// <summary>
        /// 拍摄命令 
        /// 0 表示停止拍摄；0xFFFF 表示录像；其它表示拍照张数
        /// </summary>
        public UInt16 ShootingCommand { get; set; }

        /// <summary>
        /// 拍照间隔/录像时间
        /// 秒，0 表示按最小间隔拍照或一直录像
        /// </summary>
        public UInt16 VideoTime { get; set; }

        /// <summary>
        /// 保存标志 
        /// 1：保存；0：实时上传
        /// 仅主机拍照时有效
        /// </summary>
        public Byte SaveFlag { get; set; }

        /// <summary>分辨率</summary>
        public Byte Resolution { get; set; }

        /// <summary>
        /// 图像/视频质量
        /// 1-10，1 代表质量损失最小，10 表示压缩比最大
        /// </summary>
        public Byte VideoQuality { get; set; }

        /// <summary>
        /// 亮度
        /// 0-255
        /// </summary>
        public Byte Lighting { get; set; }

        /// <summary>
        /// 对比度
        /// 0-127
        /// </summary>
        public Byte Contrast { get; set; }

        /// <summary>
        /// 饱和度
        /// 0-127
        /// </summary>
        public Byte Saturability { get; set; }

        /// <summary>
        /// 色度
        /// 0-255
        /// </summary>
        public Byte Chroma { get; set; }
    }
}