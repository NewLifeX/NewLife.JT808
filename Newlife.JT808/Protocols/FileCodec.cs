using NewLife.Data;
using NewLife.JT808.SuBiao;

namespace NewLife.JT808.Protocols;

/// <summary>告警文件粘包编解码器</summary>
/// <remarks>
/// 处理告警附件文件的粘包/半包，依据文件信息帧的 ASCII 帧头 "00cd" 分割数据流。
/// 帧结构：帧头4 + 文件名50 + 偏移4 + 总长4 + 负载长度4 + 负载N。
/// 通常与 <see cref="JTCodec"/> 配合使用，用于独立的文件传输通道。
/// </remarks>
public class FileCodec
{
    /// <summary>文件信息帧固定头长度（帧头4 + 文件名50 + 偏移4 + 总长4 + 负载长度4）</summary>
    private const Int32 HeaderSize = 4 + 50 + 4 + 4 + 4;

    /// <summary>接收缓冲区上限</summary>
    private const Int32 MaxBufferSize = 1024 * 1024;

    private readonly MemoryStream _buffer = new();

    /// <summary>解码数据包为文件消息列表（处理粘包）</summary>
    /// <param name="pk">原始数据包</param>
    /// <returns>解码后的文件消息列表</returns>
    public virtual IList<FileMessage> Decode(IPacket pk)
    {
        // 将新数据追加到缓冲区
        pk.CopyTo(_buffer);

        return ParseBuffer();
    }

    /// <summary>从缓冲区解析所有完整文件帧</summary>
    /// <returns>文件消息列表</returns>
    protected virtual IList<FileMessage> ParseBuffer()
    {
        var list = new List<FileMessage>();
        if (_buffer.Length == 0) return list;

        IPacket data = new ArrayPacket(_buffer.ToArray());

        while (data.Total > 0)
        {
            // 查找帧头 "00cd"
            var startIdx = 0;
            while (startIdx < data.Total - 3 &&
                   !(data[startIdx] == 0x30 && data[startIdx + 1] == 0x30 && data[startIdx + 2] == 0x63 && data[startIdx + 3] == 0x64)) startIdx++;
            if (startIdx >= data.Total - 3) break;

            // 头部不完整，等待后续数据
            if (startIdx + HeaderSize > data.Total) break;

            // 负载长度（大端），位于头部最后一个字段
            var lenPos = startIdx + HeaderSize - 4;
            var payloadLen = data[lenPos] << 24 | data[lenPos + 1] << 16 | data[lenPos + 2] << 8 | data[lenPos + 3];
            if (payloadLen < 0 || payloadLen > MaxBufferSize) break;

            var frameLen = HeaderSize + payloadLen;
            if (startIdx + frameLen > data.Total) break;

            var frame = data.Slice(startIdx, frameLen);
            var msg = new FileMessage();
            if (!msg.Read(frame)) break;

            list.Add(msg);

            // 移动到下一帧
            data = data.Slice(startIdx + frameLen);
        }

        // 保留未处理完的数据
        _buffer.SetLength(0);
        if (data.Total > 0 && data.Total <= MaxBufferSize)
            data.CopyTo(_buffer);

        return list;
    }

    /// <summary>重置解码器状态</summary>
    public virtual void Reset()
    {
        _buffer.SetLength(0);
    }
}
