using NewLife.JT808.Protocols;
using NewLife.JT808.UtilTool;
using NewLife.Serialization;

namespace NewLife.JT808.Models
{
    /// <summary>Can属性</summary>
    public struct CanProperty
    {
        /// <summary>CAN ID 4</summary>
        public UInt32 Id { get; set; }

        /// <summary>CAN 数据 8</summary>
        public Byte[] Data { get; set; }

        /// <summary>已重载。友好显示</summary>
        /// <returns></returns>
        public override String ToString() => $"0x{Id:X8}";
    }

    /// <summary>
    /// CAN 总线数据上传
    /// 0x0705
    /// </summary>
    [MessageKind(MessageKinds.CAN总线数据上传)]
    public class T0705 : IAccessor
    {
        /// <summary>
        /// 数据项个数
        /// 包含的 CAN 总线数据项个数，>0
        /// </summary>
        public UInt16 Count { get; set; }

        /// <summary>
        /// CAN 总线数据接收时间
        /// 第 1 条 CAN 总线数据的接收时间，hh-mm-ss-msms
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>CAN 总线数据项</summary>
        public CanProperty[] Items { get; set; }

        /// <summary></summary>
        /// <param name="stream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Boolean Read(Stream stream, Object context)
        {
            var reader = Helper.CreateReader(stream);

            Count = reader.ReadUInt16();
            if (TimeSpan.TryParseExact(reader.ReadBCD(5), "HHmmssffff", null, out var ts))
                Time = DateTime.Today.Add(ts);

            var list = new List<CanProperty>();
            for (var i = 0; i < Count; i++)
            {
                var cp = new CanProperty
                {
                    Id = reader.ReadUInt32(),
                    Data = reader.ReadBytes(8)
                };
                list.Add(cp);
            }
            Items = list.ToArray();

            return true;
        }

        /// <summary></summary>
        /// <param name="stream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Boolean Write(Stream stream, Object context)
        {
            var writer = Helper.CreateWriter(stream);

            Count = (UInt16)(Items?.Length ?? 0);
            writer.Write(Count);

            writer.WriteBCD(Time.ToString("HHmmssffff"), 5);

            if (Items != null)
            {
                foreach (var item in Items)
                {
                    writer.Write(item.Id);
                    writer.Write(item.Data, 0, item.Data.Length);
                }
            }

            return true;
        }
    }
}