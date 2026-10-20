using System.Globalization;
using NewLife.JT808.Protocols;
using NewLife.Serialization;

namespace NewLife.JT808.Models
{
    /// <summary>区域信息基类</summary>
    /// <remarks>0x8600/0x8602/0x8604 共用的区域头与条件字段。</remarks>
    public abstract class SettingArea
    {
        /// <summary>区域ID</summary>
        public UInt32 AreaId { get; set; }

        /// <summary>
        /// 区域属性
        /// bit0 是否限时；bit1 是否限速；bit6 南纬；bit7 西经
        /// </summary>
        public UInt16 AreaProperty { get; set; }

        /// <summary>起始时间。区域属性 bit0 为 1 时有效</summary>
        public DateTime? StartTime { get; set; }

        /// <summary>结束时间。区域属性 bit0 为 1 时有效</summary>
        public DateTime? EndTime { get; set; }

        /// <summary>最高速度。区域属性 bit1 为 1 时有效</summary>
        public UInt16 MaxSpeed { get; set; }

        /// <summary>超速持续时间（秒）。区域属性 bit1 为 1 时有效</summary>
        public Byte OverspeedDuration { get; set; }

        /// <summary>夜间最高速度。2019 版且区域属性 bit1 为 1 时有效</summary>
        public UInt16 NightMaxSpeed { get; set; }

        /// <summary>区域名称。2019 版有效</summary>
        public String? AreaName { get; set; }
    }

    /// <summary>圆形区域（0x8600）</summary>
    public class CircleArea : SettingArea
    {
        /// <summary>中心点纬度，单位百万分之一度</summary>
        public UInt32 Latitude { get; set; }

        /// <summary>中心点经度，单位百万分之一度</summary>
        public UInt32 Longitude { get; set; }

        /// <summary>半径（米）</summary>
        public UInt32 Radius { get; set; }
    }

    /// <summary>矩形区域（0x8602）</summary>
    public class RectangleArea : SettingArea
    {
        /// <summary>左上点纬度</summary>
        public UInt32 LeftTopLatitude { get; set; }

        /// <summary>左上点经度</summary>
        public UInt32 LeftTopLongitude { get; set; }

        /// <summary>右下点纬度</summary>
        public UInt32 RightBottomLatitude { get; set; }

        /// <summary>右下点经度</summary>
        public UInt32 RightBottomLongitude { get; set; }
    }

    /// <summary>多边形区域（0x8604）</summary>
    public class PolygonArea : SettingArea
    {
        /// <summary>顶点集合</summary>
        public List<AreaVertex> Vertices { get; set; } = [];
    }

    /// <summary>顶点坐标</summary>
    public class AreaVertex
    {
        /// <summary>纬度，单位百万分之一度</summary>
        public UInt32 Latitude { get; set; }

        /// <summary>经度，单位百万分之一度</summary>
        public UInt32 Longitude { get; set; }
    }

    /// <summary>区域消息公共编解码</summary>
    internal static class SettingAreaCodec
    {
        public static void ReadHeader(Binary reader, SettingArea area)
        {
            area.AreaId = reader.ReadUInt32();
            area.AreaProperty = reader.ReadUInt16();
        }

        public static void ReadTail(Binary reader, SettingArea area, Boolean is2019)
        {
            if ((area.AreaProperty & 0x01) != 0)
            {
                area.StartTime = ReadTime(reader);
                area.EndTime = ReadTime(reader);
            }

            if ((area.AreaProperty & 0x02) != 0)
            {
                area.MaxSpeed = reader.ReadUInt16();
                area.OverspeedDuration = reader.ReadByte();
                if (is2019) area.NightMaxSpeed = reader.ReadUInt16();
            }

            if (is2019)
            {
                var len = reader.ReadUInt16();
                if (len > 0) area.AreaName = Helper.Encoding.GetString(reader.ReadBytes(len));
            }
        }

        public static void WriteHeader(Binary writer, SettingArea area)
        {
            writer.WriteUInt32(area.AreaId);
            writer.WriteUInt16(area.AreaProperty);
        }

        public static void WriteTail(Binary writer, SettingArea area, Boolean is2019)
        {
            if ((area.AreaProperty & 0x01) != 0)
            {
                WriteTime(writer, area.StartTime);
                WriteTime(writer, area.EndTime);
            }

            if ((area.AreaProperty & 0x02) != 0)
            {
                writer.WriteUInt16(area.MaxSpeed);
                writer.WriteByte(area.OverspeedDuration);
                if (is2019) writer.WriteUInt16(area.NightMaxSpeed);
            }

            if (is2019)
            {
                var buf = area.AreaName.IsNullOrEmpty() ? new Byte[0] : Helper.Encoding.GetBytes(area.AreaName!);
                writer.WriteUInt16((UInt16)buf.Length);
                if (buf.Length > 0) writer.Write(buf);
            }
        }

        public static DateTime? ReadTime(Binary reader)
        {
            var str = reader.ReadBCD(6);
            if (DateTime.TryParseExact(str, "yyMMddHHmmss", null, DateTimeStyles.None, out var dt)) return dt;

            return null;
        }

        public static void WriteTime(Binary writer, DateTime? time)
        {
            if (time == null)
                writer.Write(new Byte[6]);
            else
                writer.WriteBCD(time.Value.ToString("yyMMddHHmmss"), 6);
        }
    }
}
