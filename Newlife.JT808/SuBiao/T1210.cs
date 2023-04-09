using NewLife.JT808.Protocols;
using NewLife.Serialization;

namespace NewLife.JT808.SuBiao
{
    /// <summary>信息类型</summary>
    public enum T1210_InfoTypes : Byte
    {
        /// <summary>正常报警文件信息</summary>
        正常 = 0x00,

        /// <summary>补传报警文件信息</summary>
        补传 = 0x01,
    }

    /// <summary>报警附件信息</summary>
    [MessageKind(MessageKinds.报警附件信息)]
    public class T1210
    {
        /// <summary>终端号</summary>
        [FieldSize(7)]
        public String TerminalId { get; set; }

        /// <summary>报警标识号</summary>
        [FieldSize(16)]
        public Byte[] AlarmId { get; set; }

        /// <summary>报警编号。平台给报警分配的唯一编号</summary>
        [FieldSize(32)]
        public String PlatformId { get; set; }

        /// <summary>信息类型。0正常，1补传</summary>
        public T1210_InfoTypes Type { get; set; }

        /// <summary>报警信息列表</summary>
        public AttachmentInfo[] Attachments { get; set; }
    }

    /// <summary>报警附件信息</summary>
    public class AttachmentInfo
    {
        /// <summary>文件名</summary>
        public String FileName { get; set; }

        /// <summary>文件大小</summary>
        public Int32 FileSize { get; set; }
    }
}