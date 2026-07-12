using NewLife.Data;
using NewLife.JT808.Protocols;
using NewLife.JT808.Models;
using NewLife.JT808.UtilTool;
using NewLife.Serialization;

namespace NewLife.JT808.Models
{
    /// <summary>定位数据批量上传</summary>
    [MessageKind(MessageKinds.定位数据批量上传)]
    public class T0704 : IAccessor
    {
        /// <summary>数据项个数</summary>
        public UInt16 Count { get; set; }

        /// <summary>位置数据类型。0：正常位置批量汇报，1：盲区补报</summary>
        public Byte Type { get; set; }

        /// <summary>位置汇报数据集合</summary>
        public List<T0200> Positions { get; set; }

        /// <summary>解析数据</summary>
        /// <param name="stream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Boolean Read(Stream stream, Object context)
        {
            var reader = context as Binary ?? Helper.CreateReader(stream);

            Count = reader.ReadUInt16();
            Type = reader.ReadByte();
            Positions = new List<T0200>();
            for (var i = 0; i < Count; i++)
            {
                var len = reader.ReadUInt16();
                var buf = reader.ReadBytes(len);
                var ms = new MemoryStream(buf);

                var msg = new T0200();
                msg.Read(ms, null);
                Positions.Add(msg);
            }

            return true;
        }

        /// <summary>序列化消息</summary>
        /// <param name="stream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Boolean Write(Stream stream, Object context)
        {
            var writer = context as Binary ?? Helper.CreateWriter(stream);

            Count = (UInt16)(Positions?.Count ?? 0);
            writer.Write(Count);
            writer.Write(Type);

            if (Count > 0)
            {
                foreach (var item in Positions)
                {
                    var pk = item.ToPacket();
                    writer.Write((UInt16)pk.Total);
                    writer.Write(((Packet)pk).ToArray());
                }
            }

            return true;
        }
    }
}