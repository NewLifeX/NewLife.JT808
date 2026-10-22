using System.ComponentModel;
using NewLife;
using NewLife.JT808.Protocols;

namespace XUnitTest.Protocols;

/// <summary>TLV/KLV 编解码单元测试</summary>
public class TLVKLVTests
{
    [Fact]
    [DisplayName("TLV_Convert_强类型字典")]
    public void TLV_Convert_ToDictionary()
    {
        var items = new[]
        {
            new TLV { Type = 0x0100, Value = new Byte[] { 0x00, 0x00, 0x00, 0x01 } },
        };

        var dic = TLV.Convert<UInt32>(items);
        Assert.Single(dic);
        Assert.Equal((UInt32)1, dic[0x0100]);
    }

    [Fact]
    [DisplayName("KLV_Convert_位置附加信息")]
    public void KLV_Convert_Mileage()
    {
        var items = new[]
        {
            new KLV { Kind = 0x01, Value = new Byte[] { 0x00, 0x00, 0x01, 0x2C } },
        };

        var dic = KLV.Convert<Byte>(items);
        Assert.Single(dic);
    }

    [Fact]
    [DisplayName("KLV_Read_手动构造数据")]
    public void KLV_Read_ManualData()
    {
        var data = new Byte[] { 0x01, 0x04, 0x00, 0x00, 0x00, 0x64 };
        var items = KLV.Read(data);
        Assert.Single(items);
        Assert.Equal((Byte)0x01, items[0].Kind);
    }

    [Fact]
    [DisplayName("KLV_截断数据_按实际长度截取")]
    public void KLV_Read_Truncated()
    {
        // 声明长度 8，实际只有 3 字节
        var data = new Byte[] { 0x01, 0x08, 0xAA, 0xBB, 0xCC };
        var items = KLV.Read(data);
        Assert.Single(items);
        Assert.Equal(3, items[0].Value.Length);
        Assert.Equal(new Byte[] { 0xAA, 0xBB, 0xCC }, items[0].Value);
    }

    [Fact]
    [DisplayName("TLV_截断数据_按实际长度截取")]
    public void TLV_Read_Truncated()
    {
        // 类型 4 字节 + 长度 1 字节，声明长度 8，实际只有 3 字节
        var data = new Byte[] { 0x00, 0x00, 0x01, 0x00, 0x08, 0xAA, 0xBB, 0xCC };
        var items = TLV.Read(data);
        Assert.Single(items);
        Assert.Equal(3, items[0].Value.Length);
    }

    [Fact]
    [DisplayName("TLV_Write_验证输出")]
    public void TLV_Write_ProducesOutput()
    {
        var tlv = new TLV { Type = 0x0100, Value = new Byte[] { 0x00, 0x00, 0x00, 0x01 } };
        var ms = new MemoryStream();
        TLV.Write(ms, new[] { tlv });
        Assert.True(ms.Length > 0);
    }

    [Fact]
    [DisplayName("KLV_Write_验证输出")]
    public void KLV_Write_ProducesOutput()
    {
        var klv = new KLV { Kind = 0x01, Value = new Byte[] { 0x00, 0x00, 0x00, 0x64 } };
        var ms = new MemoryStream();
        KLV.Write(ms, new[] { klv });
        Assert.True(ms.Length > 0);
    }
}
