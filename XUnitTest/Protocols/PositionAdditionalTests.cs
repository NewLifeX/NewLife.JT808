using System.ComponentModel;
using NewLife;
using NewLife.JT808.Models;
using NewLife.JT808.Protocols;

namespace XUnitTest.Protocols;

/// <summary>位置附加信息边界测试</summary>
public class PositionAdditionalTests
{
    /// <summary>构造位置汇报消息体，追加指定附加信息</summary>
    /// <param name="additional">附加信息原始字节</param>
    /// <returns>解析后的位置汇报</returns>
    private static T0200 ReadPosition(Byte[] additional)
    {
        using var ms = new MemoryStream();
        var writer = Helper.CreateWriter(ms);
        writer.WriteUInt32(0);          // 报警标志
        writer.WriteUInt32(0);          // 状态位
        writer.WriteInt32(113000000);   // 纬度
        writer.WriteInt32(23000000);    // 经度
        writer.WriteUInt16(100);        // 海拔
        writer.WriteUInt16(600);        // 速度
        writer.WriteUInt16(90);         // 方向
        writer.WriteBCD("240101000000", 6);
        if (additional.Length > 0) ms.Write(additional, 0, additional.Length);

        ms.Position = 0;
        var msg = new T0200();
        Assert.True(msg.Read(ms, null));
        return msg;
    }

    [Fact]
    [DisplayName("位置附加信息_无附加数据_返回空字典")]
    public void ReadAdditional_Empty()
    {
        var msg = ReadPosition([]);

        Assert.NotNull(msg.Additionals);
        Assert.Empty(msg.Additionals);
    }

    [Fact]
    [DisplayName("位置附加信息_截断KLV_按实际长度解析")]
    public void ReadAdditional_Truncated()
    {
        // 里程 KLV：类型 0x01，声明长度 8，实际只有 2 字节
        var msg = ReadPosition([0x01, 0x08, 0x01, 0x02]);

        Assert.NotNull(msg.Additionals);
        Assert.Single(msg.Additionals);
        Assert.Equal((UInt16)0x0102, msg.Additionals[AdditionalKinds.里程]);
    }
}
