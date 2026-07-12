using NewLife.Data;
using NewLife.JT808.SuBiao;

namespace NewLife.JT808.Protocols;

/// <summary>告警文件粘包编解码器</summary>
/// <remarks>
/// 处理告警附件文件的粘包/半包，根据 0x00CD 魔数帧边界分割数据流。
/// 通常与 <see cref="JTCodec"/> 配合使用，用于独立的文件传输通道。
/// </remarks>
public class FileCodec
{
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
            // 查找魔数 0x00CD
            var startIdx = 0;
            while (startIdx < data.Total - 1 && (data[startIdx] != 0x00 || data[startIdx + 1] != 0xCD)) startIdx++;
            if (startIdx >= data.Total - 1) break;

            // 文件消息最小长度约 12 字节
            if (startIdx + 12 > data.Total) break;

            var frame = data.Slice(startIdx);
            var msg = new FileMessage();
            if (!msg.Read(frame)) break;

            // 计算帧长度（魔数2 + 属性2 + 文件名长度 + 偏移4 + 长度4 + 数据 + 校验1）
            var frameLen = 2 + 2 + (msg.FileName?.Length ?? 0) + 4 + 4 + (msg.Payload?.Length ?? 0) + 1;
            if (startIdx + frameLen > data.Total) break;

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

    /// <summary>重置解码器状态</summary>
    public virtual void Reset()
    {
        _buffer.SetLength(0);
    }
}
