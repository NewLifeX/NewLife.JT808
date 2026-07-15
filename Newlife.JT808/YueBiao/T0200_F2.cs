using NewLife.JT808.Protocols;
using NewLife.Serialization;

namespace NewLife.JT808.YueBiao;

/// <summary>位置附加信息-算法异常（0xF2）</summary>
/// <remarks>
/// 粤标位置附加信息，标识终端主动安全算法异常状态。
/// </remarks>
public class T0200_F2
{
    #region 属性
    /// <summary>算法异常状态。位标志</summary>
    public AlgorithmAbnormalFlags Flags { get; set; }
    #endregion

    #region 方法
    /// <summary>从字节数据读取</summary>
    public static T0200_F2 Parse(Byte[] data)
    {
        var reader = Helper.CreateReader(new MemoryStream(data));
        return new T0200_F2
        {
            Flags = (AlgorithmAbnormalFlags)reader.ReadUInt32()
        };
    }

    /// <summary>转换为字节数据</summary>
    public Byte[] ToBytes()
    {
        var stream = new MemoryStream();
        var writer = Helper.CreateWriter(stream);
        writer.Write((UInt32)Flags);
        return stream.ToArray();
    }
    #endregion
}

/// <summary>算法异常标志位</summary>
[Flags]
public enum AlgorithmAbnormalFlags : UInt32
{
    /// <summary>无异常</summary>
    无异常 = 0,

    /// <summary>ADAS算法异常</summary>
    ADAS算法异常 = 0x01,

    /// <summary>DSM算法异常</summary>
    DSM算法异常 = 0x02,

    /// <summary>TPMS算法异常</summary>
    TPMS算法异常 = 0x04,

    /// <summary>BSD算法异常</summary>
    BSD算法异常 = 0x08,
}
