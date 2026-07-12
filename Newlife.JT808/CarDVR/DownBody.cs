using NewLife.Serialization;

namespace NewLife.JT808.CarDVR;

/// <summary>行车记录仪下行数据体基类</summary>
public abstract class DownBody
{
    /// <summary>命令字</summary>
    public abstract Byte CommandId { get; }

    /// <summary>从数据流中读取</summary>
    /// <param name="reader"></param>
    public abstract void Read(Binary reader);

    /// <summary>写入到数据流</summary>
    /// <param name="writer"></param>
    public abstract void Write(Binary writer);
}
