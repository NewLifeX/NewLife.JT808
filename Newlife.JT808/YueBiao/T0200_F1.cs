using NewLife.JT808.Protocols;
using NewLife.Serialization;

namespace NewLife.JT808.YueBiao;

/// <summary>位置附加信息-安装异常（0xF1）</summary>
/// <remarks>
/// 粤标位置附加信息，标识终端外设安装异常状态。
/// </remarks>
public class T0200_F1
{
    #region 属性
    /// <summary>安装异常状态。位标志</summary>
    public InstallAbnormalFlags Flags { get; set; }
    #endregion

    #region 方法
    /// <summary>从字节数据读取</summary>
    public static T0200_F1 Parse(Byte[] data)
    {
        var reader = Helper.CreateReader(new MemoryStream(data));
        return new T0200_F1
        {
            Flags = (InstallAbnormalFlags)reader.ReadUInt32()
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

/// <summary>安装异常标志位</summary>
[Flags]
public enum InstallAbnormalFlags : UInt32
{
    /// <summary>无异常</summary>
    无异常 = 0,

    /// <summary>摄像头异常</summary>
    摄像头异常 = 0x01,

    /// <summary>主存储器异常</summary>
    主存储器异常 = 0x02,

    /// <summary>辅存储器异常</summary>
    辅存储器异常 = 0x04,

    /// <summary>红外补光异常</summary>
    红外补光异常 = 0x08,

    /// <summary>扬声器异常</summary>
    扬声器异常 = 0x10,

    /// <summary>电池异常</summary>
    电池异常 = 0x20,

    /// <summary>通信模块异常</summary>
    通信模块异常 = 0x40,

    /// <summary>定位模块异常</summary>
    定位模块异常 = 0x80,
}
