using System.ComponentModel;
using NewLife.JT808;
using NewLife.JT808.Models;
using NewLife.JT808.Protocols;

namespace XUnitTest.Models;

/// <summary>驾驶员身份与参数应答消息体测试</summary>
public class DriverCardTests
{
    #region T0702 驾驶员身份信息采集上报
    [Fact]
    [DisplayName("T0702_2019版_插入读卡成功_往返")]
    public void T0702_Version2019_Insert()
    {
        var source = new T0702
        {
            Status = CardStatus.插入,
            Time = new DateTime(2026, 10, 20, 9, 30, 5),
            Result = CardResult.读卡成功,
            UserName = "张三",
            QualificationCode = "12345678901234567890",
            LicenseIssuing = "交通局",
            CertificateExpire = "20301231",
            IdentityCard = "110101199001011234",
            FaceMatch = 88,
            UID = "UID123456",
        };

        var body = RoundTrip(source, version: 1);

        Assert.Equal(CardStatus.插入, body.Status);
        Assert.Equal(new DateTime(2026, 10, 20, 9, 30, 5), body.Time);
        Assert.Equal(CardResult.读卡成功, body.Result);
        Assert.Equal("张三", body.UserName);
        Assert.Equal("12345678901234567890", body.QualificationCode);
        Assert.Equal("交通局", body.LicenseIssuing);
        Assert.Equal("20301231", body.CertificateExpire);
        Assert.Equal("110101199001011234", body.IdentityCard);
        Assert.Equal((Byte)88, body.FaceMatch);
        Assert.Equal("UID123456", body.UID);
    }

    [Fact]
    [DisplayName("T0702_2019版_拔出_往返")]
    public void T0702_Version2019_Remove()
    {
        var source = new T0702
        {
            Status = CardStatus.拔出,
            Time = new DateTime(2026, 10, 20, 18, 0, 0),
        };

        var body = RoundTrip(source, version: 1);

        Assert.Equal(CardStatus.拔出, body.Status);
        Assert.Equal(new DateTime(2026, 10, 20, 18, 0, 0), body.Time);
    }

    [Fact]
    [DisplayName("T0702_2011版_驾驶员信息往返")]
    public void T0702_Version2011()
    {
        var source = new T0702
        {
            UserName = "李四",
            IdentityCard = "110101199002021234",
            QualificationCode = "09876543210987654321",
            LicenseIssuing = "运输管理处",
        };

        var body = RoundTrip(source, version: 0);

        Assert.Equal("李四", body.UserName);
        Assert.Equal("110101199002021234", body.IdentityCard);
        Assert.Equal("09876543210987654321", body.QualificationCode);
        Assert.Equal("运输管理处", body.LicenseIssuing);
    }

    [Fact]
    [DisplayName("T0702_空上下文_按2011版写入不抛异常")]
    public void T0702_NullContext_Writes2011()
    {
        var source = new T0702
        {
            UserName = "王五",
            IdentityCard = "110101199003031234",
            QualificationCode = "11111111111111111111",
            LicenseIssuing = "车管所",
        };

        using var ms = new MemoryStream();
        Assert.True(source.Write(ms, null));

        ms.Position = 0;
        var body = new T0702();
        Assert.True(body.Read(ms, null));
        Assert.Equal("王五", body.UserName);
        Assert.Equal("车管所", body.LicenseIssuing);
    }
    #endregion

    #region T0104 查询终端参数应答
    [Fact]
    [DisplayName("T0104_参数转换_字符串与屏蔽字")]
    public void T0104_GetParameters()
    {
        var encoding = NewLife.JT808.Protocols.Helper.Encoding;
        var t0104 = new T0104
        {
            Sequence = 7,
            Items =
            [
                new TLV { Type = (UInt32)ParameterKinds.主APN, Value = encoding.GetBytes("CMNET\0") },
                new TLV { Type = (UInt32)ParameterKinds.报警屏蔽字, Value = [0x00, 0x00, 0x00, 0x08] },
            ],
        };

        var dic = t0104.GetParameters();

        Assert.Equal("CMNET", dic[ParameterKinds.主APN]);
        Assert.Equal("00000008", dic[ParameterKinds.报警屏蔽字]);
    }

    [Fact]
    [DisplayName("T0104_无参数_返回空字典")]
    public void T0104_GetParameters_Empty()
    {
        var t0104 = new T0104 { Sequence = 1 };

        var dic = t0104.GetParameters();

        Assert.NotNull(dic);
        Assert.Empty(dic);
    }
    #endregion

    #region 辅助
    private static T0702 RoundTrip(T0702 source, Byte version)
    {
        MessageFactory.Instance.Init();

        var msg = new JTMessage
        {
            Kind = MessageKinds.驾驶员上报,
            Mobile = "13880011111",
            Sequence = 1,
            Version = version,
        };
        msg.SetResult(source);

        var pk = msg.ToPacket();
        Assert.NotNull(pk);

        var msg2 = new JTMessage();
        Assert.True(msg2.Read(pk));

        return msg2.ReadModel<T0702>();
    }
    #endregion
}
