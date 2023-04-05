using Newlife.JT808.Protocols;
using NewLife.Serialization;

namespace Newlife.JT808.Models;

/// <summary>
/// 终端注册返回结果
/// </summary>
public enum RegisterResult : Byte
{
    /// <summary>
    /// 成功
    /// </summary>
    成功 = 0x00,
    /// <summary>
    /// 车辆已被注册
    /// </summary>
    车辆已被注册 = 0x01,
    /// <summary>
    /// 数据库中无该车辆
    /// </summary>
    数据库中无该车辆 = 0x02,
    /// <summary>
    /// 终端已被注册
    /// </summary>
    终端已被注册 = 0x03,
    /// <summary>
    /// 数据库中无该终端
    /// </summary>
    数据库中无该终端 = 0x04
}

/// <summary>
/// 终端注册应答
/// </summary>
[MessageKind(MessageKinds.终端注册应答)]
public class T8100 //: IAccessor
{
    /// <summary>
    /// 应答流水号
    /// 对应的终端注册消息的流水号
    /// </summary>
    public UInt16 Sequence { get; set; }

    /// <summary>
    /// 结果
    /// </summary>
    public RegisterResult Result { get; set; }

    /// <summary>
    /// 鉴权码
    /// 只有在成功后才有该字段
    /// </summary>
    [FullString]
    public String Code { get; set; }

    ///// <summary>解析消息</summary>
    ///// <param name="stream"></param>
    ///// <param name="context"></param>
    ///// <returns></returns>
    //public Boolean Read(Stream stream, Object context)
    //{
    //    var reader = new BinaryReader(stream);

    //    Sequence = reader.ReadUInt16();
    //    Result = (RegisterResult)reader.ReadByte();

    //    // 只有在成功后才有该字段
    //    if (Result == RegisterResult.成功) Code = stream.ToStr();

    //    return true;
    //}

    ///// <summary>序列化</summary>
    ///// <param name="stream"></param>
    ///// <param name="context"></param>
    ///// <returns></returns>
    //public Boolean Write(Stream stream, Object context)
    //{
    //    var writer = new BinaryWriter(stream);

    //    writer.Write(Sequence);
    //    writer.Write((Byte)Result);

    //    // 只有在成功后才有该字段
    //    if (Result == RegisterResult.成功)
    //    {
    //        var buf = Code.GetBytes();
    //        writer.Write(buf, 0, buf.Length);
    //    }

    //    return true;
    //}
}
