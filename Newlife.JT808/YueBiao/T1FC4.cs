using System.Globalization;
using NewLife.JT808.Protocols;
using NewLife.JT808.SuBiao;
using NewLife.Serialization;

namespace NewLife.JT808.YueBiao;

/// <summary>粤标主动安全报警上报（0x1FC4）</summary>
/// <remarks>
/// 粤标核心报警消息，终端向平台上报 ADAS/DSM/TPMS/BSD 等主动安全报警信息。
/// 与苏标报警上报不同，粤标使用独立消息 ID 0x1FC4 而非位置附加信息。
/// </remarks>
[MessageKind(MessageKinds.粤标主动安全报警)]
public class T1FC4 : IAccessor
{
    #region 属性
    /// <summary>终端ID。粤标30字节</summary>
    [FieldSize(30)]
    public String TerminalId { get; set; } = String.Empty;

    /// <summary>报警标识号</summary>
    public AlarmDevice AlarmId { get; set; } = new();

    /// <summary>车辆状态</summary>
    public CarStatus Status { get; set; }

    /// <summary>报警类型。0x64=ADAS, 0x65=DSM, 0x66=TPMS, 0x67=BSD</summary>
    public Byte AlarmType { get; set; }

    /// <summary>报警/事件类型。根据 AlarmType 对应不同枚举</summary>
    public Byte AlarmKind { get; set; }

    /// <summary>报警级别。1=一级报警，2=二级报警</summary>
    public Byte Level { get; set; }

    /// <summary>车速。单位 km/h</summary>
    public Byte Speed { get; set; }

    /// <summary>海拔高度，单位 m</summary>
    public Int16 Altitude { get; set; }

    /// <summary>纬度。百万分之一度</summary>
    public Int32 Latitude { get; set; }

    /// <summary>经度。百万分之一度</summary>
    public Int32 Longitude { get; set; }

    /// <summary>时间。YY-MM-DD-hh-mm-ss</summary>
    [BCDTime(6, "yyMMddHHmmss")]
    public DateTime Time { get; set; }

    /// <summary>报警数据长度</summary>
    public Int16 DataLength { get; set; }

    /// <summary>报警数据。根据 AlarmType 类型不同，长度可变</summary>
    public Byte[]? Data { get; set; }
    #endregion

    #region IAccessor
    /// <summary>读取</summary>
    public Boolean Read(Stream stream, Object? context)
    {
        var reader = Helper.CreateReader(stream);

        TerminalId = reader.ReadFixedString(30);
        AlarmId = reader.Read<AlarmDevice>();
        Status = (CarStatus)reader.ReadUInt16();
        AlarmType = reader.ReadByte();
        AlarmKind = reader.ReadByte();
        Level = reader.ReadByte();
        Speed = reader.ReadByte();
        Altitude = reader.ReadInt16();
        Latitude = reader.ReadInt32();
        Longitude = reader.ReadInt32();

        var str = reader.ReadBCD(6);
        if (DateTime.TryParseExact(str, "yyMMddHHmmss", null, DateTimeStyles.None, out var dt))
            Time = dt;

        DataLength = reader.ReadInt16();

        if (DataLength > 0)
            Data = reader.ReadBytes(DataLength);

        return true;
    }

    /// <summary>写入</summary>
    public Boolean Write(Stream stream, Object? context)
    {
        var writer = Helper.CreateWriter(stream);

        writer.WriteFixedString(TerminalId, 30);
        writer.Write(AlarmId);
        writer.Write((UInt16)Status);
        writer.Write(AlarmType);
        writer.Write(AlarmKind);
        writer.Write(Level);
        writer.Write(Speed);
        writer.Write(Altitude);
        writer.Write(Latitude);
        writer.Write(Longitude);
        writer.WriteBCD(Time.ToString("yyMMddHHmmss"), 6);

        DataLength = (Int16)(Data?.Length ?? 0);
        writer.Write(DataLength);
        if (Data is { Length: > 0 })
            writer.Write(Data);

        return true;
    }
    #endregion
}
