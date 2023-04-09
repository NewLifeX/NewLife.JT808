using Newlife.JT808.Protocols;
using NewLife.Serialization;

namespace Newlife.JT808.SuBiao
{
    /// <summary>报警附件上传</summary>
    [MessageKind(MessageKinds.报警附件上传)]
    public class T9208
    {
        /// <summary>服务器IP</summary>
        public String Server { get; set; }

        /// <summary>Tcp端口</summary>
        public UInt16 TcpPort { get; set; }

        /// <summary>Udp端口</summary>
        public UInt16 UdpPort { get; set; }

        /// <summary>报警标识号</summary>
        [FieldSize(16)]
        public Byte[] AlarmId { get; set; }

        /// <summary>报警编号。平台给报警分配的唯一编号</summary>
        [FieldSize(32)]
        public String PlatformId { get; set; }

        /// <summary>预留</summary>
        [FieldSize(16)]
        public Byte[] Reversed { get; set; }
    }
}