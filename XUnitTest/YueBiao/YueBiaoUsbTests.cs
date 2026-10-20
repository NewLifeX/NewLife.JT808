using System.ComponentModel;
using NewLife.JT808.YueBiao;

namespace XUnitTest.YueBiao;

/// <summary>粤标 0x0900 透传子类型测试</summary>
public class YueBiaoUsbTests
{
    [Fact]
    [DisplayName("T0900_F7_USB状态_往返")]
    public void F7_RoundTrip()
    {
        var source = new T0900_F7
        {
            TotalCount = 2,
            Items =
            [
                new USBStatusItem { Id = 1, Status = 0, Online = 1, Reserved = 0 },
                new USBStatusItem { Id = 2, Status = 1, Online = 0, Reserved = 0xFF },
            ],
        };

        var bytes = source.ToBytes();
        Assert.Equal(2 + 2 * 4, bytes.Length);

        var result = T0900_F7.Parse(bytes);

        Assert.Equal((Byte)2, result.TotalCount);
        Assert.NotNull(result.Items);
        Assert.Equal(2, result.Items.Length);
        Assert.Equal((Byte)1, result.Items[0].Id);
        Assert.Equal((Byte)1, result.Items[0].Online);
        Assert.Equal((Byte)0xFF, result.Items[1].Reserved);
    }

    [Fact]
    [DisplayName("T0900_F7_空列表_往返")]
    public void F7_Empty()
    {
        var bytes = new T0900_F7 { TotalCount = 0 }.ToBytes();

        var result = T0900_F7.Parse(bytes);

        Assert.Equal((Byte)0, result.TotalCount);
        Assert.NotNull(result.Items);
        Assert.Empty(result.Items);
    }

    [Fact]
    [DisplayName("T0900_F8_USB信息_往返")]
    public void F8_RoundTrip()
    {
        var source = new T0900_F8
        {
            TotalCount = 1,
            Items =
            [
                new USBInfoItem
                {
                    Id = 3,
                    Company = "SAMSUNG",
                    Model = "X1",
                    HardwareVersion = "V1.0",
                    FirmwareVersion = "F1.0",
                },
            ],
        };

        var bytes = source.ToBytes();
        Assert.Equal(2 + 1 * 61, bytes.Length);

        var result = T0900_F8.Parse(bytes);

        Assert.Equal((Byte)1, result.TotalCount);
        Assert.NotNull(result.Items);
        var item = Assert.Single(result.Items);
        Assert.Equal((Byte)3, item.Id);
        Assert.Equal("SAMSUNG", item.Company);
        Assert.Equal("X1", item.Model);
        Assert.Equal("V1.0", item.HardwareVersion);
        Assert.Equal("F1.0", item.FirmwareVersion);
    }
}
