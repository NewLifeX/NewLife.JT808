using NewLife.JT808.Protocols;
using NewLife.Serialization;

namespace NewLife.JT808.Models
{
    /// <summary>
    /// 终端补传分包请求
    /// 2019版本
    /// </summary>
    [MessageKind(MessageKinds.终端补传分包请求)]
    public class T0005 : IAccessor
    {
        /// <summary>
        /// 原始消息流水号
        /// 对应要求补传的原始消息第一包的消息流水号
        /// </summary>
        public ushort Sequence { get; set; }

        /// <summary>
        /// 重传包总数 n
        /// </summary>
        public ushort Count { get; set; }

        /// <summary>
        /// 重传包 ID 列表
        /// BYTE[2*n]
        /// 重传包序号顺序排列，如“包 ID1 包 ID2......包 IDn”。
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>读取</summary>
        /// <param name="stream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool Read(Stream stream, object context)
        {
            var header = context as JTMessage;
            var reader = new BinaryReader(stream);

            Sequence = reader.ReadUInt16();

            if (header != null && header.Version > 0)
                Count = reader.ReadUInt16();
            else
                Count = reader.ReadByte();

            Data = reader.ReadBytes(Count * 2);

            return true;
        }

        /// <summary>写入</summary>
        /// <param name="stream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool Write(Stream stream, object context)
        {
            var header = context as JTMessage;
            var writer = new BinaryWriter(stream);

            Count = (ushort)(Data != null ? Data.Length / 2 : 0);

            writer.Write(Sequence);

            if (header != null && header.Version > 0)
                writer.Write(Count);
            else
                writer.Write((byte)Count);

            writer.Write(Data);

            return true;
        }
    }
}