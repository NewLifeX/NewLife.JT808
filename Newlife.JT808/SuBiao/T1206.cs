using Newlife.JT808.Protocols;

namespace Newlife.JT808.SuBiao
{
    /// <summary>文件上传完成通知</summary>
    [MessageKind(MessageKinds.文件上传完成通知)]
    public class T1206
    {
        /// <summary>流水号</summary>
        public Int16 SerialNumber { get; set; }

        /// <summary>结果。0成功，1失败</summary>
        public Byte Result { get; set; }
    }
}