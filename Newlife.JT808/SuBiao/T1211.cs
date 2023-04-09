using Newlife.JT808.Protocols;

namespace Newlife.JT808.SuBiao
{
    /// <summary>文件类型</summary>
    public enum T1211_FileTypes : Byte
    {
        /// <summary>图片</summary>
        图片 = 0x00,

        /// <summary>音频</summary>
        音频 = 0x01,

        /// <summary>视频</summary>
        视频 = 0x02,

        /// <summary>文本</summary>
        文本 = 0x03,

        /// <summary>其它</summary>
        其它 = 0x04,
    }

    /// <summary>文件信息上传</summary>
    [MessageKind(MessageKinds.文件信息上传)]
    public class T1211
    {
        /// <summary>文件名</summary>
        public String FileName { get; set; }

        /// <summary>文件类型</summary>
        public T1211_FileTypes FileType { get; set; }

        /// <summary>文件大小</summary>
        public Int32 FileSize { get; set; }
    }
}