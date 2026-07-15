using System.ComponentModel;
using NewLife.JT808.JT1078;
using NewLife.JT808.Protocols;

namespace XUnitTest.Protocols;

/// <summary>JT/T 1078 音视频扩展协议单元测试</summary>
public class JT1078Tests
{
    [Fact]
    [DisplayName("T1003_音视频属性_构造")]
    public void T1003_Constructs()
    {
        var msg = new T1003();
        Assert.NotNull(msg);

        msg.AudioChannels = 1;
        msg.MaxAudioChannels = 1;
        msg.MaxVideoChannels = 4;

        Assert.Equal(1, msg.AudioChannels);
        Assert.Equal(4, msg.MaxVideoChannels);
    }

    [Fact]
    [DisplayName("T9101_实时音视频请求_构造")]
    public void T9101_Constructs()
    {
        var msg = new T9101
        {
            Server = "192.168.1.100",
            TcpPort = 9101,
            UdpPort = 9102,
            Channel = 1,
        };

        Assert.Equal("192.168.1.100", msg.Server);
        Assert.Equal(9101, msg.TcpPort);
        Assert.Equal(1, msg.Channel);
    }

    [Fact]
    [DisplayName("T9102_音视频控制_构造")]
    public void T9102_Constructs()
    {
        var msg = new T9102
        {
            Channel = 1,
        };

        Assert.Equal(1, msg.Channel);
    }

    [Fact]
    [DisplayName("T9201_远程录像回放_构造")]
    public void T9201_Constructs()
    {
        var msg = new T9201
        {
            Server = "192.168.1.100",
            TcpPort = 9201,
            UdpPort = 9202,
            Channel = 1,
        };

        Assert.Equal("192.168.1.100", msg.Server);
        Assert.Equal(9201, msg.TcpPort);
        Assert.Equal(1, msg.Channel);
    }
}
