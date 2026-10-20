using NewLife.JT808.Protocols;
using NewLife.Serialization;

namespace NewLife.JT808.Models
{
    /// <summary>
    /// 多媒体数据上传
    /// 0x0801
    /// </summary>
    [MessageKind(MessageKinds.多媒体数据上传)]
    public class T0801 : IAccessor
    {
        /// <summary>多媒体 ID</summary>
        public UInt32 MultimediaId { get; set; }

        /// <summary>多媒体类型</summary>
        public Byte MultimediaType { get; set; }

        /// <summary>
        /// 多媒体格式编码 
        /// 0：JPEG；1：TIF；2：MP3；3：WAV；4：WMV；其他保留
        /// </summary>
        public Byte MultimediaCodingFormat { get; set; }

        /// <summary>事件项编码</summary>
        public Byte EventItemCoding { get; set; }

        /// <summary>通道 ID</summary>
        public Byte ChannelId { get; set; }

        /// <summary>定位错误</summary>
        /// <remarks>保留属性，不参与协议编解码</remarks>
        public Boolean PositionError { get; set; }

        /// <summary>
        /// 位置信息汇报(0x0200)消息体
        /// 表示拍摄或录制的起始时刻的位置基本信息数据
        /// </summary>
        public T0200 Position { get; set; }

        /// <summary>多媒体数据包</summary>
        public Byte[] MultimediaDataPackage { get; set; }

        /// <summary>位置信息固定长度。0x0801 内嵌的位置信息不含附加项</summary>
        private const Int32 PositionLength = 28;

        /// <summary>读取</summary>
        /// <param name="stream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Boolean Read(Stream stream, Object? context)
        {
            var reader = Helper.CreateReader(stream, context as JTMessage);

            MultimediaId = reader.ReadUInt32();
            MultimediaType = reader.ReadByte();
            MultimediaCodingFormat = reader.ReadByte();
            EventItemCoding = reader.ReadByte();
            ChannelId = reader.ReadByte();

            // 位置信息固定 28 字节，不足视为非法消息
            if (stream.Length - stream.Position < PositionLength) return false;

            Position = new T0200();
            Position.Read(new MemoryStream(reader.ReadBytes(PositionLength)), context);

            // 剩余字节全部为多媒体数据包
            var remain = stream.Length - stream.Position;
            if (remain > 0) MultimediaDataPackage = reader.ReadBytes((Int32)remain);

            return true;
        }

        /// <summary>写入</summary>
        /// <param name="stream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Boolean Write(Stream stream, Object? context)
        {
            var writer = Helper.CreateWriter(stream, context as JTMessage);

            writer.Write(MultimediaId);
            writer.Write(MultimediaType);
            writer.Write(MultimediaCodingFormat);
            writer.Write(EventItemCoding);
            writer.Write(ChannelId);

            // 内嵌位置信息固定 28 字节，不含附加项
            var position = Position ?? new T0200();
            var additionals = position.Additionals;
            position.Additionals = null;
            using (var ms = new MemoryStream())
            {
                position.Write(ms, context);
                var buf = ms.ToArray();
                var count = Math.Min(PositionLength, buf.Length);
                writer.Write(buf, 0, count);
                if (count < PositionLength) writer.Write(new Byte[PositionLength - count], 0, PositionLength - count);
            }
            position.Additionals = additionals;

            if (MultimediaDataPackage is { Length: > 0 }) writer.Write(MultimediaDataPackage);

            return true;
        }
    }
}
