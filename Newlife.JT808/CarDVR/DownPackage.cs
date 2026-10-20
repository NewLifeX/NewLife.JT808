using NewLife.Serialization;

namespace NewLife.JT808.CarDVR;

/// <summary>行车记录仪下行数据包</summary>
/// <remarks>
/// JT/T 19056 汽车行驶记录仪协议下行数据包。
/// 帧结构：起始字头(0x557A 2B) + 命令字(1B) + 数据块长度(2B) + 保留(1B) + 数据体 + 校验码(1B)
/// </remarks>
public class DownPackage
{
    /// <summary>起始字头</summary>
    public const UInt16 BeginFlag = 0x557A;

    /// <summary>起始字头</summary>
    public UInt16 Begin { get; set; } = BeginFlag;

    /// <summary>命令字</summary>
    public Byte CommandId { get; set; }

    /// <summary>数据块长度</summary>
    public UInt16 DataLength { get; set; }

    /// <summary>保留字段</summary>
    public Byte KeepFields { get; set; }

    /// <summary>下行数据体</summary>
    public DownBody? Bodies { get; set; }

    /// <summary>校验字</summary>
    public Byte CheckCode { get; set; }

    /// <summary>从数据流读取</summary>
    /// <param name="stream">数据流</param>
    /// <returns>是否成功</returns>
    public Boolean Read(Stream stream)
    {
        var reader = new Binary { Stream = stream, IsLittleEndian = false };

        Begin = reader.ReadUInt16();
        if (Begin != BeginFlag) return false;

        CommandId = reader.ReadByte();
        DataLength = reader.ReadUInt16();
        KeepFields = reader.ReadByte();

        if (DataLength > 0)
        {
            // 按声明长度截取数据块，保证校验字位置正确；未知命令字仅跳过数据体
            var data = reader.ReadBytes(DataLength);
            var body = CreateBody(CommandId);
            if (body != null)
            {
                body.Read(new Binary { Stream = new MemoryStream(data), IsLittleEndian = false });
                Bodies = body;
            }
        }

        CheckCode = reader.ReadByte();
        return true;
    }

    /// <summary>写入到数据流</summary>
    /// <param name="stream">数据流</param>
    public void Write(Stream stream)
    {
        var writer = new Binary { Stream = stream, IsLittleEndian = false };

        var startPos = stream.Position;

        writer.Write(Begin);
        writer.Write(CommandId);

        var lenPos = stream.Position;
        writer.Write((UInt16)0);
        writer.Write(KeepFields);

        if (Bodies != null)
        {
            Bodies.Write(writer);

            var bodyLen = (UInt16)(stream.Position - lenPos - 2 - 1);
            writer.Stream.Position = lenPos;
            writer.Write(bodyLen);
            writer.Stream.Position = stream.Length;
        }

        var checkSum = GetCheckSum(stream, (Int32)startPos);
        writer.Write(checkSum);
    }

    private static Byte GetCheckSum(Stream stream, Int32 offset)
    {
        var pos = stream.Position;
        stream.Position = offset;

        var crc = (Byte)stream.ReadByte();
        for (var i = 1; i < pos - offset; i++)
            crc = (Byte)(crc ^ (Byte)stream.ReadByte());

        return crc;
    }

    /// <summary>根据命令字创建对应的下行数据体</summary>
    private static DownBody? CreateBody(Byte commandId)
    {
        return null;
    }
}
