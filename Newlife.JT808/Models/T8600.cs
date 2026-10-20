using NewLife.JT808.Protocols;
using NewLife.Serialization;

namespace NewLife.JT808.Models
{
    /// <summary>
    /// 设置圆形区域
    /// 0x8600
    /// 注：本条消息协议支持周期时间范围，如要限制每天的8:30-18:00，起始/结束时间设为：00-00-00-08-30-00/00-00-00-18-00-00，其他以此类推
    /// </summary>
    [MessageKind(MessageKinds.设置圆形区域)]
    public class T8600 : IAccessor
    {
        /// <summary>设置属性。0 更新区域，1 追加区域，2 修改区域</summary>
        public Byte SettingAreaProperty { get; set; }

        /// <summary>区域项集合</summary>
        public List<CircleArea> Areas { get; set; } = [];

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
                var area = new CircleArea();
                SettingAreaCodec.ReadHeader(reader, area);
                area.Latitude = reader.ReadUInt32();
                area.Longitude = reader.ReadUInt32();
                area.Radius = reader.ReadUInt32();
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
                    writer.WriteUInt32(area.Latitude);
                    writer.WriteUInt32(area.Longitude);
                    writer.WriteUInt32(area.Radius);
                    SettingAreaCodec.WriteTail(writer, area, is2019);
                }
            }

            return true;
        }
    }
}
