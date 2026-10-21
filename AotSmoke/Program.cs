using System.Globalization;
using System.Reflection;
using NewLife;
using NewLife.Data;
using NewLife.JT808.Models;
using NewLife.JT808.Protocols;
using NewLife.Log;
using NewLife.Serialization;
using NewLife.Reflection;

var failed = 0;
var passed = 0;

void Run(String name, Action action)
{
    try
    {
        action();
        Console.WriteLine($"  OK   {name}");
        passed++;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"  FAIL {name}: {ex.GetType().Name} {ex.Message}");
        failed++;
    }
}

static JTMessage Roundtrip(JTMessage message)
{
    var pk = message.ToPacket() ?? throw new InvalidOperationException("ToPacket returned null");
    var reply = new JTMessage();
    if (!reply.Read(pk)) throw new InvalidOperationException("Read returned false");
    return reply;
}

Console.WriteLine("==== NewLife.JT808 NativeAOT 冒烟 ====");

Run("MessageFactory 注册全部消息体", () =>
{
    InitFactory();
    var count = MessageFactory.Instance.Maps.Count;
    if (count < 80) throw new InvalidOperationException($"消息体数量不足：{count}");
});

Run("心跳帧编码解码", () =>
{
    var msg = new JTMessage { Kind = MessageKinds.终端心跳, Mobile = "13800138000", Sequence = 1 };
    var reply = Roundtrip(msg);
    if (reply.Kind != MessageKinds.终端心跳) throw new InvalidOperationException("消息ID错误");
    if (reply.Mobile != "13800138000") throw new InvalidOperationException("手机号错误");
});

Run("2019 版本帧头（10位手机号）", () =>
{
    var msg = new JTMessage { Kind = MessageKinds.终端心跳, Mobile = "13800138000", Sequence = 2, Version = 1 };
    var reply = Roundtrip(msg);
    if (reply.Version != 1) throw new InvalidOperationException("版本错误");
    if (reply.Mobile != "13800138000") throw new InvalidOperationException("手机号错误");
});

Run("分包字段往返", () =>
{
    var msg = new JTMessage
    {
        Kind = MessageKinds.位置信息汇报,
        Mobile = "13800138000",
        Sequence = 3,
        Property = PropertyFlags.Package,
        PackageCount = 3,
        PackageIndex = 2,
        Payload = new ArrayPacket(new Byte[] { 1, 2 }),
    };
    var reply = Roundtrip(msg);
    if (reply.PackageCount != 3 || reply.PackageIndex != 2) throw new InvalidOperationException("分包字段错误");
});

Run("转义字节往返", () =>
{
    var payload = new Byte[] { 0x7D, 0x7E, 0x7D, 0x01, 0x7D, 0x02, 0x00, 0xFF };
    var msg = new JTMessage { Kind = MessageKinds.数据上行透传, Mobile = "13800138000", Sequence = 4, Payload = new ArrayPacket(payload) };
    var reply = Roundtrip(msg);
    if (!reply.Payload.ToArray().SequenceEqual(payload)) throw new InvalidOperationException("负载不一致");
});

Run("位置信息汇报往返", () =>
{
    var body = new T0200
    {
        Alarm = PositionAlarms.超速报警,
        Status = PositionStatus.ACC,
        Latitude = 22334455,
        Longitude = 114112233,
        Altitude = 50,
        Speed = 80,
        Direction = 270,
        GPSTime = new DateTime(2024, 1, 2, 3, 4, 5),
    };
    var msg = new JTMessage { Kind = MessageKinds.位置信息汇报, Mobile = "13800138000", Sequence = 5 };
    msg.WriteModel(body);

    var reply = Roundtrip(msg);
    var model = reply.ReadModel<T0200>();
    if (model.Latitude != 22334455 || model.Longitude != 114112233) throw new InvalidOperationException("经纬度错误");
    if (model.GPSTime != new DateTime(2024, 1, 2, 3, 4, 5)) throw new InvalidOperationException($"时间错误：{model.GPSTime}");
});

Run("多媒体数据上传往返（嵌套位置信息）", () =>
{
    var body = new T0801
    {
        MultimediaId = 100,
        MultimediaType = 0,
        MultimediaCodingFormat = 0,
        EventItemCoding = 0,
        ChannelId = 1,
        Position = new T0200 { Latitude = 22334455, Longitude = 114112233, GPSTime = new DateTime(2024, 1, 2, 3, 4, 5) },
        MultimediaDataPackage = new Byte[] { 1, 2, 3, 4, 5 },
    };
    var msg = new JTMessage { Kind = MessageKinds.多媒体数据上传, Mobile = "13800138000", Sequence = 6 };
    msg.WriteModel(body);

    var reply = Roundtrip(msg);
    var model = reply.ReadModel<T0801>();
    if (model.MultimediaId != 100) throw new InvalidOperationException("多媒体ID错误");
    if (model.MultimediaDataPackage?.Length != 5) throw new InvalidOperationException("数据包长度错误");
});

Run("设置电话本往返（联系人列表）", () =>
{
    var body = new T8401
    {
        SettingTelephoneBook = SetTelephoneBook.追加电话本,
        JT808ContactProperties =
        [
            new ContactProperty { Type = ContactType.呼入, PhoneNumber = "13800138000", Contact = "张三" },
            new ContactProperty { Type = ContactType.呼出, PhoneNumber = "13900139000", Contact = "李四" },
        ],
    };
    var msg = new JTMessage { Kind = MessageKinds.设置电话本, Mobile = "13800138000", Sequence = 7 };
    msg.WriteModel(body);

    var reply = Roundtrip(msg);
    var model = reply.ReadModel<T8401>();
    if (model.ContactCount != 2) throw new InvalidOperationException($"联系人数量错误：{model.ContactCount}");
    if (model.JT808ContactProperties[1].Contact != "李四") throw new InvalidOperationException("中文联系人错误");
});

Run("全部消息体写读往返", () =>
{
    var failures = new List<String>();
    foreach (var item in MessageFactory.Instance.Maps)
    {
        var model = item.Value.CreateInstance();
        if (model == null)
        {
            failures.Add($"{item.Key}: 创建失败");
            continue;
        }

        FillModel(model, item.Value);

        try
        {
            var msg = new JTMessage { Kind = item.Key, Mobile = "13800138000", Sequence = 8 };
            msg.WriteModel(model);

            var reply = Roundtrip(msg);
            if (reply.GetBody() == null) failures.Add($"{item.Key}: 解析为空");
        }
        catch (Exception ex)
        {
            failures.Add($"{item.Key}: {ex.Message}");
        }
    }
    if (failures.Count > 0) throw new InvalidOperationException($"失败 {failures.Count} 项：{String.Join("; ", failures)}");
});

Run("TLV 写读往返", () =>
{
    var list = TLV.Convert(new Dictionary<UInt32, Object>
    {
        [0x0001] = (UInt32)30,
        [0x0002] = (UInt16)60,
        [0x0003] = new Byte[] { 1, 2, 3 },
    });
    using var ms = new MemoryStream();
    TLV.Write(ms, list);
    ms.Position = 0;

    var dic = TLV.Convert<UInt32>(TLV.Read(ms));
    if (dic.Count != 3) throw new InvalidOperationException($"条目数量错误：{dic.Count}");
    if ((UInt32)dic[0x0001] != 30) throw new InvalidOperationException("TLV 数值错误");
});

Run("KLV 写读往返", () =>
{
    var list = KLV.Convert(new Dictionary<Byte, Object>
    {
        [0x01] = (Byte)1,
        [0x02] = (UInt32)2,
        [0x03] = new Byte[] { 9, 9 },
    });
    using var ms = new MemoryStream();
    KLV.Write(ms, list);
    ms.Position = 0;

    var dic = KLV.Convert<Byte>(KLV.Read(ms));
    if (dic.Count != 3) throw new InvalidOperationException($"条目数量错误：{dic.Count}");
});

Run("多语言 11 种语言自有词条可用", () =>
{
    var saved = Localization.Culture;
    try
    {
        foreach (var name in new[] { "zh-Hans", "zh-Hant", "en", "es", "de", "ja", "fr", "pt", "ru", "ko", "ar" })
        {
            var culture = CultureInfo.GetCultureInfo(name);
            if (!Localization.IsSupported(culture)) throw new InvalidOperationException($"缺少语言：{name}");

            // Core 自带全部语言，必须用本库自有词条验证词表已注册
            Localization.Culture = culture;
            var text = Localization.Translate("Heartbeat => {0}", 1);
            if (name == "en" ? text != "Heartbeat => 1" : text == "Heartbeat => 1")
                throw new InvalidOperationException($"词表未生效：{name} => {text}");
        }
    }
    finally
    {
        Localization.Culture = saved;
    }
});

Run("多语言 程序集元数据与日志翻译", () =>
{
    var saved = Localization.Culture;
    var savedLog = Localization.LogCulture;
    try
    {
        var title = typeof(JTMessage).Assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title ?? "";
        Localization.Culture = CultureInfo.GetCultureInfo("zh-Hans");
        if (Localization.Translate(title) != "JT/T 808 协议库") throw new InvalidOperationException($"标题未翻译：{title}");

        Localization.LogCulture = CultureInfo.GetCultureInfo("zh-Hans");
        var log = Logger.FormatMessage(null, "Heartbeat => {0}", [7]);
        if (log != "心跳 => 7") throw new InvalidOperationException($"日志未翻译：{log}");
    }
    finally
    {
        Localization.Culture = saved;
        Localization.LogCulture = savedLog;
    }
});

Run("GBK 读写", () =>
{
    using var ms = new MemoryStream();
    var writer = Helper.CreateWriter(ms);
    Helper.WriteAllGBK(writer, "中文GBK");
    ms.Position = 0;
    var text = Helper.CreateReader(ms).ReadAllGBK();
    if (text != "中文GBK") throw new InvalidOperationException($"GBK 不一致：{text}");
});

Run("JSON 序列化消息体", () =>
{
    var json = new T8001 { Sequence = 1, Kind = MessageKinds.终端心跳, Result = ResultKinds.Success }.ToJsonJT808();
    if (json.IsNullOrEmpty() || !json.Contains("Sequence")) throw new InvalidOperationException($"JSON 异常：{json}");
});

Run("消息处理器反射注册", () =>
{
    var server = new JT808Server();
    server.AddHandler(new SmokeHandler());
    if (!server.Handlers.ContainsKey(MessageKinds.终端心跳)) throw new InvalidOperationException("处理器未注册");
});

Console.WriteLine($"==== 通过 {passed}，失败 {failed} ====");
return failed == 0 ? 0 : 1;

/// <summary>初始化消息工厂。AOT 冒烟工程已通过 TrimmerRootAssembly 保留协议库类型</summary>
[System.Diagnostics.CodeAnalysis.UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "AOT 冒烟工程已通过 TrimmerRootAssembly 保留 NewLife.JT808 全部类型")]
static void InitFactory() => MessageFactory.Instance.Init();

/// <summary>填充基础默认值，避免空引用与零长度字段影响编解码</summary>
/// <param name="model">模型</param>
/// <param name="type">类型</param>
[System.Diagnostics.CodeAnalysis.UnconditionalSuppressMessage("Trimming", "IL2070", Justification = "冒烟工程通过 TrimmerRootAssembly 保留 NewLife.JT808 全部类型")]
static void FillModel(Object model, Type type)
{
    foreach (var property in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
    {
        if (!property.CanWrite) continue;

        var propertyType = property.PropertyType;
        if (propertyType == typeof(String))
            property.SetValue(model, "1234567890");
        else if (propertyType == typeof(DateTime))
            property.SetValue(model, new DateTime(2024, 1, 2, 3, 4, 5));
        else if (propertyType == typeof(Byte[]))
            property.SetValue(model, new Byte[] { 0x01, 0x02, 0x03, 0x04 });
    }
}

/// <summary>冒烟处理器，验证反射扫描方法特性</summary>
public class SmokeHandler
{
    /// <summary>心跳处理</summary>
    /// <param name="session">会话</param>
    /// <param name="message">消息</param>
    /// <returns></returns>
    [MessageKind(MessageKinds.终端心跳)]
    public Object OnHeartbeat(JT808Session session, JTMessage message) => null;
}
