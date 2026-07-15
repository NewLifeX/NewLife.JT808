using System.ComponentModel;
using NewLife.JT808.SuBiao;
using NewLife.JT808.Protocols;

namespace XUnitTest.Protocols;

/// <summary>苏标扩展协议单元测试</summary>
public class SuBiaoTests
{
    [Fact]
    [DisplayName("ADASInfo_构造和基本属性")]
    public void ADASInfo_Constructs()
    {
        var info = new ADASInfo();
        Assert.NotNull(info);

        info.Id = 1;
        info.Speed = 60;
        info.Latitude = 22334455;
        info.Longitude = 114112233;

        Assert.Equal(1, info.Id);
        Assert.Equal(60, info.Speed);
        Assert.Equal(22334455, info.Latitude);
        Assert.Equal(114112233, info.Longitude);
    }

    [Fact]
    [DisplayName("DSMInfo_构造和基本属性")]
    public void DSMInfo_Constructs()
    {
        var info = new DSMInfo();
        Assert.NotNull(info);

        info.Id = 2;
        info.Speed = 50;
        info.Latitude = 22334455;
        info.Longitude = 114112233;

        Assert.Equal(2, info.Id);
        Assert.Equal(50, info.Speed);
    }

    [Fact]
    [DisplayName("TPMSInfo_构造")]
    public void TPMSInfo_Constructs()
    {
        var info = new TPMSInfo();
        Assert.NotNull(info);

        info.Id = 3;
        Assert.Equal(3, info.Id);
    }

    [Fact]
    [DisplayName("BSDInfo_构造")]
    public void BSDInfo_Constructs()
    {
        var info = new BSDInfo();
        Assert.NotNull(info);

        info.Id = 4;
        info.Speed = 30;
        info.Latitude = 22334455;
        info.Longitude = 114112233;

        Assert.Equal(4, info.Id);
        Assert.Equal(30, info.Speed);
    }



    [Fact]
    [DisplayName("T1210_报警附件信息_构造")]
    public void T1210_Constructs()
    {
        var msg = new T1210
        {
            TerminalId = "TEST001",
            PlatformId = "PLAT001",
        };

        Assert.Equal("TEST001", msg.TerminalId);
        Assert.Equal("PLAT001", msg.PlatformId);
    }

    [Fact]
    [DisplayName("T1211_文件信息上传_构造")]
    public void T1211_Constructs()
    {
        var msg = new T1211
        {
            FileName = "alarm.jpg",
        };

        Assert.Equal("alarm.jpg", msg.FileName);
    }

    [Fact]
    [DisplayName("T9208_报警附件上传_构造")]
    public void T9208_Constructs()
    {
        var msg = new T9208
        {
            Server = "192.168.1.100",
            TcpPort = 9201,
            UdpPort = 9202,
        };

        Assert.Equal("192.168.1.100", msg.Server);
        Assert.Equal(9201, msg.TcpPort);
        Assert.Equal(9202, msg.UdpPort);
    }
}
