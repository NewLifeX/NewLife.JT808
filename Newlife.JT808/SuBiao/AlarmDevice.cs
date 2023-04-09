using System.Globalization;
using NewLife.Serialization;

namespace Newlife.JT808.SuBiao
{
    /// <summary>报警标识号</summary>
    public class AlarmDevice : IAccessor
    {
        /// <summary>终端ID。苏标7字节，粤标30字节</summary>
        public String DeviceId { get; set; }

        /// <summary>YY-MM-DD-hh-mm-ss（GMT+8 时间）</summary>
        public DateTime Time { get; set; }

        /// <summary>序号</summary>
        public Byte Sequence { get; set; }

        /// <summary>附件数量</summary>
        public Byte Attachments { get; set; }

        /// <summary>预留</summary>
        public UInt16 Reserved { get; set; }

        /// <summary>读取</summary>
        /// <param name="stream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Boolean Read(Stream stream, Object context)
        {
            var binary = new Binary { Stream = stream };

            // 苏标16字节，粤标40字节
            var retain = stream.Length - stream.Position;
            var len = retain >= 40 ? 30 : 7;
            DeviceId = binary.ReadFixedString(len);

            var str = binary.ReadBCD(6);
            if (DateTime.TryParseExact(str, "yyMMddHHmmss", null, DateTimeStyles.None, out var dt))
                Time = dt;

            Sequence = binary.ReadByte();
            Attachments = binary.ReadByte();
            Reserved = len == 7 ? binary.ReadByte() : binary.ReadUInt16();

            return true;
        }

        /// <summary>写入</summary>
        /// <param name="stream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Boolean Write(Stream stream, Object context)
        {
            var binary = new Binary { Stream = stream };

            // 苏标16字节，粤标40字节
            var len = DeviceId.Length > 7 ? 30 : 7;

            binary.WriteFixedString(DeviceId, len);
            binary.WriteBCD(Time.ToString("yyMMddHHmmss"), 6);
            binary.WriteByte(Sequence);
            binary.WriteByte(Attachments);

            if (len == 7)
                binary.WriteByte((Byte)Reserved);
            else
                binary.WriteUInt16(Reserved);

            return true;
        }
    }
}