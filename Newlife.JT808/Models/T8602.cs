using NewLife.JT808.Protocols;
using NewLife.Serialization;

namespace NewLife.JT808.Models
{
    /// <summary>
    /// 设置矩形区域
    /// 0x8602
    /// 注：本条消息协议支持周期时间范围，如要限制每天的8:30-18:00，起始/结束时间设为：00-00-00-08-30-00/00-00-00-18-00-00，其他以此类推
    /// </summary>
    [MessageKind(MessageKinds.设置矩形区域)]
    public class T8602 : IAccessor
    {
        /// <summary>设置属性。0 更新区域，1 追加区域，2 修改区域</summary>
        public Byte SettingAreaProperty { get; set; }

        /// <summary>区域项集合</summary>
        public List<RectangleArea> Areas { get; set; } = [];

        /// <summary>解析消息</summary>
        /// <param name="stream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Boolean Read(Stream stream, Object? context)
        {
            var header = context as JTMessage;
            var is2019 = header != null && header.Version > 0;
            var reader = Helper.CreateReader(stream, header);

            SettingAreaProperty = reader.ReadByte();
            var count = reader.ReadByte();

            Areas = [];
            for (var i = 0; i < count; i++)
            {
                var area = new RectangleArea();
                SettingAreaCodec.ReadHeader(reader, area);
                area.LeftTopLatitude = reader.ReadUInt32();
                area.LeftTopLongitude = reader.ReadUInt32();
                area.RightBottomLatitude = reader.ReadUInt32();
                area.RightBottomLongitude = reader.ReadUInt32();
                SettingAreaCodec.ReadTail(reader, area, is2019);
                Areas.Add(area);
            }

            return true;
        }

        /// <summary>序列化消息</summary>
        /// <param name="stream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Boolean Write(Stream stream, Object? context)
        {
            var header = context as JTMessage;
            var is2019 = header != null && header.Version > 0;
            var writer = Helper.CreateWriter(stream, header);

            writer.WriteByte(SettingAreaProperty);

            var count = (Byte)(Areas?.Count ?? 0);
            writer.WriteByte(count);

            if (Areas != null)
            {
                foreach (var area in Areas)
                {
                    SettingAreaCodec.WriteHeader(writer, area);
                    writer.WriteUInt32(area.LeftTopLatitude);
                    writer.WriteUInt32(area.LeftTopLongitude);
                    writer.WriteUInt32(area.RightBottomLatitude);
                    writer.WriteUInt32(area.RightBottomLongitude);
                    SettingAreaCodec.WriteTail(writer, area, is2019);
                }
            }

            return true;
        }
    }
}
