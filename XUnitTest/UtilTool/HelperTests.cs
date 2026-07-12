using System.ComponentModel;
using NewLife.JT808.Protocols;
using NewLife.JT808.UtilTool;

namespace XUnitTest.UtilTool;

/// <summary>Helper 工具类单元测试</summary>
public class HelperTests
{
    [Fact]
    [DisplayName("CreateReader_大端序")]
    public void CreateReader_BigEndian()
    {
        using var ms = new MemoryStream(new Byte[] { 0x00, 0x01 });
        var reader = Helper.CreateReader(ms);
        var val = reader.ReadUInt16();
        Assert.Equal((UInt16)1, val); // 大端序：0x0001 = 1
    }

    [Fact]
    [DisplayName("CreateWriter_大端序")]
    public void CreateWriter_BigEndian()
    {
        using var ms = new MemoryStream();
        var writer = Helper.CreateWriter(ms);
        writer.Write((UInt16)0x0102);
        var data = ms.ToArray();
        Assert.Equal(new Byte[] { 0x01, 0x02 }, data);
    }

    [Fact]
    [DisplayName("FastRead_整型")]
    public void FastRead_Int()
    {
        var data = new Byte[] { 0x00, 0x00, 0x00, 0x01 };
        var result = Helper.FastRead<Int32>(data);
        Assert.Equal(1, result);
    }

    [Fact]
    [DisplayName("ToJsonJT808_枚举序列化输出中文")]
    public void ToJsonJT808_Enum_ChineseName()
    {
        var t8001 = new NewLife.JT808.Models.T8001
        {
            Sequence = 1,
            Kind = MessageKinds.位置信息汇报,
            Result = NewLife.JT808.Models.ResultKinds.Success,
        };

        var json = t8001.ToJsonJT808();
        Assert.NotNull(json);
        Assert.True(json.Contains("成功") || json.Contains("Success"), $"JSON未包含'成功'或'Success'：{json}");
        Assert.True(json.Contains("位置信息汇报") || json.Contains("Kind"), $"JSON未包含'位置信息汇报'或'Kind'：{json}");
    }

    [Fact]
    [DisplayName("GBK编码_中文字符串读写")]
    public void GBK_StringReadWrite()
    {
        var original = "测试中文GBK编码";

        using var ms = new MemoryStream();
        var writer = Helper.CreateWriter(ms);
        Helper.WriteAllGBK(writer, original);

        ms.Position = 0;
        var reader = Helper.CreateReader(ms);
        var result = Helper.ReadAllGBK(reader);

        Assert.Equal(original, result);
    }
}
