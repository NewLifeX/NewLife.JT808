using System.Reflection;
using System.Runtime.Serialization;
using NewLife.Buffers;
using NewLife.Collections;
using NewLife.Data;
using NewLife.JT808.Protocols;
using NewLife.Log;
using NewLife.Messaging;
using NewLife.Serialization;

namespace NewLife.JT808.Protocols;

/// <summary>JT808报文消息</summary>
public class JTMessage : IAccessor
{
    #region 属性
    /// <summary>消息编号</summary>
    public MessageKinds Kind { get; set; }

    /// <summary>消息体属性</summary>
    public PropertyFlags Property { get; set; }

    /// <summary>协议版本号。2019版</summary>
    public byte Version { get; set; }

    /// <summary>终端手机号</summary>
    public string Mobile { get; set; }

    /// <summary>消息流水号</summary>
    /// <remarks>发送信息的序列号，用于接收方检测是否有信息的丢失，上级平台和下级平台接自己发送数据包的个数计数，互不影响。</remarks>
    public ushort Sequence { get; set; }

    /// <summary>消息总包数</summary>
    public ushort PackageCount { get; set; }

    /// <summary>包序号。从1开始</summary>
    public ushort PackageIndex { get; set; }
    #endregion

    #region 接口属性
    /// <summary>是否响应</summary>
    /// <remarks>所有终端主发的消息均应要求平台应答</remarks>
    public bool Reply { get; set; }

    /// <summary>负载数据</summary>
    [IgnoreDataMember]
    public IPacket? Payload { get; set; }
    #endregion

    #region 构造
    #endregion

    #region 读写
    /// <summary>解析数据流</summary>
    /// <param name="stream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public bool Read(Stream stream, object context)
    {
        if (stream == null && context == null) return false;

        // 头部标识探测
        if (stream.ReadByte() != 0x7E) return false;

        // 转义处理，0x7d->0x7d01，0x7e->0x7d02
        var ms = new MemoryStream();
        while (true)
        {
            var b = stream.ReadByte();
            if (b < 0) break;

            if (b == 0x7D)
            {
                var b2 = stream.ReadByte();
                if (b2 < 0)
                {
                    ms.WriteByte((byte)b);
                    break;
                }

                if (b2 == 0x01)
                    ms.Write(0x7D);
                else if (b2 == 0x02)
                    ms.Write(0x7E);
                else
                    ms.Write((byte)b, (byte)b2);
            }
            else
                ms.WriteByte((byte)b);
        }
        ms.Position = 0;

        var reader = new Binary { Stream = ms, IsLittleEndian = false };

        Kind = (MessageKinds)reader.Read<ushort>();
        var flag = reader.Read<ushort>();
        //Property = (PropertyFlags)flag;
        Property = (PropertyFlags)(flag & ~0x3FF);

        // 2019版多了协议版本号
        if (Property.Has(PropertyFlags.Version)) Version = reader.ReadByte();

        if (Version > 0)
            Mobile = reader.ReadBCD(10).TrimStart('0');
        else
            Mobile = reader.ReadBCD(6).TrimStart('0');

        Sequence = reader.Read<ushort>();

        // 分包
        if (Property.Has(PropertyFlags.Package))
        {
            PackageCount = reader.Read<ushort>();
            PackageIndex = reader.Read<ushort>();
        }

        // 负载数据
        var length = flag & 0x3FF;
        if (length > 0)
        {
            var retain = ms.Length - ms.Position;
            if (length > retain)
            {
                Payload = (ArrayPacket)reader.ReadBytes((int)retain);
                return false;
            }

            Payload = (ArrayPacket)reader.ReadBytes(length);
        }

        // 异或校验。从消息头首字节开始。考虑到TCP/UDP本身有校验，这里不再判断
        var checkSum2 = GetCheckSum(ms, 0);
        var checkSum = reader.ReadByte();
        //if (checkSum != checkSum2) return false;
        if (checkSum != checkSum2) XTrace.WriteLine(L.T("Checksum error {0:X2}!={1:X2}", checkSum, checkSum2));

        Reply = MessageFactory.IsReply(Kind);

        // 尾部标识探测
        return reader.ReadByte() == 0x7E;
    }

    /// <summary>写入数据</summary>
    /// <param name="stream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public bool Write(Stream stream, object context)
    {
        if (stream == null && context == null) return false;

        // 计算属性
        var flag = (ushort)Property;
        var pk = Payload;
        var length = pk?.Total ?? 0;
        if (length > 0x3FF) throw new InvalidOperationException(L.T("The message body length {0} exceeds the 1023-byte limit; split it into sub-packets.", length));
        flag = (ushort)(flag | length & 0x3FF);
        if (Version > 0) flag |= 1 << 14;

        // 使用内存流，后面可能需要转义
        var ms = Pool.MemoryStream.Get();
        var writer = new Binary { Stream = ms, IsLittleEndian = false };

        //writer.Write(0x7E);

        writer.Write((ushort)Kind);
        writer.Write(flag);

        // 2019版多了协议版本号
        if (Version > 0)
        {
            writer.Write(Version);
            var str = Mobile ?? String.Empty;
            if (str.Length < 10 * 2) str = str.PadLeft(10 * 2, '0');
            writer.WriteBCD(str, 10);
        }
        // 2013版本
        else
        {
            var str = Mobile ?? String.Empty;
            if (str.Length < 6 * 2) str = str.PadLeft(6 * 2, '0');
            writer.WriteBCD(str, 6);
        }

        writer.Write(Sequence);

        if (Property.Has(PropertyFlags.Package))
        {
            writer.Write(PackageCount);
            writer.Write(PackageIndex);
        }

        if (length > 0) pk.CopyTo(writer.Stream);

        // 计算校验和
        var checkSum = GetCheckSum(ms, 0);
        writer.Write(checkSum);

        //writer.Write(0x7E);

        // 转义处理，0x7d->0x7d01，0x7e->0x7d02
        var ms2 = new MemoryStream();
        ms2.WriteByte(0x7E);

        ms.Position = 0;
        while (ms.Position < ms.Length)
        {
            var b = ms.ReadByte();
            if (b == 0x7D)
                ms2.Write(0x7D, 0x01);
            else if (b == 0x7E)
                ms2.Write(0x7D, 0x02);
            else
                ms2.WriteByte((byte)b);
        }
        ms.Put(false);
        ms2.WriteByte(0x7E);

        // 一次性写入结果
        ms2.Position = 0;
        ms2.CopyTo(stream);

        return true;
    }

    private static byte GetCheckSum(Stream stream, long offset)
    {
        var count = stream.Position - offset;
        stream.Position = offset;
        var crc = (byte)stream.ReadByte();
        for (var i = 1; i < count; i++)
        {
            var b = (byte)stream.ReadByte();
            crc = (byte)(crc ^ b);
        }
        return crc;
    }

    /// <summary>读取数据包</summary>
    /// <param name="pk"></param>
    /// <returns></returns>
    public Boolean Read(IPacket pk) => Read(pk.GetStream(), null);

    /// <summary>转数据包</summary>
    /// <returns></returns>
    public IPacket ToPacket()
    {
        try
        {
            var ms = new MemoryStream();
            if (!Write(ms, null)) return null;

            ms.Position = 0;
            return new ArrayPacket(ms);
        }
        catch (Exception ex)
        {
            XTrace.WriteException(ex);
            throw;
        }
    }
    #endregion

    #region 分包
    private IPacket?[]? _packets;
    /// <summary>附加分包，并返回是否完整</summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public bool Append(JTMessage message)
    {
        // 包序号从1开始，合法范围 1..PackageCount
        if (message == null || message.PackageCount <= 0) return false;
        if (message.PackageIndex < 1 || message.PackageIndex > message.PackageCount) return false;

        // 放入分包数组，注意要深度拷贝，否则可能存在网络缓冲区冲突，导致数据被修改
        _packets ??= new IPacket[message.PackageCount];
        if (message.PackageIndex > _packets.Length) return false;

        // 首包自身也要进入集合，分包可能乱序到达
        if (PackageIndex >= 1 && PackageIndex <= _packets.Length && Payload != null)
            _packets[PackageIndex - 1] ??= Payload.Clone();

        if (message.Payload != null)
            _packets[message.PackageIndex - 1] = message.Payload.Clone();

        return _packets.All(e => e != null);
    }
    #endregion

    #region 解析负载
    /// <summary>从负载数据解析业务消息模型</summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public bool TryReadModel(object model)
    {
        // 考虑分包
        var pk = Payload;
        if (_packets != null && _packets.Length > 0)
        {
            // 构建链式
            var pk2 = pk = _packets[0];
            for (var i = 1; i < _packets.Length; i++)
            {
                pk2.Append(_packets[i]);
                pk2 = _packets[i];
            }
        }
        if (pk == null || pk.Total <= 0) return true;

        if (model is IAccessor accessor)
            return accessor.Read(pk.GetStream(), this);
        else
            return Helper.CreateReader(pk.GetStream(), this).TryRead(null, ref model);
    }

    /// <summary>从负载数据解析业务消息模型</summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidDataException"></exception>
    public T ReadModel<T>() where T : new()
    {
        var msg = new T();
        if (!TryReadModel(msg)) throw new InvalidDataException(L.T("Failed to parse the message body."));

        return msg;
    }

    /// <summary>取得主体模型</summary>
    /// <returns></returns>
    /// <exception cref="InvalidDataException"></exception>
    public object GetBody()
    {
        var msg = MessageFactory.Instance.Create(Kind);
        if (!TryReadModel(msg)) return null;

        return msg;
    }

    /// <summary>写入业务消息模型到负载数据</summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public bool WriteModel(object model)
    {
        var att = model.GetType().GetCustomAttribute<MessageKindAttribute>();
        if (att != null)
            Kind = att.Kind;
        else if (MessageFactory.Instance.Types.TryGetValue(model.GetType(), out var kind))
            Kind = kind;

        var rs = false;
        var ms = new MemoryStream();
        if (model is IAccessor accessor)
            rs = accessor.Write(ms, this);
        else
            rs = Helper.CreateWriter(ms, this).Write(model);

        ms.Position = 0;
        Payload = new ArrayPacket(ms);

        return rs;
    }

    /// <summary>设置结果</summary>
    /// <param name="result"></param>
    public void SetResult(object result)
    {
        Reply = true;

        WriteModel(result);
    }
    #endregion

    #region 辅助
    /// <summary>创建响应</summary>
    /// <returns></returns>
    public JTMessage CreateReply() => new() { Reply = true, Kind = Kind, Mobile = Mobile, Version = Version };

    /// <summary>获取数据包长度</summary>
    /// <param name="pk">以 0x7E 开头的原始帧数据</param>
    /// <returns>完整帧长度；半包或非法帧返回 0</returns>
    /// <remarks>按原始字节扫描帧尾，跳过 0x7D 转义对，避免消息体包含转义字节时长度计算偏小。</remarks>
    public static int GetLength(IPacket pk)
    {
        if (pk == null || pk.Total < 2 || pk[0] != 0x7E) return 0;

        for (var i = 1; i < pk.Total; i++)
        {
            var b = pk[i];
            if (b == 0x7D)
            {
                // 转义对必须成对出现，末字节孤立表示半包
                if (i + 1 >= pk.Total) return 0;
                i++;
                continue;
            }

            // 第一个未转义的 0x7E 即帧尾
            if (b == 0x7E) return i + 1;
        }

        return 0;
    }

    /// <summary>已重写。返回友好文本</summary>
    /// <returns></returns>
    public override string ToString() => $"[{Kind}] {Mobile} Seq={Sequence} [{Payload?.Total}]";
    #endregion
}