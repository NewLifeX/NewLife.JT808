using NewLife.JT808.Protocols;
using NewLife.Serialization;

namespace NewLife.JT808.Models
{
    /// <summary>
    /// 存储多媒体数据检索应答
    /// 0x0802
    /// </summary>
    [MessageKind(MessageKinds.存储多媒体数据检索应答)]
    public class T0802 : IAccessor
    {
        /// <summary>位置数据固定长度（不含附加信息）</summary>
        public const Int32 PositionLength = 28;

        /// <summary>
        /// 应答流水号
        /// 对应的多媒体数据检索消息的流水号
        /// </summary>
        public UInt16 MsgNum { get; set; }

        /// <summary>
        /// 多媒体数据总项数
        /// 满足检索条件的多媒体数据总项数
        /// </summary>
        public UInt16 MultimediaItemCount { get; set; }

        /// <summary>
        /// 检索项集合
        /// </summary>
        public List<MediaSearchItem> MediaProperties { get; set; } = [];

        /// <summary>解析消息</summary>
        /// <param name="stream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Boolean Read(Stream stream, Object? context)
        {
            var reader = Helper.CreateReader(stream);

            MsgNum = reader.ReadUInt16();
            MultimediaItemCount = reader.ReadUInt16();

            MediaProperties = [];
            for (var i = 0; i < MultimediaItemCount; i++)
            {
                var item = new MediaSearchItem
                {
                    MediaId = reader.ReadUInt32(),
                    MediaType = (MediaTypes)reader.ReadByte(),
                    ChannelId = reader.ReadByte(),
                    EventItemCoding = reader.ReadByte(),
                };

                var buf = reader.ReadBytes(PositionLength);
                if (buf != null && buf.Length == PositionLength)
                {
                    var position = new T0200();
                    position.Read(new MemoryStream(buf), null);
                    item.Position = position;
                }

                MediaProperties.Add(item);
            }

            return true;
        }

        /// <summary>序列化消息</summary>
        /// <param name="stream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Boolean Write(Stream stream, Object? context)
        {
            var writer = Helper.CreateWriter(stream);

            writer.WriteUInt16(MsgNum);
            MultimediaItemCount = (UInt16)(MediaProperties?.Count ?? 0);
            writer.WriteUInt16(MultimediaItemCount);

            if (MediaProperties != null)
            {
                foreach (var item in MediaProperties)
                {
                    writer.WriteUInt32(item.MediaId);
                    writer.WriteByte((Byte)item.MediaType);
                    writer.WriteByte(item.ChannelId);
                    writer.WriteByte(item.EventItemCoding);

                    var buf = new Byte[0];
                    if (item.Position != null)
                    {
                        using var ms = new MemoryStream();
                        item.Position.Write(ms, null);
                        buf = ms.ToArray();
                    }

                    var count = Math.Min(PositionLength, buf.Length);
                    if (count > 0) writer.Write(buf, 0, count);
                    if (count < PositionLength) writer.Write(new Byte[PositionLength - count], 0, PositionLength - count);
                }
            }

            return true;
        }
    }
}
