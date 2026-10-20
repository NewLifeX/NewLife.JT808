using NewLife.JT808.Protocols;
using NewLife.Serialization;

namespace NewLife.JT808.Models
{
    /// <summary>
    /// 设置路线
    /// 0x8606
    /// </summary>
    [MessageKind(MessageKinds.设置路线)]
    public class T8606 : IAccessor
    {
        /// <summary>路线 ID</summary>
        public UInt32 RouteId { get; set; }

        /// <summary>路线属性。bit0 是否限时</summary>
        public UInt16 RouteProperty { get; set; }

        /// <summary>起始时间。路线属性 bit0 为 1 时有效</summary>
        public DateTime? StartTime { get; set; }

        /// <summary>结束时间。路线属性 bit0 为 1 时有效</summary>
        public DateTime? EndTime { get; set; }

        /// <summary>路线总拐点数</summary>
        public UInt16 InflectionPointCount { get; set; }

        /// <summary>拐点项集合</summary>
        public List<RoutePoint> InflectionPointItems { get; set; } = [];

        /// <summary>路线名称长度。2019 版有效</summary>
        public UInt16 RouteNameLength { get; set; }

        /// <summary>路线名称。2019 版有效</summary>
        public String? RouteName { get; set; }

        /// <summary>解析消息</summary>
        /// <param name="stream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Boolean Read(Stream stream, Object? context)
        {
            var header = context as JTMessage;
            var is2019 = header != null && header.Version > 0;
            var reader = Helper.CreateReader(stream, header);

            RouteId = reader.ReadUInt32();
            RouteProperty = reader.ReadUInt16();

            if ((RouteProperty & 0x01) != 0)
            {
                StartTime = SettingAreaCodec.ReadTime(reader);
                EndTime = SettingAreaCodec.ReadTime(reader);
            }

            InflectionPointCount = reader.ReadUInt16();
            InflectionPointItems = [];
            for (var i = 0; i < InflectionPointCount; i++)
            {
                var item = new RoutePoint
                {
                    InflectionPointId = reader.ReadUInt32(),
                    SectionId = reader.ReadUInt32(),
                    Latitude = reader.ReadUInt32(),
                    Longitude = reader.ReadUInt32(),
                    SectionWidth = reader.ReadByte(),
                    SectionProperty = reader.ReadByte(),
                };

                if ((item.SectionProperty & 0x01) != 0)
                {
                    item.LongDrivingThreshold = reader.ReadUInt16();
                    item.ShortDrivingThreshold = reader.ReadUInt16();
                }

                if ((item.SectionProperty & 0x02) != 0)
                {
                    item.HighestSpeed = reader.ReadUInt16();
                    item.OverspeedDuration = reader.ReadByte();
                    if (is2019) item.NightMaximumSpeed = reader.ReadUInt16();
                }

                InflectionPointItems.Add(item);
            }

            if (is2019)
            {
                RouteNameLength = reader.ReadUInt16();
                if (RouteNameLength > 0) RouteName = Helper.Encoding.GetString(reader.ReadBytes(RouteNameLength));
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

            writer.WriteUInt32(RouteId);
            writer.WriteUInt16(RouteProperty);

            if ((RouteProperty & 0x01) != 0)
            {
                SettingAreaCodec.WriteTime(writer, StartTime);
                SettingAreaCodec.WriteTime(writer, EndTime);
            }

            InflectionPointCount = (UInt16)(InflectionPointItems?.Count ?? 0);
            writer.WriteUInt16(InflectionPointCount);

            if (InflectionPointItems != null)
            {
                foreach (var item in InflectionPointItems)
                {
                    writer.WriteUInt32(item.InflectionPointId);
                    writer.WriteUInt32(item.SectionId);
                    writer.WriteUInt32(item.Latitude);
                    writer.WriteUInt32(item.Longitude);
                    writer.WriteByte(item.SectionWidth);
                    writer.WriteByte(item.SectionProperty);

                    if ((item.SectionProperty & 0x01) != 0)
                    {
                        writer.WriteUInt16(item.LongDrivingThreshold ?? 0);
                        writer.WriteUInt16(item.ShortDrivingThreshold ?? 0);
                    }

                    if ((item.SectionProperty & 0x02) != 0)
                    {
                        writer.WriteUInt16(item.HighestSpeed ?? 0);
                        writer.WriteByte(item.OverspeedDuration ?? 0);
                        if (is2019) writer.WriteUInt16(item.NightMaximumSpeed ?? 0);
                    }
                }
            }

            if (is2019)
            {
                var buf = RouteName.IsNullOrEmpty() ? new Byte[0] : Helper.Encoding.GetBytes(RouteName!);
                RouteNameLength = (UInt16)buf.Length;
                writer.WriteUInt16(RouteNameLength);
                if (buf.Length > 0) writer.Write(buf);
            }

            return true;
        }
    }

    /// <summary>路线拐点项</summary>
    public class RoutePoint
    {
        /// <summary>拐点ID</summary>
        public UInt32 InflectionPointId { get; set; }

        /// <summary>路段ID</summary>
        public UInt32 SectionId { get; set; }

        /// <summary>拐点纬度</summary>
        public UInt32 Latitude { get; set; }

        /// <summary>拐点经度</summary>
        public UInt32 Longitude { get; set; }

        /// <summary>路段宽度（米）</summary>
        public Byte SectionWidth { get; set; }

        /// <summary>路段属性。bit0 是否限时；bit1 是否限速；bit2 南纬；bit3 西经</summary>
        public Byte SectionProperty { get; set; }

        /// <summary>路段行驶过长阈值（秒）。路段属性 bit0 为 1 时有效</summary>
        public UInt16? LongDrivingThreshold { get; set; }

        /// <summary>路段行驶过短阈值（秒）。路段属性 bit0 为 1 时有效</summary>
        public UInt16? ShortDrivingThreshold { get; set; }

        /// <summary>路段最高速度。路段属性 bit1 为 1 时有效</summary>
        public UInt16? HighestSpeed { get; set; }

        /// <summary>超速持续时间（秒）。路段属性 bit1 为 1 时有效</summary>
        public Byte? OverspeedDuration { get; set; }

        /// <summary>夜间最高速度。2019 版且路段属性 bit1 为 1 时有效</summary>
        public UInt16? NightMaximumSpeed { get; set; }
    }
}
