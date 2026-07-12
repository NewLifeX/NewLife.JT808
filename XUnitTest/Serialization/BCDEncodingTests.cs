using System.ComponentModel;
using System.Text;
using NewLife.JT808.Protocols;

namespace XUnitTest.Serialization;

/// <summary>BCD 编码/解码单元测试</summary>
public class BCDEncodingTests
{
    [Fact]
    [DisplayName("BCD字符串_编码解码")]
    public void BCDString_EncodeDecode()
    {
        var attr = new BCDStringAttribute(6);
        var data = new Byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0x12 };

        // 手动 BCD 编解码验证
        var result = BCDString(data);
        Assert.Equal("123456789012", result);
    }

    [Fact]
    [DisplayName("BCD时间_编码解码")]
    public void BCDTime_EncodeDecode()
    {
        var attr = new BCDTimeAttribute(6, "yyMMddHHmmss");
        var data = new Byte[] { 0x26, 0x07, 0x12, 0x10, 0x30, 0x00 };

        // 手动 BCD 编解码验证
        var result = BCDString(data);
        Assert.Equal("260712103000", result);
    }

    [Fact]
    [DisplayName("BCD时间_边界值_0点")]
    public void BCDTime_Midnight()
    {
        var data = new Byte[] { 0x26, 0x07, 0x12, 0x00, 0x00, 0x00 };
        var result = BCDString(data);
        Assert.Equal("260712000000", result);
    }

    /// <summary>BCD 字节转字符串</summary>
    private static String BCDString(Byte[] data)
    {
        var sb = new StringBuilder(data.Length * 2);
        foreach (var b in data)
        {
            sb.Append((Char)('0' + (b >> 4)));
            sb.Append((Char)('0' + (b & 0x0F)));
        }
        return sb.ToString();
    }
}
