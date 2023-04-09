using NewLife.JT808.Protocols;

namespace NewLife.JT808.Models
{
    /// <summary>录音开始命令</summary>
    [MessageKind(MessageKinds.录音开始命令)]
    public class T8804
    {
        /// <summary>
        /// 录音命令
        /// 0：停止录音；0x01：开始录音；
        /// </summary>
        public Byte RecordCmd { get; set; }

        /// <summary>
        /// 单位为秒（s），0 表示一直录音
        /// </summary>
        public UInt16 RecordTime { get; set; }

        /// <summary>
        /// 保存标志
        /// 0：实时上传；1：保存
        /// </summary>
        public Byte RecordSave { get; set; }

        /// <summary>
        /// 音频采样率
        /// 0：8K；1：11K；2：23K；3：32K；其他保留
        /// </summary>
        public Byte AudioSampleRate { get; set; }
    }
}