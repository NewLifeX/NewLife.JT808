using System.ComponentModel;
using NewLife.JT808.CarDVR;

namespace XUnitTest.Protocols;

/// <summary>JT/T 19056 行车记录仪数据包单元测试</summary>
public class CarDVRTests
{
    [Fact]
    [DisplayName("上行包_未知命令字_跳过数据体并定位校验字")]
    public void UpPackage_UnknownBody_SkipsData()
    {
        // 起始字头 + 命令字 + 长度(3) + 保留 + 数据体(3) + 校验
        var bytes = new Byte[]
        {
            0x55, 0x7A,
            0x7F,
            0x00, 0x03,
            0x00,
            0x11, 0x22, 0x33,
            0x5A,
        };

        var package = new UpPackage();
        Assert.True(package.Read(new MemoryStream(bytes)));

        Assert.Equal(UpPackage.BeginFlag, package.Begin);
        Assert.Equal((Byte)0x7F, package.CommandId);
        Assert.Equal((UInt16)3, package.DataLength);
        Assert.Null(package.Bodies);
        Assert.Equal((Byte)0x5A, package.CheckCode);
    }

    [Fact]
    [DisplayName("下行包_未知命令字_跳过数据体并定位校验字")]
    public void DownPackage_UnknownBody_SkipsData()
    {
        var bytes = new Byte[]
        {
            0x55, 0x7A,
            0x01,
            0x00, 0x02,
            0x00,
            0xAB, 0xCD,
            0x33,
        };

        var package = new DownPackage();
        Assert.True(package.Read(new MemoryStream(bytes)));

        Assert.Equal((UInt16)2, package.DataLength);
        Assert.Null(package.Bodies);
        Assert.Equal((Byte)0x33, package.CheckCode);
    }

    [Fact]
    [DisplayName("数据包_起始字头错误_读取失败")]
    public void Package_WrongMagic_ReturnsFalse()
    {
        var bytes = new Byte[] { 0x00, 0x00, 0x01, 0x00, 0x00, 0x00 };
        Assert.False(new UpPackage().Read(new MemoryStream(bytes)));
        Assert.False(new DownPackage().Read(new MemoryStream(bytes)));
    }
}
