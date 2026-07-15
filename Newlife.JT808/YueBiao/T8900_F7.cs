using NewLife.JT808.Protocols;
using NewLife.Serialization;

namespace NewLife.JT808.YueBiao;

/// <summary>USB外设状态查询（0x8900 子类型 0xF7）</summary>
/// <remarks>
/// 平台通过 0x8900 透传消息查询终端 USB 外设状态。
/// </remarks>
public class T8900_F7
{
    #region 属性
    /// <summary>USB接口ID。0xFF=查询所有</summary>
    public Byte UsbId { get; set; }
    #endregion

    #region 方法
    /// <summary>序列化</summary>
    public Byte[] ToBytes()
    {
        var stream = new MemoryStream();
        var writer = Helper.CreateWriter(stream);
        writer.Write(UsbId);
        return stream.ToArray();
    }

    /// <summary>反序列化</summary>
    public static T8900_F7 Parse(Byte[] data)
    {
        var reader = Helper.CreateReader(new MemoryStream(data));
        return new T8900_F7 { UsbId = reader.ReadByte() };
    }
    #endregion
}
