using NewLife.JT808.Protocols;
using NewLife.Serialization;

namespace NewLife.JT808.Models
{
    /// <summary>
    /// 设置多边形区域
    /// 0x8604
    /// </summary>
    [MessageKind(MessageKinds.设置多边形区域)]
    public class T8604 : IAccessor
    {
        /// <summary>区域 ID</summary>
        public UInt32 AreaId { get; set; }

        /// <summary>区域属性。bit0 是否限时；bit1 是否限速；bit6 南纬；bit7 西经</summary>
        public UInt16 AreaProperty { get; set; }

        /// <summary>起始时间。区域属性 bit0 为 1 时有效</summary>
        public DateTime? StartTime { get; set; }

        /// <summary>结束时间。区域属性 bit0 为 1 时有效</summary>
        public DateTime? EndTime { get; set; }

        /// <summary>最高速度。区域属性 bit1 为 1 时有效</summary>
        public UInt16 HighestSpeed { get; set; }

        /// <summary>超速持续时间（秒）。区域属性 bit1 为 1 时有效</summary>
        public Byte OverspeedDuration { get; set; }

        /// <summary>区域总顶点数</summary>
        public UInt16 PeakCount { get; set; }

        /// <summary>顶点项集合</summary>
        public List<AreaVertex> PeakItems { get; set; } = [];

        /// <summary>夜间最高速度。2019 版且区域属性 bit1 为 1 时有效</summary>
        public UInt16 NightMaximumSpeed { get; set; }

        /// <summary>区域名称长度。2019 版有效</summary>
        public UInt16 NameLength { get; set; }

        /// <summary>区域名称。2019 版有效</summary>
        public String? AreaName { get; set; }

        /// <summary>解析消息</summary>
        /// <param name="stream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Boolean Read(Stream stream, Object? context)
        {
            var header = context as JTMessage;
            var is2019 = header != null && header.Version > 0;
            var reader = Helper.CreateReader(stream, header);

            AreaId = reader.ReadUInt32();
            AreaProperty = reader.ReadUInt16();

            if ((AreaProperty & 0x01) != 0)
            {
                StartTime = SettingAreaCodec.ReadTime(reader);
                EndTime = SettingAreaCodec.ReadTime(reader);
            }

            if ((AreaProperty & 0x02) != 0)
            {
                HighestSpeed = reader.ReadUInt16();
                OverspeedDuration = reader.ReadByte();
            }

            PeakCount = reader.ReadUInt16();
            PeakItems = [];
            for (var i = 0; i < PeakCount; i++)
            {
                PeakItems.Add(new AreaVertex
                {
                    Latitude = reader.ReadUInt32(),
                    Longitude = reader.ReadUInt32(),
                });
            }

            if (is2019)
            {
                NightMaximumSpeed = reader.ReadUInt16();
                NameLength = reader.ReadUInt16();
                if (NameLength > 0) AreaName = Helper.Encoding.GetString(reader.ReadBytes(NameLength));
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

            writer.WriteUInt32(AreaId);
            writer.WriteUInt16(AreaProperty);

            if ((AreaProperty & 0x01) != 0)
            {
                SettingAreaCodec.WriteTime(writer, StartTime);
                SettingAreaCodec.WriteTime(writer, EndTime);
            }

            if ((AreaProperty & 0x02) != 0)
            {
                writer.WriteUInt16(HighestSpeed);
                writer.WriteByte(OverspeedDuration);
            }

            PeakCount = (UInt16)(PeakItems?.Count ?? 0);
            writer.WriteUInt16(PeakCount);
            if (PeakItems != null)
            {
                foreach (var item in PeakItems)
                {
                    writer.WriteUInt32(item.Latitude);
                    writer.WriteUInt32(item.Longitude);
                }
            }

            if (is2019)
            {
                writer.WriteUInt16(NightMaximumSpeed);

                var buf = AreaName.IsNullOrEmpty() ? new Byte[0] : Helper.Encoding.GetBytes(AreaName!);
                NameLength = (UInt16)buf.Length;
                writer.WriteUInt16(NameLength);
                if (buf.Length > 0) writer.Write(buf);
            }

            return true;
        }
    }
}
