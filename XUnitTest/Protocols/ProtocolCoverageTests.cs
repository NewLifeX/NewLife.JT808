using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using NewLife;
using NewLife.Data;
using NewLife.JT808.Protocols;
using NewLife.Reflection;

namespace XUnitTest.Protocols;

/// <summary>协议层全覆盖测试</summary>
/// <remarks>
/// 基础协议层是整库根基：帧结构、消息工厂、全部消息体往返、TLV/KLV 与多语言。
/// 本类以数据驱动方式遍历程序集内所有 <see cref="MessageKindAttribute"/> 类型，
/// 确保新增消息体自动进入回归范围。
/// </remarks>
public class ProtocolCoverageTests
{
    private static readonly MessageFactory _factory = CreateFactory();

    private static MessageFactory CreateFactory()
    {
        var factory = MessageFactory.Instance;
        factory.Init();

        return factory;
    }

    #region 全部消息体
    [Fact]
    [DisplayName("全部消息体_类型注册完整")]
    public void AllMessageBodies_Registered()
    {
        var types = typeof(MessageKinds).Assembly.GetTypes()
            .Where(e => e.GetCustomAttribute<MessageKindAttribute>() != null)
            .ToArray();

        Assert.True(types.Length >= 80, $"消息体类型数量不足：{types.Length}");

        foreach (var type in types)
        {
            var att = type.GetCustomAttribute<MessageKindAttribute>();
            Assert.True(_factory.Maps.ContainsKey(att.Kind), $"未注册消息类型：{att.Kind} => {type.Name}");
        }
    }

    [Fact]
    [DisplayName("全部消息体_写读往返一致")]
    public void AllMessageBodies_Roundtrip()
    {
        var failures = new List<String>();
        var tested = 0;

        foreach (var pair in _factory.Maps.OrderBy(e => e.Key))
        {
            var model = CreateModel(pair.Value);
            if (model == null)
            {
                failures.Add($"{pair.Key}: 无法创建 {pair.Value.Name}");
                continue;
            }

            try
            {
                var msg = new JTMessage
                {
                    Kind = pair.Key,
                    Mobile = "13800138000",
                    Sequence = 7,
                    Version = pair.Key == MessageKinds.终端注册 ? (Byte)1 : (Byte)0,
                };

                var written = msg.WriteModel(model);
                Assert.True(written, $"{pair.Key}: 写入失败");

                var payload = msg.Payload?.ToArray() ?? [];
                var msg2 = new JTMessage
                {
                    Kind = pair.Key,
                    Mobile = msg.Mobile,
                    Sequence = msg.Sequence,
                    Version = msg.Version,
                    Payload = new ArrayPacket(payload),
                };

                var parsed = msg2.GetBody();
                Assert.NotNull(parsed);
                Assert.Equal(pair.Value, parsed.GetType());

                // 读回的模型应能稳定重写同样的消息体字节
                var msg3 = new JTMessage { Kind = pair.Key, Mobile = msg.Mobile, Sequence = msg.Sequence, Version = msg.Version };
                msg3.WriteModel(parsed);
                var payload3 = msg3.Payload?.ToArray() ?? [];
                if (!payload.SequenceEqual(payload3))
                    failures.Add($"{pair.Key}: 二次写出字节不一致");
            }
            catch (Exception ex)
            {
                failures.Add($"{pair.Key}: {pair.Value.Name} 异常 {ex.Message}");
            }

            tested++;
        }

        Assert.True(tested >= 80, $"参与往返的消息体不足：{tested}");
        Assert.True(failures.Count == 0, $"消息体往返失败 {failures.Count} 项：{Environment.NewLine}{String.Join(Environment.NewLine, failures)}");
    }

    /// <summary>创建带有合法默认值的消息体实例</summary>
    /// <param name="type">消息体类型</param>
    /// <returns>实例</returns>
    private static Object CreateModel(Type type)
    {
        var model = type.CreateInstance();
        if (model == null) return null;

        FillModel(model, type);

        return model;
    }

    /// <summary>填充标量默认值，并为嵌套模型与集合创建实例</summary>
    /// <param name="model">实例</param>
    /// <param name="type">类型</param>
    private static void FillModel(Object model, Type type)
    {
        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!property.CanWrite) continue;

            var propertyType = property.PropertyType;
            if (propertyType == typeof(String))
                property.SetValue(model, "1234567890");
            else if (propertyType == typeof(DateTime))
                property.SetValue(model, new DateTime(2024, 1, 2, 3, 4, 5));
            else if (propertyType == typeof(Byte[]))
                property.SetValue(model, new Byte[] { 0x01, 0x02, 0x03, 0x04 });
            else if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                property.SetValue(model, Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(propertyType.GetGenericArguments())));
            else if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(IList<>))
                property.SetValue(model, Activator.CreateInstance(typeof(List<>).MakeGenericType(propertyType.GetGenericArguments())));
            else if (propertyType.IsArray && propertyType != typeof(Byte[]))
                property.SetValue(model, Array.CreateInstance(propertyType.GetElementType(), 0));
            else if (propertyType.IsClass && propertyType != typeof(String) && propertyType.Namespace?.StartsWith("NewLife.JT808") == true)
            {
                var nested = property.GetValue(model) ?? propertyType.CreateInstance();
                if (nested != null)
                {
                    FillModel(nested, propertyType);
                    property.SetValue(model, nested);
                }
            }
        }
    }
    #endregion

    #region 帧结构
    [Theory]
    [InlineData((Byte)0)]
    [InlineData((Byte)1)]
    [DisplayName("帧头_版本与手机号长度")]
    public void Frame_Header_Versions(Byte version)
    {
        var msg = new JTMessage
        {
            Kind = MessageKinds.终端心跳,
            Mobile = "13800138000",
            Sequence = 0x1234,
            Version = version,
        };

        var pk = msg.ToPacket();
        Assert.NotNull(pk);

        // 0x7E + 消息ID(2) + 属性(2) + [版本1] + 手机号[6|10] + 流水号(2) + 校验(1) + 0x7E
        var expected = version > 0 ? 1 + 2 + 2 + 1 + 10 + 2 + 1 + 1 : 1 + 2 + 2 + 6 + 2 + 1 + 1;
        Assert.Equal(expected, pk.Total);

        var msg2 = new JTMessage();
        Assert.True(msg2.Read(pk));
        Assert.Equal(version, msg2.Version);
        Assert.Equal("13800138000", msg2.Mobile);
        Assert.Equal((UInt16)0x1234, msg2.Sequence);
        Assert.Equal(MessageKinds.终端心跳, msg2.Kind);
    }

    [Fact]
    [DisplayName("帧头_分包字段往返")]
    public void Frame_SubPackage_Roundtrip()
    {
        var msg = new JTMessage
        {
            Kind = MessageKinds.位置信息汇报,
            Mobile = "13800138000",
            Sequence = 9,
            Property = PropertyFlags.Package,
            PackageCount = 5,
            PackageIndex = 2,
            Payload = new ArrayPacket(new Byte[] { 0x11, 0x22 }),
        };

        var pk = msg.ToPacket();
        Assert.NotNull(pk);

        var msg2 = new JTMessage();
        Assert.True(msg2.Read(pk));
        Assert.True(msg2.Property.Has(PropertyFlags.Package));
        Assert.Equal((UInt16)5, msg2.PackageCount);
        Assert.Equal((UInt16)2, msg2.PackageIndex);
        Assert.Equal(new Byte[] { 0x11, 0x22 }, msg2.Payload.ToArray());
    }

    [Fact]
    [DisplayName("GetLength_识别完整帧长度")]
    public void GetLength_DetectsFrame()
    {
        var msg = new JTMessage
        {
            Kind = MessageKinds.平台通用应答,
            Mobile = "13800138000",
            Sequence = 1,
            Payload = new ArrayPacket(new Byte[] { 0x00, 0x01, 0x02, 0x00, 0x00 }),
        };

        var pk = msg.ToPacket();
        Assert.NotNull(pk);
        Assert.Equal(pk.Total, JTMessage.GetLength(pk));

        // 不足最小帧长
        Assert.Equal(0, JTMessage.GetLength(new ArrayPacket(new Byte[10])));
    }

    [Fact]
    [DisplayName("解码_帧间垃圾数据可跳过")]
    public void Decode_GarbageBetweenFrames()
    {
        var codec = new JTCodec();

        var msg1 = new JTMessage { Kind = MessageKinds.终端心跳, Mobile = "13800138000", Sequence = 1 };
        var msg2 = new JTMessage { Kind = MessageKinds.终端注销, Mobile = "13800138001", Sequence = 2 };

        var bytes = new Byte[] { 0x00, 0xFF, 0x55 }
            .Concat(msg1.ToPacket().ToArray())
            .Concat(new Byte[] { 0x01, 0x02 })
            .Concat(msg2.ToPacket().ToArray())
            .ToArray();

        var list = codec.Decode(new ArrayPacket(bytes));

        Assert.Equal(2, list.Count);
        Assert.Equal(MessageKinds.终端心跳, list[0].Kind);
        Assert.Equal(MessageKinds.终端注销, list[1].Kind);
    }

    [Fact]
    [DisplayName("解码_超长消息体转义往返")]
    public void Decode_LargePayload_Escaped()
    {
        var payload = new Byte[1000];
        for (var i = 0; i < payload.Length; i++)
        {
            // 覆盖 0x7D/0x7E 转义分支
            payload[i] = (Byte)(i % 3 == 0 ? 0x7D : i % 3 == 1 ? 0x7E : i & 0xFF);
        }

        var msg = new JTMessage
        {
            Kind = MessageKinds.数据上行透传,
            Mobile = "13800138000",
            Sequence = 42,
            Payload = new ArrayPacket(payload),
        };

        var pk = msg.ToPacket();
        Assert.NotNull(pk);
        // 转义会增大帧长
        Assert.True(pk.Total > payload.Length);

        var msg2 = new JTMessage();
        Assert.True(msg2.Read(pk));
        Assert.Equal(payload, msg2.Payload.ToArray());
    }
    #endregion

    #region 消息工厂
    [Fact]
    [DisplayName("消息工厂_创建与解析")]
    public void Factory_CreateAndParse()
    {
        var model = _factory.Create(MessageKinds.平台通用应答);
        Assert.IsType<NewLife.JT808.Models.T8001>(model);

        var unknown = _factory.Create((MessageKinds)0x7FFF);
        Assert.Null(unknown);

        var msg = new JTMessage { Kind = MessageKinds.平台通用应答, Mobile = "13800138000" };
        msg.WriteModel(new NewLife.JT808.Models.T8001 { Sequence = 3, Kind = MessageKinds.终端心跳, Result = NewLife.JT808.Models.ResultKinds.Failure });

        var parsed = _factory.Parse(msg);
        var t8001 = Assert.IsType<NewLife.JT808.Models.T8001>(parsed);
        Assert.Equal((UInt16)3, t8001.Sequence);
        Assert.Equal(NewLife.JT808.Models.ResultKinds.Failure, t8001.Result);
    }

    [Fact]
    [DisplayName("消息工厂_自定义注册")]
    public void Factory_RegisterCustom()
    {
        var factory = new MessageFactory();
        factory.Register(MessageKinds.查询服务器时间应答, typeof(CustomReply));

        Assert.Equal(typeof(CustomReply), factory.Maps[MessageKinds.查询服务器时间应答]);
        Assert.Equal(MessageKinds.查询服务器时间应答, factory.Types[typeof(CustomReply)]);

        factory.SetMap<CustomReply>();
        Assert.IsType<CustomReply>(factory.Create(MessageKinds.查询服务器时间应答));
    }

    [MessageKind(MessageKinds.查询服务器时间应答)]
    private class CustomReply { }

    [Fact]
    [DisplayName("消息工厂_响应判定")]
    public void Factory_IsReply()
    {
        Assert.True(MessageFactory.IsReply(MessageKinds.平台通用应答));
        Assert.True(MessageFactory.IsReply(MessageKinds.终端通用应答));
        Assert.True(MessageFactory.IsReply(MessageKinds.终端注册应答));
        Assert.True(MessageFactory.IsReply(MessageKinds.查询终端参数应答));
        Assert.False(MessageFactory.IsReply(MessageKinds.终端心跳));
        Assert.False(MessageFactory.IsReply(MessageKinds.位置信息汇报));
        Assert.False(MessageFactory.IsReply(MessageKinds.补传分包请求));

        // 0x8xxx 默认识别为下行应答
        Assert.True(MessageFactory.IsReply((MessageKinds)0x8501));
        Assert.False(MessageFactory.IsReply((MessageKinds)0x0501));
    }

    [Fact]
    [DisplayName("消息工厂_类型名")]
    public void Factory_GetKindName()
    {
        Assert.Equal("终端心跳", MessageFactory.GetKindName(MessageKinds.终端心跳));
        Assert.Equal("7FFF", MessageFactory.GetKindName((MessageKinds)0x7FFF));
    }
    #endregion

    #region 序列化特性
    [Fact]
    [DisplayName("BCD时间_往返")]
    public void BCDTime_Roundtrip()
    {
        var time = new DateTime(2026, 10, 20, 10, 35, 59);

        using var ms = new MemoryStream();
        var writer = Helper.CreateWriter(ms);
        writer.WriteBCD(time.ToString("yyMMddHHmmss"), 6);
        Assert.Equal(new Byte[] { 0x26, 0x10, 0x20, 0x10, 0x35, 0x59 }, ms.ToArray());

        ms.Position = 0;
        var reader = Helper.CreateReader(ms);
        Assert.Equal("261020103559", reader.ReadBCD(6));
    }

    [Fact]
    [DisplayName("GBK字符串_越界与空值")]
    public void GBK_EdgeCases()
    {
        // 空数据返回空字符串
        Assert.True(String.IsNullOrEmpty(Helper.CreateReader(new MemoryStream()).ReadAllGBK()));

        using var ms = new MemoryStream();
        var writer = Helper.CreateWriter(ms);
        Helper.WriteAllGBK(writer, "abc中");
        ms.Position = 0;
        Assert.Equal("abc中", Helper.CreateReader(ms).ReadAllGBK());
    }
    #endregion

    #region 多语言
    [Fact]
    [DisplayName("多语言_十一种语言已注册")]
    public void Localization_AllLanguagesRegistered()
    {
        var cultures = new[] { "zh-Hans", "zh-Hant", "ja", "ko", "de", "fr", "es", "pt", "ru", "ar", "en" };
        foreach (var name in cultures)
        {
            Assert.True(Localization.IsSupported(CultureInfo.GetCultureInfo(name)), $"缺少语言表：{name}");
        }

        // 区域变体按父级匹配
        Assert.True(Localization.IsSupported(CultureInfo.GetCultureInfo("zh-CN")));
        Assert.True(Localization.IsSupported(CultureInfo.GetCultureInfo("zh-TW")));
    }

    [Fact]
    [DisplayName("多语言_异常消息按语言翻译")]
    public void Localization_ExceptionMessageTranslated()
    {
        var saved = Localization.Culture;
        try
        {
            Localization.Culture = CultureInfo.GetCultureInfo("zh-Hans");
            var ex = Assert.Throws<Exception>(() => TLV.Convert(new Dictionary<UInt32, Object> { [1] = true }));
            Assert.Contains("不支持的类型", ex.Message);

            Localization.Culture = CultureInfo.GetCultureInfo("en");
            var ex2 = Assert.Throws<Exception>(() => TLV.Convert(new Dictionary<UInt32, Object> { [1] = true }));
            Assert.Contains("Unsupported type", ex2.Message);

            Localization.Culture = CultureInfo.GetCultureInfo("ja");
            var ex3 = Assert.Throws<Exception>(() => TLV.Convert(new Dictionary<UInt32, Object> { [1] = true }));
            Assert.Contains("未サポート", ex3.Message);
        }
        finally
        {
            Localization.Culture = saved;
        }
    }

    [Fact]
    [DisplayName("多语言_占位符格式化与英文回退")]
    public void Localization_FormatAndFallback()
    {
        var saved = Localization.Culture;
        try
        {
            Localization.Culture = CultureInfo.GetCultureInfo("zh-Hans");
            Assert.Equal("心跳 => 12", Localization.Translate("Heartbeat => {0}", 12));

            // 未收录的键回退英文原文
            Assert.Equal("Not translated {0}", Localization.Translate("Not translated {0}"));

            Localization.Culture = CultureInfo.GetCultureInfo("en");
            Assert.Equal("Heartbeat => 12", Localization.Translate("Heartbeat => {0}", 12));
        }
        finally
        {
            Localization.Culture = saved;
        }
    }

    [Fact]
    [DisplayName("多语言_AcceptLanguage解析")]
    public void Localization_AcceptLanguage()
    {
        var c1 = RequestCultureScope.ParseAcceptLanguage("en-US,en;q=0.9,zh-CN;q=0.8");
        Assert.NotNull(c1);
        Assert.Equal("en-US", c1.Name);

        var c2 = RequestCultureScope.ParseAcceptLanguage("xx-YY;q=1.0, ja;q=0.5");
        Assert.NotNull(c2);
        Assert.Equal("ja", c2.Name);

        Assert.Null(RequestCultureScope.ParseAcceptLanguage(null));
        Assert.Null(RequestCultureScope.ParseAcceptLanguage("*"));
    }
    #endregion

    #region TLV/KLV
    [Fact]
    [DisplayName("TLV_写入读回_数值类型")]
    public void TLV_Roundtrip()
    {
        var values = new Object[] { (Byte)1, (UInt16)2, (UInt32)3 };
        var expected = new Object[] { (Byte)1, (UInt16)2, 3u };

        for (var i = 0; i < values.Length; i++)
        {
            var dic = new Dictionary<UInt32, Object> { [0x0102] = values[i] };
            var list = TLV.Convert(dic);
            Assert.Single(list);

            using var ms = new MemoryStream();
            TLV.Write(ms, list);
            ms.Position = 0;

            var dic2 = TLV.Convert<UInt32>(TLV.Read(ms));
            Assert.Equal(expected[i], dic2[0x0102u]);
        }
    }

    [Fact]
    [DisplayName("TLV_不支持类型抛异常")]
    public void TLV_UnsupportedType()
    {
        var dic = new Dictionary<UInt32, Object> { [1] = 3.14 };
        Assert.Throws<Exception>(() => TLV.Convert(dic));
    }

    [Fact]
    [DisplayName("KLV_写入读回_数值类型")]
    public void KLV_Roundtrip()
    {
        var dic = new Dictionary<Byte, Object>
        {
            [0x01] = (Byte)0x7F,
            [0x02] = (UInt16)0x1234,
            [0x03] = (UInt32)0x12345678,
            [0x04] = new Byte[] { 1, 2, 3 },
        };

        var list = KLV.Convert(dic);
        Assert.Equal(4, list.Count);

        using var ms = new MemoryStream();
        KLV.Write(ms, list);
        ms.Position = 0;

        var dic2 = KLV.Convert<Byte>(KLV.Read(ms));
        Assert.Equal((Byte)0x7F, dic2[0x01]);
        Assert.Equal((UInt16)0x1234, dic2[0x02]);
        Assert.Equal(0x12345678u, dic2[0x03]);
        Assert.Equal(new Byte[] { 1, 2, 3 }, dic2[0x04]);
    }

    [Fact]
    [DisplayName("KLV_不支持类型抛异常")]
    public void KLV_UnsupportedType()
    {
        var dic = new Dictionary<Byte, Object> { [1] = 1.5 };
        Assert.Throws<Exception>(() => KLV.Convert(dic));
    }
    #endregion
}
