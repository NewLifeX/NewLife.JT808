using Newlife.JT808.Protocols;
using NewLife.Serialization;

namespace Newlife.JT808.Models;

/// <summary>
/// 终端控制
/// </summary>
[MessageKind(MessageKinds.终端控制)]
public class T8105 : IAccessor
{
    /// <summary>
    /// 标准命令参数数量
    /// </summary>
    public const Int32 CommandParameterCount = 13;
    private const Char CommandParameterSeparator = ';';
    private const Byte CommandParameterSeparatorValue = (Byte)';';

    /// <summary>
    /// 命令字
    /// </summary>
    public Byte CommandWord { get; set; }

    ///// <summary>
    ///// 命令参数集合
    ///// </summary>
    //public List<ICommandParameter> CommandParameters { get; set; }

    ///// <summary>
    ///// 自定义命令参数集合
    ///// </summary>
    //public List<ICommandParameter> CustomCommandParameters { get; set; }

    /// <summary>
    /// 未知的命令参数集合
    /// key:order
    /// value:data
    /// </summary>
    public Dictionary<Int32, Byte[]> UnknownCommandParameters { get; set; }

    /// <summary>
    /// 读取
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public Boolean Read(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var reader = new BinaryReader(stream);

        return true;
    }

    /// <summary>
    /// 写入
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public Boolean Write(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var writer = new BinaryWriter(stream);

        return true;
    }
}