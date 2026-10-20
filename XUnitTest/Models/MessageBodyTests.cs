using System.ComponentModel;
using NewLife;
using NewLife.JT808.Protocols;
using NewLife.JT808.Models;
using NewLife.JT808.Protocols;
using NewLife.JT808.Protocols;

namespace XUnitTest.Models;

/// <summary>关键消息体往返序列化测试</summary>
public class MessageBodyTests
{
    [Fact]
    [DisplayName("T0100_终端注册_反序列化")]
    public void T0100_AllFields()
    {
        var t0100 = new T0100
        {
            Province = 31,
            City = 755,
            Provider = "TEST01",
            Model = "JT808-TEST",
            DeviceId = "DEV123456",
            Color = CarColors.蓝色,
            PlateNo = "粤B88888",
        };

        var buf = SerializeDeserialize(t0100, MessageKinds.终端注册);
        Assert.IsType<T0100>(buf);
    }

    [Fact]
    [DisplayName("T0102_终端鉴权_2019版")]
    public void T0102_WithCodeAndIMEI()
    {
        var t0102 = new T0102
        {
            Code = "AUTH123456",
        };

        var buf = SerializeDeserialize(t0102, MessageKinds.终端鉴权);
        var result = (T0102)buf;
        Assert.Equal("AUTH123456", result.Code);
    }

    [Fact]
    [DisplayName("T0200_位置信息汇报_全部字段")]
    public void T0200_AllFields()
    {
        var t0200 = new T0200
        {
            Alarm = (PositionAlarms)0,
            Status = PositionStatus.ACC | PositionStatus.定位,
            Latitude = 22334455,
            Longitude = 114112233,
            Altitude = 50,
            Speed = 80,
            Direction = 270,
        };

        var buf = SerializeDeserialize(t0200, MessageKinds.位置信息汇报);
        var result = (T0200)buf;
        Assert.Equal(22334455, result.Latitude);
        Assert.Equal(114112233, result.Longitude);
        Assert.Equal(50, result.Altitude);
        Assert.Equal(80, result.Speed);
        Assert.Equal(270, result.Direction);
    }

    [Fact]
    [DisplayName("T0200_位置报警标志")]
    public void T0200_AlarmFlags()
    {
        var t0200 = new T0200
        {
            Alarm = PositionAlarms.紧急报瞥 | PositionAlarms.超速报警,
            Status = 0,
            Latitude = 22334455,
            Longitude = 114112233,
        };

        var buf = SerializeDeserialize(t0200, MessageKinds.位置信息汇报);
        var result = (T0200)buf;
        Assert.True(result.Alarm.Has(PositionAlarms.紧急报瞥));
        Assert.True(result.Alarm.Has(PositionAlarms.超速报警));
    }

    [Fact]
    [DisplayName("T8001_平台通用应答_三种结果")]
    public void T8001_AllResults()
    {
        foreach (var result in new[] { ResultKinds.Success, ResultKinds.Failure, ResultKinds.NotSupport })
        {
            var t8001 = new T8001
            {
                Sequence = 123,
                Kind = MessageKinds.位置信息汇报,
                Result = result,
            };

            var buf = SerializeDeserialize(t8001, MessageKinds.平台通用应答);
            var parsed = (T8001)buf;
            Assert.Equal(result, parsed.Result);
            Assert.Equal((UInt16)123, parsed.Sequence);
        }
    }

    [Fact]
    [DisplayName("T8100_终端注册应答")]
    public void T8100_RegisterReply()
    {
        var t8100 = new T8100
        {
            Sequence = 1,
            Result = RegisterResult.成功,
            Code = "ABCD1234",
        };

        var buf = SerializeDeserialize(t8100, MessageKinds.终端注册应答);
        var result = (T8100)buf;
        Assert.Equal(RegisterResult.成功, result.Result);
        Assert.Equal("ABCD1234", result.Code);
    }

    [Fact]
    [DisplayName("T8300_文本信息下发")]
    public void T8300_TextMessage()
    {
        var t8300 = new T8300
        {
            Flag = TextFlags.Service | TextFlags.Urgent,
            Kind = TextKinds.Notice,
            Text = "测试文本消息",
        };

        var buf = SerializeDeserialize(t8300, MessageKinds.文本信息下发);
        var result = (T8300)buf;
        Assert.Equal(TextFlags.Service | TextFlags.Urgent, result.Flag);
        Assert.Equal("测试文本消息", result.Text);
    }

    [Fact]
    [DisplayName("T0900_数据上行透传")]
    public void T0900_TransparentData()
    {
        var t0900 = new T0900
        {
            Kind = DataKinds.GNSS,
            Data = new Byte[] { 0x01, 0x02, 0x03, 0x04 },
        };

        var buf = SerializeDeserialize(t0900, MessageKinds.数据上行透传);
        var result = (T0900)buf;
        Assert.Equal(DataKinds.GNSS, result.Kind);
        Assert.NotNull(result.Data);
        Assert.True(result.Data.Length >= 4);
    }

    [Fact]
    [DisplayName("T0705_CAN数据_空或不足8字节补零")]
    public void T0705_ShortData_Padded()
    {
        var t0705 = new T0705
        {
            Time = new DateTime(2024, 1, 2, 3, 4, 5),
            Items =
            [
                new CanProperty { Id = 0x11, Data = null },
                new CanProperty { Id = 0x22, Data = new Byte[] { 1, 2, 3 } },
            ],
        };

        var buf = SerializeDeserialize(t0705, MessageKinds.CAN总线数据上传);
        var result = (T0705)buf;
        Assert.Equal(2, result.Items.Length);
        Assert.Equal(new Byte[8], result.Items[0].Data);
        Assert.Equal(new Byte[] { 1, 2, 3, 0, 0, 0, 0, 0 }, result.Items[1].Data);
    }

    [Fact]
    [DisplayName("T8103_设置终端参数_TLV列表")]
    public void T8103_ParameterSettings()
    {
        var t8103 = new T8103
        {
            Items = new List<TLV>
            {
                new() { Type = 0x0001, Value = BitConverter.GetBytes(30) }, // 心跳间隔30秒
                new() { Type = 0x0002, Value = BitConverter.GetBytes(60) }, // TCP超时60秒
            }
        };

        var buf = SerializeDeserialize(t8103, MessageKinds.设置终端参数);
        var result = (T8103)buf;
        Assert.NotNull(result.Items);
        Assert.True(result.Items.Count >= 2);
    }

    [Fact]
    [DisplayName("T0104_查询参数应答_TLV列表")]
    public void T0104_ParameterReply()
    {
        var t0104 = new T0104
        {
            Sequence = 1,
            Items = new List<TLV>
            {
                new() { Type = 0x0001, Value = BitConverter.GetBytes(30) },
            }
        };

        var buf = SerializeDeserialize(t0104, MessageKinds.查询终端参数应答);
        var result = (T0104)buf;
        Assert.Equal((UInt16)1, result.Sequence);
        Assert.NotNull(result.Items);
        Assert.True(result.Items.Count >= 1);
    }

    [Fact]
    [DisplayName("T0100_终端注册_2013版")]
    public void T0100_Version2013()
    {
        var t0100 = new T0100
        {
            Province = 31,
            City = 755,
            Provider = "ABC",
            Model = "MODEL-1",
            DeviceId = "DEV1",
            Color = CarColors.蓝色,
            PlateNo = "粤B12345",
        };

        var msg = new JTMessage { Kind = MessageKinds.终端注册, Mobile = "13880011111", Sequence = 1, Version = 0 };
        msg.SetResult(t0100);

        var pk = msg.ToPacket();
        Assert.NotNull(pk);

        var msg2 = new JTMessage();
        Assert.True(msg2.Read(pk));
        Assert.Equal(0, msg2.Version);

        var body = msg2.ReadModel<T0100>();
        Assert.Equal(31, body.Province);
        Assert.Equal(755, body.City);
        Assert.Equal("ABC", body.Provider);
        Assert.Equal("MODEL-1", body.Model);
        Assert.Equal("DEV1", body.DeviceId);
        Assert.Equal("粤B12345", body.PlateNo);
    }

    [Fact]
    [DisplayName("T0100_终端注册_2011版8字节型号")]
    public void T0100_Version2011_ShortModel()
    {
        var t0100 = new T0100
        {
            Province = 11,
            City = 100,
            Provider = "ABCDE",
            Model = "MODEL-11",
            DeviceId = "DEV11",
            Color = CarColors.黄色,
            PlateNo = "京A12345",
        };

        // 2011 与 2013 共用 Version=0，读取时按剩余长度识别 8/20 字节型号
        var msg = new JTMessage { Kind = MessageKinds.终端注册, Mobile = "13880011111", Sequence = 1, Version = 0 };
        msg.SetResult(t0100);

        var pk = msg.ToPacket();
        var msg2 = new JTMessage();
        Assert.True(msg2.Read(pk));

        var body = msg2.ReadModel<T0100>();
        Assert.Equal(11, body.Province);
        Assert.Equal("MODEL-11", body.Model);
        Assert.Equal("京A12345", body.PlateNo);
    }

    [Fact]
    [DisplayName("T0102_终端鉴权_2019版往返")]
    public void T0102_Version2019()
    {
        var t0102 = new T0102
        {
            Code = "AUTH123",
            IMEI = "123456789012345",
            Version = "1.0.0",
        };

        var msg = new JTMessage { Kind = MessageKinds.终端鉴权, Mobile = "13880011111", Sequence = 1, Version = 1 };
        msg.SetResult(t0102);

        var pk = msg.ToPacket();
        Assert.NotNull(pk);

        var msg2 = new JTMessage();
        Assert.True(msg2.Read(pk));

        var body = msg2.ReadModel<T0102>();
        Assert.Equal("AUTH123", body.Code);
        Assert.Equal("123456789012345", body.IMEI);
        Assert.Equal("1.0.0", body.Version);
    }

    #region 辅助
    /// <summary>序列化消息体到JTMessage再反序列化，返回反序列化结果</summary>
    private static Object SerializeDeserialize(Object body, MessageKinds kind)
    {
        // 确保消息工厂已扫描程序集（Init 幂等），避免测试执行顺序影响解析
        MessageFactory.Instance.Init();

        var msg = new JTMessage
        {
            Kind = kind,
            Mobile = "13880011111",
            Sequence = 1,
        };
        msg.SetResult(body);

        var pk = msg.ToPacket();
        Assert.NotNull(pk);

        var msg2 = new JTMessage();
        var ok = msg2.Read(pk);
        if (!ok) Assert.True(ok, $"Read failed for {kind}");

        var result = msg2.GetBody();
        Assert.NotNull(result);

        return result;
    }
    #endregion
}
