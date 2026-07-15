using NewLife.JT808.Protocols;
using NewLife.Serialization;

namespace NewLife.JT808.YueBiao;

/// <summary>USB外设信息上报（0x0900 子类型 0xF8）</summary>
/// <remarks>
/// 粤标终端通过 0x0900 透传消息上报 USB 外设的详细信息（厂商/型号等）。
/// </remarks>
public class T0900_F8
{
    #region 属性
    /// <summary>USB总数量</summary>
    public Byte TotalCount { get; set; }

    /// <summary>USB设备列表</summary>
    public USBInfoItem[]? Items { get; set; }
    #endregion

    #region 方法
    /// <summary>从透传数据读取</summary>
    public static T0900_F8 Parse(Byte[] data)
    {
        var reader = Helper.CreateReader(new MemoryStream(data));
        var result = new T0900_F8
        {
            TotalCount = reader.ReadByte()
        };

        var count = reader.ReadByte();
        var items = new USBInfoItem[count];
        for (var i = 0; i < count; i++)
        {
            items[i] = new USBInfoItem
            {
                Id = reader.ReadByte(),
                Company = reader.ReadString(20),
                Model = reader.ReadString(20),
                HardwareVersion = reader.ReadString(10),
                FirmwareVersion = reader.ReadString(10),
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
            writer.WriteFixedString(item.Company, 20);
            writer.WriteFixedString(item.Model, 20);
            writer.WriteFixedString(item.HardwareVersion, 10);
            writer.WriteFixedString(item.FirmwareVersion, 10);
        }

        return stream.ToArray();
    }
    #endregion
}

/// <summary>USB设备信息项</summary>
public class USBInfoItem
{
    /// <summary>USB接口ID</summary>
    public Byte Id { get; set; }

    /// <summary>制造商</summary>
    public String Company { get; set; } = String.Empty;

    /// <summary>型号</summary>
    public String Model { get; set; } = String.Empty;

    /// <summary>硬件版本</summary>
    public String HardwareVersion { get; set; } = String.Empty;

    /// <summary>固件版本</summary>
    public String FirmwareVersion { get; set; } = String.Empty;
}
