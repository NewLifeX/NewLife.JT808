using System.ComponentModel;
using NewLife.Data;
using NewLife.JT808.Protocols;
using NewLife.JT808.SuBiao;

namespace XUnitTest.Protocols;

/// <summary>告警附件文件粘包编解码器单元测试</summary>
public class FileCodecTests
{
    private static FileMessage CreateMessage(String name, Byte[] payload) => new()
    {
        FileName = name,
        Offset = 0,
        Length = payload.Length,
        Payload = payload,
    };

    [Fact]
    [DisplayName("文件帧_单帧往返")]
    public void Decode_SingleFrame()
    {
        var source = CreateMessage("test.jpg", new Byte[] { 0x01, 0x02, 0x03 });
        var pk = source.ToPacket();
        Assert.NotNull(pk);

        var list = new FileCodec().Decode(pk);

        var msg = Assert.Single(list);
        Assert.Equal("test.jpg", msg.FileName);
        Assert.Equal(source.Length, msg.Length);
        Assert.Equal(source.Payload, msg.Payload);
    }

    [Fact]
    [DisplayName("文件帧_两帧粘包")]
    public void Decode_TwoFrames()
    {
        var a = CreateMessage("a.bin", new Byte[] { 1 });
        var b = CreateMessage("b.bin", new Byte[] { 2, 3, 4 });

        var bytes = a.ToPacket().ToArray().Concat(b.ToPacket().ToArray()).ToArray();
        var list = new FileCodec().Decode(new ArrayPacket(bytes));

        Assert.Equal(2, list.Count);
        Assert.Equal("a.bin", list[0].FileName);
        Assert.Equal("b.bin", list[1].FileName);
        Assert.Equal(3, list[1].Payload.Length);
    }

    [Fact]
    [DisplayName("文件帧_半包续传")]
    public void Decode_PartialThenComplete()
    {
        var source = CreateMessage("split.bin", new Byte[] { 9, 8, 7, 6, 5 });
        var bytes = source.ToPacket().ToArray();

        var codec = new FileCodec();

        var first = codec.Decode(new ArrayPacket(bytes.Take(40).ToArray()));
        Assert.Empty(first);

        var second = codec.Decode(new ArrayPacket(bytes.Skip(40).ToArray()));
        var msg = Assert.Single(second);
        Assert.Equal("split.bin", msg.FileName);
        Assert.Equal(source.Payload, msg.Payload);
    }

    [Fact]
    [DisplayName("文件帧_前置垃圾数据可跳过")]
    public void Decode_GarbagePrefix()
    {
        var source = CreateMessage("g.bin", new Byte[] { 0xAA });
        var bytes = new Byte[] { 0x00, 0xCD, 0xFF }
            .Concat(source.ToPacket().ToArray())
            .ToArray();

        var list = new FileCodec().Decode(new ArrayPacket(bytes));

        var msg = Assert.Single(list);
        Assert.Equal("g.bin", msg.FileName);
    }

    [Fact]
    [DisplayName("文件消息_空流返回失败")]
    public void FileMessage_NullStream()
    {
        var msg = CreateMessage("x.bin", []);
        Assert.False(msg.Read((Stream)null, null));
        Assert.False(msg.Write((Stream)null, null));
    }
}
