using NewLife.Data;
using NewLife.Messaging;

namespace NewLife.JT808.Protocols;

/// <summary>JT/T 808网络粘包编解码器</summary>
/// <remarks>
/// 处理TCP粘包/半包，根据0x7E帧边界分割数据流。
/// 每会话一个实例，在TCP收到数据时将字节追加到缓冲区，然后逐帧解析。
/// </remarks>
public class JTCodec
{
    private Int32 _gid;
    private MemoryStream _buffer = new();

    /// <summary>获取下一个消息流水号</summary>
    /// <returns></returns>
    public UInt16 GetSequence() => (UInt16)Interlocked.Increment(ref _gid);

    /// <summary>编码消息为数据包（分配流水号并转为Packet）</summary>
    /// <param name="message">消息</param>
    /// <returns>数据包</returns>
    public virtual IPacket Encode(JTMessage message)
    {
        if (message.Sequence == 0) message.Sequence = GetSequence();

        return message.ToPacket();
    }

    /// <summary>解码数据包为消息列表（处理粘包）</summary>
    /// <param name="pk">原始数据包</param>
    /// <returns>解码后的消息列表</returns>
    public virtual IList<JTMessage> Decode(IPacket pk)
    {
        // 将新数据追加到缓冲区
        pk.CopyTo(_buffer);

        return ParseBuffer();
    }

    /// <summary>从缓冲区解析所有完整帧</summary>
    /// <returns>消息列表</returns>
    protected virtual IList<JTMessage> ParseBuffer()
    {
        var list = new List<JTMessage>();
        if (_buffer.Length == 0) return list;

        IPacket data = new ArrayPacket(_buffer.ToArray());

        while (data.Total > 0)
        {
            // 查找帧头0x7E
            var startIdx = 0;
            while (startIdx < data.Total && data[startIdx] != 0x7E) startIdx++;
            if (startIdx >= data.Total) break;

            // 获取完整帧长度
            var frameLen = JTMessage.GetLength(data.Slice(startIdx));
            if (frameLen <= 0 || startIdx + frameLen > data.Total) break;

            // 提取完整帧
            var frame = data.Slice(startIdx, frameLen);
            var msg = new JTMessage();
            if (msg.Read(frame))
                list.Add(msg);

            // 移动到下一帧
            data = data.Slice(startIdx + frameLen);
        }

        // 保留未处理完的数据
        _buffer.SetLength(0);
        if (data.Total > 0)
            data.CopyTo(_buffer);

        return list;
    }

    /// <summary>重置解码器状态（连接断开时调用）</summary>
    public virtual void Reset()
    {
        _buffer.SetLength(0);
    }
}

