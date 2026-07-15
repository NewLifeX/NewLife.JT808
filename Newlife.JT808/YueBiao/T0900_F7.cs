using NewLife.JT808.Protocols;
using NewLife.Serialization;

namespace NewLife.JT808.YueBiao;

/// <summary>USB外设状态信息上报（0x0900 子类型 0xF7）</summary>
/// <remarks>
/// 粤标终端通过 0x0900 透传消息上报 USB 外设的状态信息。
/// </remarks>
public class T0900_F7
{
    #region 属性
    /// <summary>USB总数量</summary>
    public Byte TotalCount { get; set; }

    /// <summary>USB设备列表</summary>
    public USBStatusItem[]? Items { get; set; }
    #endregion

    #region 方法
    /// <summary>从透传数据读取</summary>
    public static T0900_F7 Parse(Byte[] data)
    {
        var reader = Helper.CreateReader(new MemoryStream(data));
        var result = new T0900_F7
        {
            TotalCount = reader.ReadByte()
        };

        var count = reader.ReadByte();
        var items = new USBStatusItem[count];
        for (var i = 0; i < count; i++)
        {
            items[i] = new USBStatusItem
            {
                Id = reader.ReadByte(),
                Status = reader.ReadByte(),
                Online = reader.ReadByte(),
                Reserved = reader.ReadByte(),
            };
        }
        result.Items = items;

        return result;
    }

    /// <summary>转换为透传数据</summary>
    public Byte[] ToBytes()
    {
        var stream = new MemoryStream();
        var writer = Helper.CreateWriter(stream);

        writer.Write(TotalCount);
        var count = (Byte)(Items?.Length ?? 0);
        writer.Write(count);
        for (var i = 0; i < count; i++)
        {
            var item = Items![i];
            writer.Write(item.Id);
            writer.Write(item.Status);
            writer.Write(item.Online);
            writer.Write(item.Reserved);
        }

        return stream.ToArray();
    }
    #endregion
}

/// <summary>USB设备状态项</summary>
public class USBStatusItem
{
    /// <summary>USB接口ID</summary>
    public Byte Id { get; set; }

    /// <summary>状态。0x00=正常工作，0x01=异常</summary>
    public Byte Status { get; set; }

    /// <summary>在线状态。0x00=不在线，0x01=在线</summary>
    public Byte Online { get; set; }

    /// <summary>预留</summary>
    public Byte Reserved { get; set; }
}
