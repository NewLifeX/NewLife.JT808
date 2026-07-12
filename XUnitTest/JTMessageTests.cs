using System.ComponentModel;
using NewLife.JT808.Protocols;
using NewLife.JT808.Models;
using NewLife.JT808.UtilTool;

namespace XUnitTest;

/// <summary>JTMessage 帧解析测试</summary>
public class JTMessageTests
{
    [Fact]
    [DisplayName("解析位置汇报帧")]
    public void Parse_PositionReport()
    {
        // 构造一个 T0200 消息体
        var t0200 = new T0200
        {
            Alarm = PositionAlarms.紧急报瞥,
            Status = PositionStatus.ACC,
            Latitude = 22334455,
            Longitude = 114112233,
            Speed = 60,
        };

        // 封装为 JTMessage
        var msg = new JTMessage
        {
            Kind = MessageKinds.位置信息汇报,
            Mobile = "013880011111",
            Sequence = 1,
        };
        msg.SetResult(t0200);

        // 序列化为字节
        var pk = msg.ToPacket();
        Assert.NotNull(pk);
        Assert.True(pk.Total > 0);

        // 反序列化回来
        var msg2 = new JTMessage();
        Assert.True(msg2.Read(pk));

        Assert.Equal(MessageKinds.位置信息汇报, msg2.Kind);
        Assert.Equal("13880011111", msg2.Mobile);
        Assert.Equal((UInt16)1, msg2.Sequence);
        Assert.NotNull(msg2.Payload);
        Assert.True(msg2.Payload.Total > 0);

        // 解析消息体
        var body = msg2.ReadModel<T0200>();
        Assert.NotNull(body);
        Assert.Equal(22334455, body.Latitude);
        Assert.Equal(114112233, body.Longitude);
        Assert.Equal(60, body.Speed);
    }

    [Fact]
    [DisplayName("MessageFactory 初始化")]
    public void MessageFactory_Init()
    {
        MessageFactory.Instance.Init();
        var maps = MessageFactory.Instance.Maps;

        // 检查关键消息类型是否注册
        Assert.True(maps.ContainsKey(MessageKinds.位置信息汇报));
        Assert.True(maps.ContainsKey(MessageKinds.终端注册));
        Assert.True(maps.ContainsKey(MessageKinds.终端鉴权));
        Assert.True(maps.ContainsKey(MessageKinds.平台通用应答));
        Assert.True(maps.ContainsKey(MessageKinds.终端心跳));
    }

    [Fact]
    [DisplayName("外部程序集注册")]
    public void MessageFactory_RegisterAssembly()
    {
        MessageFactory.Instance.Init();
        var count = MessageFactory.Instance.Maps.Count;
        Assert.True(count > 60); // 应该覆盖 60+ 消息类型
    }
}
