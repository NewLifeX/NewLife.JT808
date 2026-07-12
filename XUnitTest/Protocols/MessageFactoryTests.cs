using System.ComponentModel;
using NewLife.JT808.Protocols;
using NewLife.JT808.Models;

namespace XUnitTest.Protocols;

/// <summary>MessageFactory 消息工厂测试</summary>
public class MessageFactoryTests
{
    public MessageFactoryTests()
    {
        MessageFactory.Instance.Init();
    }

    [Fact]
    [DisplayName("初始化_注册消息类型数量")]
    public void Init_RegistersAllMessageTypes()
    {
        var maps = MessageFactory.Instance.Maps;
        Assert.True(maps.Count > 60, $"注册的消息类型应大于60，实际{maps.Count}");

        var expected = new[]
        {
            MessageKinds.终端通用应答,
            MessageKinds.终端心跳,
            MessageKinds.终端注册,
            MessageKinds.终端鉴权,
            MessageKinds.位置信息汇报,
            MessageKinds.平台通用应答,
            MessageKinds.设置终端参数,
            MessageKinds.文本信息下发,
            MessageKinds.数据上行透传,
            MessageKinds.终端RSA公钥,
        };

        foreach (var kind in expected)
            Assert.True(maps.ContainsKey(kind), $"缺少消息类型 {kind}");
    }

    [Fact]
    [DisplayName("创建消息体_Create")]
    public void Create_KnownType_ReturnsInstance()
    {
        var obj = MessageFactory.Instance.Create(MessageKinds.位置信息汇报);
        Assert.NotNull(obj);
        Assert.IsType<T0200>(obj);

        obj = MessageFactory.Instance.Create(MessageKinds.平台通用应答);
        Assert.NotNull(obj);
        Assert.IsType<T8001>(obj);

        obj = MessageFactory.Instance.Create(MessageKinds.终端注册);
        Assert.NotNull(obj);
        Assert.IsType<T0100>(obj);
    }

    [Fact]
    [DisplayName("创建消息体_未知类型_返回null")]
    public void Create_UnknownType_ReturnsNull()
    {
        var obj = MessageFactory.Instance.Create((MessageKinds)0xFFFF);
        Assert.Null(obj);
    }

    [Fact]
    [DisplayName("解析_Parse_IAccessor")]
    public void Parse_IAccessorMessage_ReturnsBody()
    {
        var msg = new JTMessage
        {
            Kind = MessageKinds.位置信息汇报,
            Mobile = "13880011111",
            Sequence = 1,
        };

        var body = new T0200
        {
            Latitude = 22334455,
            Longitude = 114112233,
            Speed = 60,
        };
        msg.SetResult(body);

        var parsed = MessageFactory.Instance.Parse(msg);
        Assert.NotNull(parsed);
        Assert.IsType<T0200>(parsed);

        var t0200 = (T0200)parsed;
        Assert.Equal(22334455, t0200.Latitude);
    }

    [Fact]
    [DisplayName("解析_Parse_反射序列化")]
    public void Parse_ReflectionMessage_ReturnsBody()
    {
        var msg = new JTMessage
        {
            Kind = MessageKinds.平台通用应答,
            Mobile = "13880011111",
            Sequence = 1,
        };

        var body = new T8001
        {
            Sequence = 100,
            Kind = MessageKinds.位置信息汇报,
            Result = ResultKinds.Success,
        };
        msg.SetResult(body);

        var parsed = MessageFactory.Instance.Parse(msg);
        Assert.NotNull(parsed);
        Assert.IsType<T8001>(parsed);

        var t8001 = (T8001)parsed;
        Assert.Equal(ResultKinds.Success, t8001.Result);
    }

    [Fact]
    [DisplayName("外部注册_RegisterAssembly")]
    public void RegisterAssembly_AddsNewTypes()
    {
        var before = MessageFactory.Instance.Maps.Count;
        MessageFactory.Instance.Register(typeof(JTMessage).Assembly);
        var after = MessageFactory.Instance.Maps.Count;

        // 重复注册不应减少数量
        Assert.True(after >= before);
    }

    [Fact]
    [DisplayName("类型名_GetKindName")]
    public void GetKindName_ReturnsName()
    {
        var name = MessageFactory.GetKindName(MessageKinds.位置信息汇报);
        Assert.Equal("位置信息汇报", name);

        name = MessageFactory.GetKindName(MessageKinds.终端心跳);
        Assert.Equal("终端心跳", name);
    }

    [Fact]
    [DisplayName("是否应答判定_IsReply")]
    public void IsReply_CorrectlyIdentifies()
    {
        Assert.True(MessageFactory.IsReply(MessageKinds.终端通用应答));
        Assert.True(MessageFactory.IsReply(MessageKinds.平台通用应答));
        Assert.True(MessageFactory.IsReply(MessageKinds.终端注册应答));

        Assert.False(MessageFactory.IsReply(MessageKinds.终端心跳));
        Assert.False(MessageFactory.IsReply(MessageKinds.终端注册));
        Assert.False(MessageFactory.IsReply(MessageKinds.终端鉴权));
    }
}
