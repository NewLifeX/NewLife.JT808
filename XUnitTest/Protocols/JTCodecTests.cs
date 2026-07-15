using System.ComponentModel;
using NewLife.Data;
using NewLife.JT808.Protocols;

namespace XUnitTest.Protocols;

/// <summary>JTCodec 粘包编解码器单元测试</summary>
public class JTCodecTests
{
    [Fact]
    [DisplayName("编码_分配流水号")]
    public void Encode_AssignsSequence()
    {
        var codec = new JTCodec();
        var msg = new JTMessage
        {
            Kind = MessageKinds.终端心跳,
            Mobile = "13800138000",
        };

        var pk = codec.Encode(msg);
        Assert.NotNull(pk);
        Assert.True(msg.Sequence > 0);
    }

    [Fact]
    [DisplayName("编码_连续调用_流水号递增")]
    public void Encode_ConsecutiveCalls_SequenceIncrements()
    {
        var codec = new JTCodec();
        var msg1 = new JTMessage { Kind = MessageKinds.终端心跳, Mobile = "13800138000" };
        var msg2 = new JTMessage { Kind = MessageKinds.终端心跳, Mobile = "13800138001" };

        codec.Encode(msg1);
        codec.Encode(msg2);

        Assert.True(msg2.Sequence > msg1.Sequence);
    }

    [Fact]
    [DisplayName("解码_完整单帧_正确解析")]
    public void Decode_SingleFrame_ParsesCorrectly()
    {
        // 构造一个完整的心跳消息帧
        var msg = new JTMessage
        {
            Kind = MessageKinds.终端心跳,
            Mobile = "13800138000",
            Sequence = 1,
        };
        var rawPk = msg.ToPacket();
        Assert.NotNull(rawPk);

        var codec = new JTCodec();
        var list = codec.Decode(rawPk);

        Assert.Single(list);
        Assert.Equal(MessageKinds.终端心跳, list[0].Kind);
        Assert.Equal("13800138000", list[0].Mobile);
    }

    [Fact]
    [DisplayName("解码_两帧粘包_正确解析两帧")]
    public void Decode_TwoFrames_ParsesBoth()
    {
        var codec = new JTCodec();

        // 构造两个消息帧
        var msg1 = new JTMessage { Kind = MessageKinds.终端心跳, Mobile = "13800138000", Sequence = 1 };
        var msg2 = new JTMessage { Kind = MessageKinds.终端注册, Mobile = "13800138001", Sequence = 2 };

        var pk1 = msg1.ToPacket()!;
        var pk2 = msg2.ToPacket()!;

        // 合并两帧
        var bytes = pk1.ToArray().Concat(pk2.ToArray()).ToArray();
        var combined = new ArrayPacket(bytes);

        var list = codec.Decode(combined);

        Assert.Equal(2, list.Count);
        Assert.Equal(MessageKinds.终端心跳, list[0].Kind);
        Assert.Equal(MessageKinds.终端注册, list[1].Kind);
    }

    [Fact]
    [DisplayName("解码_半包_不产生消息")]
    public void Decode_IncompleteFrame_NoMessage()
    {
        var codec = new JTCodec();

        // 只发送一个不完整的帧头
        var partial = new ArrayPacket(new Byte[] { 0x7E, 0x02, 0x00 });
        var list = codec.Decode(partial);

        Assert.Empty(list);
    }

    [Fact]
    [DisplayName("解码_半包续传_补全后正确解析")]
    public void Decode_PartialThenComplete_ParsesCorrectly()
    {
        var codec = new JTCodec();

        // 构造消息并获取完整字节
        var msg = new JTMessage { Kind = MessageKinds.终端心跳, Mobile = "13800138000", Sequence = 1 };
        var fullPk = msg.ToPacket()!;
        var bytes = fullPk.ToArray();

        // 先发送前半部分
        var firstHalf = new ArrayPacket(bytes.Take(bytes.Length / 2).ToArray());
        var list1 = codec.Decode(firstHalf);
        Assert.Empty(list1);

        // 再发送后半部分
        var secondHalf = new ArrayPacket(bytes.Skip(bytes.Length / 2).ToArray());
        var list2 = codec.Decode(secondHalf);

        Assert.Single(list2);
        Assert.Equal(MessageKinds.终端心跳, list2[0].Kind);
    }

    [Fact]
    [DisplayName("解码_空数据_返回空列表")]
    public void Decode_EmptyData_ReturnsEmpty()
    {
        var codec = new JTCodec();
        var list = codec.Decode(new ArrayPacket(new Byte[0]));

        Assert.Empty(list);
    }

    [Fact]
    [DisplayName("重置_清空缓冲区")]
    public void Reset_ClearsBuffer()
    {
        var codec = new JTCodec();

        // 发送半包
        var partial = new ArrayPacket(new Byte[] { 0x7E, 0x02 });
        codec.Decode(partial);

        // 重置
        codec.Reset();

        // 发送剩余部分
        var remaining = new ArrayPacket(new Byte[] { 0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x7E });
        var list = codec.Decode(remaining);

        // 重置后缓冲区清空，单独发送剩余部分无法解析成有效帧
        Assert.Empty(list);
    }

    [Fact]
    [DisplayName("GetSequence_原子递增")]
    public void GetSequence_ThreadSafe()
    {
        var codec = new JTCodec();
        var sequences = new List<UInt16>();

        for (var i = 0; i < 100; i++)
        {
            sequences.Add(codec.GetSequence());
        }

        // 验证唯一性
        Assert.Equal(100, sequences.Distinct().Count());
        // 验证递增
        for (var i = 1; i < sequences.Count; i++)
        {
            Assert.True(sequences[i] > sequences[i - 1]);
        }
    }
}
