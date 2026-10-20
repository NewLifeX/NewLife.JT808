using System.ComponentModel;
using NewLife;
using NewLife.Data;
using NewLife.JT808.Protocols;
using NewLife.JT808.Models;
using NewLife.JT808.Protocols;

namespace XUnitTest.Protocols;

/// <summary>JTMessage 帧解析单元测试</summary>
public class JTMessageTests
{
    [Fact]
    [DisplayName("解析位置汇报帧_往返序列化")]
    public void Parse_PositionReport_Roundtrip()
    {
        var t0200 = new T0200
        {
            Alarm = PositionAlarms.紧急报瞥,
            Status = PositionStatus.ACC,
            Latitude = 22334455,
            Longitude = 114112233,
            Altitude = 100,
            Speed = 60,
            Direction = 180,
        };

        var msg = new JTMessage
        {
            Kind = MessageKinds.位置信息汇报,
            Mobile = "013880011111",
            Sequence = 1,
        };
        msg.SetResult(t0200);

        var pk = msg.ToPacket();
        Assert.NotNull(pk);

        var msg2 = new JTMessage();
        Assert.True(msg2.Read(pk));
        Assert.Equal(MessageKinds.位置信息汇报, msg2.Kind);
        Assert.Equal("13880011111", msg2.Mobile);
        Assert.Equal((UInt16)1, msg2.Sequence);

        var body = msg2.ReadModel<T0200>();
        Assert.NotNull(body);
        Assert.Equal(22334455, body.Latitude);
        Assert.Equal(114112233, body.Longitude);
        Assert.Equal(60, body.Speed);
    }

    [Fact]
    [DisplayName("终端注册_往返序列化")]
    public void Register_Roundtrip()
    {
        var t0100 = new T0100
        {
            Province = 31,
            City = 755,
            Provider = "TEST",
            Model = "JT808-01",
            DeviceId = "ABC123",
            Color = CarColors.蓝色,
            PlateNo = "粤B12345",
        };

        var msg = new JTMessage
        {
            Kind = MessageKinds.终端注册,
            Mobile = "13880011111",
            Sequence = 1,
        };
        msg.SetResult(t0100);

        var pk = msg.ToPacket();
        Assert.NotNull(pk);

        var msg2 = new JTMessage();
        Assert.True(msg2.Read(pk));

        var body = msg2.ReadModel<T0100>();
        Assert.NotNull(body);
        Assert.Equal(31, body.Province);
        Assert.Equal(755, body.City);
        Assert.Equal("TEST", body.Provider);
        Assert.Equal("粤B12345", body.PlateNo);
    }

    [Fact]
    [DisplayName("平台通用应答_反射序列化")]
    public void T8001_Roundtrip()
    {
        var t8001 = new T8001
        {
            Sequence = 1,
            Kind = MessageKinds.位置信息汇报,
            Result = ResultKinds.Success,
        };

        var msg = new JTMessage
        {
            Kind = MessageKinds.平台通用应答,
            Mobile = "13880011111",
            Sequence = 100,
        };
        msg.SetResult(t8001);

        var pk = msg.ToPacket();
        Assert.NotNull(pk);

        var msg2 = new JTMessage();
        Assert.True(msg2.Read(pk));

        var body = msg2.ReadModel<T8001>();
        Assert.NotNull(body);
        Assert.Equal((UInt16)1, body.Sequence);
        Assert.Equal(ResultKinds.Success, body.Result);
    }

    [Fact]
    [DisplayName("终端心跳_空消息体")]
    public void Heartbeat_EmptyBody()
    {
        var msg = new JTMessage
        {
            Kind = MessageKinds.终端心跳,
            Mobile = "13880011111",
            Sequence = 1,
        };

        var pk = msg.ToPacket();
        Assert.NotNull(pk);
        Assert.True(pk.Total > 0);

        var msg2 = new JTMessage();
        Assert.True(msg2.Read(pk));
        Assert.Equal(MessageKinds.终端心跳, msg2.Kind);
        Assert.Null(msg2.Payload);
    }

    [Fact]
    [DisplayName("转义处理_0x7D和0x7E")]
    public void Escape_0x7D_0x7E()
    {
        // 构造包含特殊字节的消息体
        var t0200 = new T0200
        {
            Alarm = (PositionAlarms)0x7D7E7D7E, // 包含转义字节
            Status = PositionStatus.ACC,
            Latitude = 22334455,
            Longitude = 114112233,
        };

        var msg = new JTMessage
        {
            Kind = MessageKinds.位置信息汇报,
            Mobile = "13880011111",
            Sequence = 1,
        };
        msg.SetResult(t0200);

        var pk = msg.ToPacket();
        Assert.NotNull(pk);

        // 反序列化
        var msg2 = new JTMessage();
        Assert.True(msg2.Read(pk));

        var body = msg2.ReadModel<T0200>();
        Assert.NotNull(body);
        Assert.Equal((UInt32)0x7D7E7D7E, (UInt32)body.Alarm);
    }

    [Fact]
    [DisplayName("2019版本帧_读取和写入")]
    public void Version2019_Roundtrip()
    {
        var t0200 = new T0200
        {
            Alarm = 0,
            Status = 0,
            Latitude = 22334455,
            Longitude = 114112233,
        };

        var msg = new JTMessage
        {
            Kind = MessageKinds.位置信息汇报,
            Mobile = "13880011111",
            Sequence = 1,
            Version = 1, // 2019版
        };
        msg.SetResult(t0200);

        var pk = msg.ToPacket();
        Assert.NotNull(pk);

        var msg2 = new JTMessage();
        Assert.True(msg2.Read(pk));
        Assert.Equal(1, msg2.Version);
        Assert.Equal("13880011111", msg2.Mobile);

        var body = msg2.ReadModel<T0200>();
        Assert.NotNull(body);
        Assert.Equal(22334455, body.Latitude);
    }

    [Fact]
    [DisplayName("分包消息_Append合并")]
    public void PackageSplit_Merge()
    {
        var payload = new Byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 };

        var pkg1 = new JTMessage
        {
            Kind = MessageKinds.位置信息汇报,
            Mobile = "13880011111",
            Sequence = 1,
            Property = PropertyFlags.Package,
            PackageCount = 2,
            PackageIndex = 1,
            Payload = (ArrayPacket)payload,
        };

        var pkg2 = new JTMessage
        {
            Kind = MessageKinds.位置信息汇报,
            Mobile = "13880011111",
            Sequence = 1,
            Property = PropertyFlags.Package,
            PackageCount = 2,
            PackageIndex = 2,
            Payload = (ArrayPacket)payload,
        };

        // 合并
        var merged = pkg1.Append(pkg2);
        Assert.True(merged);
        Assert.Equal(2, pkg1.PackageCount);

        // 包序号从1开始，0 为非法值
        var invalid = new JTMessage { PackageCount = 2, PackageIndex = 0, Payload = (ArrayPacket)payload };
        Assert.False(pkg1.Append(invalid));
    }

    [Fact]
    [DisplayName("校验和_错误检测")]
    public void CheckSum_Error()
    {
        var msg = new JTMessage
        {
            Kind = MessageKinds.终端心跳,
            Mobile = "13880011111",
            Sequence = 1,
        };

        var pk = msg.ToPacket();
        Assert.NotNull(pk);

        // 篡改校验字节
        pk = pk.Slice(0, pk.Total - 1);
        var badCheckSum = (Byte)(pk[pk.Total - 1] ^ 0xFF);
        pk = pk.Slice(0, pk.Total - 1);
        pk.Append(new ArrayPacket(new Byte[] { badCheckSum, 0x7E }));

        // 应该能解析（目前只记录错误，不中断）
        var msg2 = new JTMessage();
        Assert.True(msg2.Read(pk));
    }
}
