# NewLife.JT808 - بروتوكول تتبع المركبات JT/T 808

![GitHub License](https://img.shields.io/github/license/NewLifeX/NewLife.JT808?logo=github)
![NuGet Downloads](https://img.shields.io/nuget/dt/NewLife.JT808?logo=nuget)
![NuGet](https://img.shields.io/nuget/v/NewLife.JT808?logo=nuget)
![NuGet prerelease](https://img.shields.io/nuget/vpre/NewLife.JT808?label=dev%20nuget&logo=nuget)

[![test](https://github.com/NewLifeX/NewLife.JT808/actions/workflows/test.yml/badge.svg?branch=master)](https://github.com/NewLifeX/NewLife.JT808/actions/workflows/test.yml)

<!-- metrics:start -->
[![AOT smoke](https://img.shields.io/badge/AOT%20smoke-16%2F16-brightgreen?logo=dotnet)](AotSmoke/Program.cs)
[![TFM](https://img.shields.io/badge/TFM-5%20%28net45%20%E2%86%92%20net8.0%29-512BD4?logo=dotnet)](Newlife.JT808/NewLife.JT808.csproj)
[![locales](https://img.shields.io/badge/locales-11-2ea44f?logo=dotnet)](Doc/多语言本地化.md)
<!-- metrics:end -->
![platform](https://img.shields.io/badge/platform-Windows%20%7C%20Linux%20%7C%20macOS-blue)
[![contributors](https://img.shields.io/github/contributors/NewLifeX/NewLife.JT808?logo=github)](https://github.com/NewLifeX/NewLife.JT808/graphs/contributors)

<!-- lang:start -->
🌐 [English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [Español](README.es.md) | [Deutsch](README.de.md) | [日本語](README.ja.md) | [Français](README.fr.md) | [Português](README.pt.md) | [Русский](README.ru.md) | [한국어](README.ko.md)
<!-- lang:end -->

> قد تكون هذه الترجمة متأخرة؛ راجع [English README](README.md).

تطبيق كامل لبروتوكول JT/T 808 لتتبع المركبات، يغطي إصدارات 2011/2013/2019 مع أكثر من 70 جسم رسالة قياسي، وامتدادات السلامة النشطة SuBiao (جيانغسو) وYueBiao (قوانغدونغ)، ورسائل الصوت والفيديو JT/T 1078، وبروتوكول مسجل القيادة JT/T 19056، والتكامل مع قوائم انتظار الرسائل.

المصدر: https://github.com/NewLifeX/NewLife.JT808  
NuGet: [NewLife.JT808](https://www.nuget.org/packages/NewLife.JT808)  
التوثيق: [المتطلبات](Doc/需求文档.md) / [البنية](Doc/架构设计.md) / [قائمة الميزات](Doc/功能清单.md) / [دليل الاستخدام](Doc/使用文档.md) / [التوطين](Doc/多语言本地化.md)

## المزايا الرئيسية

- **بروتوكول كامل**: تحليل إطارات JT/T 808-2011/2013/2019 مع محددات 0x7E، وتهريب 0x7D، والمجموع الاختباري XOR، وإعادة تجميع الحزم الفرعية
- **أجسام رسائل غنية**: أكثر من 70 جسم رسالة قياسي (0x0001~0x8A00) يغطي اتجاه الرفع من الطرفية واتجاه الإرسال من المنصة
- **توافق ثلاثي الإصدارات**: تكيّف تلقائي للحقول عبر حقل `Version` مع السمتين `[JT2011]`/`[JT2019]`
- **وضعان للتسلسل**: انعكاس مدفوع بالسمات للأجسام البسيطة، و`IAccessor` للأجسام المعقدة التي تحتاج تحكمًا يدويًا
- **تحليل TLV/KLV**: مرمّزات مدمجة للنوع-الطول-القيمة والنوع-الطول-القيمة مع تحويل إلى قواميس قوية النوع
- **طبقة الشبكات**: مرمّز الحزم الملتصقة في TCP باسم `JTCodec` والعميل `JT808Client` مع توجيه تلقائي للمعالجات
- **امتداد SuBiao**: إنذارات السلامة النشطة ADAS/DSM/TPMS/BSD، وحالة الأجهزة الملحقة، وبروتوكول المرفقات
- **امتداد YueBiao**: معيار السلامة النشطة في قوانغدونغ مع تقرير إنذار مخصص (0x1FC4)، وتمرير USB، وامتدادات الموقع، ومعاملات المورّد
- **أساسيات JT/T 1078**: نماذج أجسام رسائل الصوت والفيديو وتعداداتها
- **مسجل القيادة**: مرمّز حزم JT/T 19056 الصاعدة والهابطة
- **ترميز BCD**: سمات تسلسل مخصصة لسلاسل BCD والطوابع الزمنية
- **ترميز GBK**: مرمّز سلاسل GBK مدمج مع تسجيل تلقائي لـ `CodePagesEncodingProvider`
- **إدارة Web API**: إرسال موحّد للأوامر، وإدارة الجلسات المتصلة، والقائمة السوداء، ومصادقة الرمز المميّز
- **تجريد قائمة انتظار الرسائل**: ينشئ `IMessageBus` المنتجين والمستهلكين وأحداث الجلسة ومعالجات الاتجاه الهابط؛ و`RedisMessageBus` هو النقل الداخلي المدمج، وتُركَّب محوّلات RocketMQ/Kafka في `JT808.Server` عبر مفتاح التهيئة `JT808:Messaging`
- **إرسال الأوامر في العنقود**: يشارك `ClusterState` الجلسات المتصلة ونبضات العقد والقائمة السوداء عبر Redis، ويوجّه الأوامر إلى العقدة التي تملك الطرفية، ويُبقي `ICommandBus` كنقطة امتداد لنقل مخصص
- **تقوية العميل**: إعادة اتصال تلقائية، ونبضات قابلة للتهيئة، وعدّادات إرسال واستقبال
- **دعم أطر متعددة**: `net45 / net461 / netstandard2.0 / netstandard2.1 / net8.0`
- **متوافق مع AOT**: مشروع فحص NativeAOT يغطي مسارات المرمّز والمصنع والتسلسل
- **التوطين**: 11 لغة وقت تشغيل جاهزة، مع النص الإنجليزي المصدر كبديل، وجداول آمنة مع AOT
- **تكامل RocketMQ**: محوّل منتج/مستهلك في مشروع `JT808.Server` (غير مشمول في حزمة NuGet)، يدعم Alibaba Cloud وHuawei Cloud وTencent Cloud ومصادقة ACL
- **تكامل Kafka**: محوّل منتج/مستهلك في مشروع `JT808.Server` (غير مشمول في حزمة NuGet)، مبني على `Confluent.Kafka` مع مجموعات المستهلكين ودعم ACK

## إصدارات البروتوكول المدعومة

| الإصدار | العلامة | الوصف |
|---------|---------|-------|
| JT/T 808-2011 | `Version=0` | إصدار مبكر مع اختلافات في بعض هياكل الرسائل |
| JT/T 808-2013 | `Version=0` | الإصدار الأكثر انتشارًا |
| JT/T 808-2019 | `Version=1` | أحدث إصدار مع حقول ورسائل جديدة |

## مصفوفة الرسائل

| # | معرّف الرسالة | الاسم | الاتجاه | مكتمل |
|:---:|:------:|---------|:----:|:----:|
| 1 | 0x0001 | استجابة الطرفية العامة | صاعد | ✅ |
| 2 | 0x8001 | الاستجابة العامة للمنصة | هابط | ✅ |
| 3 | 0x0002 | نبضة الطرفية | صاعد | ✅ |
| 4 | 0x8003 | طلب إعادة إرسال الحزمة الفرعية | هابط | ✅ |
| 5 | 0x0004 | طلب استعلام وقت الخادم | صاعد | ✅ |
| 6 | 0x0100 | تسجيل الطرفية | صاعد | ✅ |
| 7 | 0x8100 | استجابة تسجيل الطرفية | هابط | ✅ |
| 8 | 0x0003 | إلغاء تسجيل الطرفية | صاعد | ✅ |
| 9 | 0x0102 | مصادقة الطرفية | صاعد | ✅ |
| 10 | 0x8103 | تعيين معاملات الطرفية | هابط | ✅ |
| 11 | 0x8104 | استعلام معاملات الطرفية | هابط | ✅ |
| 12 | 0x0104 | استجابة استعلام معاملات الطرفية | صاعد | ✅ |
| 13 | 0x8105 | التحكم في الطرفية | هابط | ✅ |
| 14 | 0x8106 | استعلام معاملات طرفية محددة | هابط | ✅ |
| 15 | 0x8107 | استعلام سمات الطرفية | هابط | ✅ |
| 16 | 0x0107 | استجابة استعلام سمات الطرفية | صاعد | ✅ |
| 17 | 0x8108 | إرسال حزمة ترقية الطرفية | هابط | ✅ |
| 18 | 0x0108 | إشعار نتيجة ترقية الطرفية | صاعد | ✅ |
| 19 | 0x0200 | تقرير الموقع | صاعد | ✅ |
| 20 | 0x8201 | استعلام الموقع | هابط | ✅ |
| 21 | 0x0201 | استجابة استعلام الموقع | صاعد | ✅ |
| 22 | 0x8202 | التحكم في التتبع المؤقت للموقع | هابط | ✅ |
| 23 | 0x8203 | تأكيد الإنذار يدويًا | هابط | ✅ |
| 24 | 0x8300 | إرسال رسالة نصية | هابط | ✅ |
| 25 | 0x8301 | إعداد الأحداث | هابط | ✅ |
| 26 | 0x0301 | تقرير الحدث | صاعد | ✅ |
| 27 | 0x8302 | إرسال سؤال | هابط | ✅ |
| 28 | 0x0302 | استجابة السؤال | صاعد | ✅ |
| 29 | 0x8303 | إعداد قائمة المعلومات عند الطلب | هابط | ✅ |
| 30 | 0x0303 | المعلومات عند الطلب / الإلغاء | صاعد | ✅ |
| 31 | 0x8304 | خدمة المعلومات | هابط | ✅ |
| 32 | 0x8400 | معاودة الاتصال الهاتفي | هابط | ✅ |
| 33 | 0x8401 | تعيين دفتر الهاتف | هابط | ✅ |
| 34 | 0x8500 | التحكم في المركبة | هابط | ✅ |
| 35 | 0x0500 | استجابة التحكم في المركبة | صاعد | ✅ |
| 36 | 0x8600 | تعيين منطقة دائرية | هابط | ✅ |
| 37 | 0x8601 | حذف منطقة دائرية | هابط | ✅ |
| 38 | 0x8602 | تعيين منطقة مستطيلة | هابط | ✅ |
| 39 | 0x8603 | حذف منطقة مستطيلة | هابط | ✅ |
| 40 | 0x8604 | تعيين منطقة متعددة الأضلاع | هابط | ✅ |
| 41 | 0x8605 | حذف منطقة متعددة الأضلاع | هابط | ✅ |
| 42 | 0x8606 | تعيين المسار | هابط | ✅ |
| 43 | 0x8607 | حذف المسار | هابط | ✅ |
| 44 | 0x8608 | استعلام بيانات المنطقة أو المسار | هابط | ✅ |
| 45 | 0x0608 | استجابة استعلام بيانات المنطقة أو المسار | صاعد | ✅ |
| 46 | 0x8700 | أمر جمع بيانات مسجل القيادة | هابط | ✅ |
| 47 | 0x0700 | رفع بيانات مسجل القيادة | صاعد | ✅ |
| 48 | 0x8701 | أمر إرسال معاملات مسجل القيادة | هابط | ✅ |
| 49 | 0x0701 | تقرير بوليصة الشحن الإلكترونية | صاعد | ✅ |
| 50 | 0x8702 | طلب الإبلاغ عن هوية السائق | هابط | ✅ |
| 51 | 0x0702 | تقرير معلومات هوية السائق | صاعد | ✅ |
| 52 | 0x0704 | رفع بيانات المواقع على دفعات | صاعد | ✅ |
| 53 | 0x0705 | رفع بيانات ناقل CAN | صاعد | ✅ |
| 54 | 0x0800 | تقرير حدث الوسائط المتعددة | صاعد | ✅ |
| 55 | 0x0801 | رفع بيانات الوسائط المتعددة | صاعد | ✅ |
| 56 | 0x8800 | استجابة رفع بيانات الوسائط المتعددة | هابط | ✅ |
| 57 | 0x8801 | أمر التقاط فوري من الكاميرا | هابط | ✅ |
| 58 | 0x0805 | استجابة الالتقاط الفوري من الكاميرا | صاعد | ✅ |
| 59 | 0x8802 | البحث في بيانات الوسائط المتعددة المخزنة | هابط | ✅ |
| 60 | 0x0802 | استجابة البحث في بيانات الوسائط المتعددة المخزنة | صاعد | ✅ |
| 61 | 0x8803 | رفع بيانات الوسائط المتعددة المخزنة | هابط | ✅ |
| 62 | 0x8804 | أمر بدء التسجيل | هابط | ✅ |
| 63 | 0x8805 | رفع استرجاع قطعة واحدة من بيانات الوسائط المخزنة | هابط | ✅ |
| 64 | 0x8900 | تمرير البيانات نحو الأسفل | هابط | ✅ |
| 65 | 0x0900 | تمرير البيانات نحو الأعلى | صاعد | ✅ |
| 66 | 0x0901 | تقرير البيانات المضغوطة | صاعد | ✅ |
| 67 | 0x8A00 | المفتاح العام RSA للمنصة | هابط | ✅ |
| 68 | 0x0005 | طلب الطرفية إعادة إرسال الحزمة الفرعية | صاعد | ✅ |
| 69 | 0x8204 | كشف حالة الوصلة | هابط | ✅ |
| 70 | 0x6006 | استجابة تسليم الرسالة النصية | صاعد | ✅ |
| 71 | 0x1080 | رسالة مخصصة | — | ✅ |

## البدء السريع

### تثبيت حزمة NuGet

```bash
dotnet add package NewLife.JT808
```

### تحليل رسالة صاعدة

```csharp
using NewLife.JT808.Protocols;
using NewLife.JT808.Models;

// Scan the assembly and register every message type
var factory = MessageFactory.Instance;
factory.Init();

// Raw bytes reported by the terminal
var raw = new Byte[] { 0x7E, 0x02, 0x00, /* ... */, 0x7E };

// Decode the JTMessage frame
var jtMessage = new JTMessage();
var stream = new MemoryStream(raw);
jtMessage.Read(stream, null);

// Materialize the strongly-typed body by message ID
var body = factory.Parse(jtMessage);
if (body is T0200 position)
{
    Console.WriteLine($"Latitude={position.Latitude / 1_000_000.0}");
    Console.WriteLine($"Longitude={position.Longitude / 1_000_000.0}");
    Console.WriteLine($"Speed={position.Speed} km/h");
}
```

### إنشاء رسالة هابطة

```csharp
// Build a platform general response
var t8001 = new T8001
{
    Sequence = jtMessage.Sequence,
    Kind = jtMessage.Kind,
    Result = ResultKinds.Success
};

// Serialize the body
var ms = new MemoryStream();
var writer = Helper.CreateWriter(ms, jtMessage);
writer.TryWrite(null, ref t8001);

// Wrap it in a JTMessage frame
var reply = new JTMessage
{
    Kind = MessageKinds.平台通用应答,
    Mobile = jtMessage.Mobile,
    Sequence = (UInt16)(jtMessage.Sequence + 1),
    Payload = new ArrayPacket(ms.ToArray()),
    Reply = false
};

// Emit the complete frame
var outMs = new MemoryStream();
reply.Write(outMs, null);
var frameBytes = outMs.ToArray();
```

### تسلسل جسم معقد يدويًا (IAccessor)

```csharp
[MessageKind(MessageKinds.位置信息汇报)]
public class T0200 : IAccessor
{
    public PositionAlarms Alarm { get; set; }
    public PositionStatus Status { get; set; }
    public Int32 Latitude { get; set; }
    public Int32 Longitude { get; set; }

    public Boolean Read(Stream stream, Object context)
    {
        var reader = context as Binary ?? Helper.CreateReader(stream);
        Alarm = (PositionAlarms)reader.ReadUInt32();
        Status = (PositionStatus)reader.ReadUInt32();
        Latitude = reader.ReadInt32();
        Longitude = reader.ReadInt32();
        // ... remaining fields
        return true;
    }

    public Boolean Write(Stream stream, Object context)
    {
        var writer = context as Binary ?? Helper.CreateWriter(stream);
        writer.Write((UInt32)Alarm);
        writer.Write((UInt32)Status);
        writer.Write(Latitude);
        writer.Write(Longitude);
        // ... remaining fields
        return true;
    }
}
```

### التوطين

توفّر المكتبة 11 لغة وقت التشغيل (الصينية المبسطة/التقليدية، والإنجليزية، والإسبانية، والألمانية، واليابانية، والفرنسية، والبرتغالية، والروسية، والكورية، والعربية). تُفهرَس الرسائل بالنص الإنجليزي المصدر وتعود إليه عند غياب الترجمة.

```csharp
using System.Globalization;
using NewLife;

// Follow the process UI culture by default; pin it explicitly if needed
Localization.Culture = CultureInfo.GetCultureInfo("ja");

// ASP.NET Core hosts can resolve the request language from Accept-Language
using var scope = RequestCultureScope.Create(Request.Headers.AcceptLanguage);
```

### NativeAOT

يكتشف `MessageFactory.Init()` أجسام الرسائل عبر فحص تجميعة البروتوكول. يجب على تطبيقات NativeAOT أو التطبيقات المقلَّمة (trimmed) الإبقاء على تلك التجميعة:

```xml
<ItemGroup>
  <TrimmerRootAssembly Include="NewLife.JT808" />
</ItemGroup>
```

يُنشَر مشروع `AotSmoke` في هذا المستودع باستخدام NativeAOT ويشغّل 16 سيناريو على Windows/Linux (x64 وarm64).

## خريطة المنظومة

NewLife منظومة متعدّدة الطبقات من مشاريع مفتوحة المصدر تُصدَر بإصدارات مستقلة. الخريطة أدناه مشتركة حرفيًا بين جميع مستودعات NewLife؛ وكل مشروع يرتبط بمستودعه وبحزمته الرئيسية على NuGet.

| المشروع | الوظيفة | التنزيلات |
| --- | --- | --- |
| **الأساس** | | |
| [NewLife.Core](https://github.com/NewLifeX/X) | التسجيل والتهيئة والتخزين المؤقت والشبكات والتسلسل والجدولة وتتبّع أداء APM. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Core?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Core) |
| [NewLife.XCode](https://github.com/NewLifeX/NewLife.XCode) | وسيط بيانات: جداول واحدة بمستوى عشرات المليارات من الصفوف، تقسيم تلقائي للجداول، فصل القراءة عن الكتابة، وأكثر من 10 قواعد بيانات. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.XCode?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.XCode) |
| [NewLife.Net](https://github.com/NewLifeX/NewLife.Net) | مكتبة شبكات عالية الأداء: 1.77 مليون pps / 14.5 Gbps على جهاز واحد (TCP 1KB، 2026-10)، و4 ملايين اتصال TCP طويل الأمد. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Net?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Net) |
| [NewLife.Remoting](https://github.com/NewLifeX/NewLife.Remoting) | إطار اتصال بروتوكولي وRPC لتطبيقات العميل/الخادم وأجهزة إنترنت الأشياء. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Remoting?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Remoting) |
| [NewLife.Agent](https://github.com/NewLifeX/NewLife.Agent) | يثبّت تطبيقات .NET كخدمات حارسة لنظام التشغيل: خدمات Windows وsystemd في Linux. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Agent?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Agent) |
| [NewLife.Templates](https://github.com/NewLifeX/NewLife.Zero) | قوالب مشاريع `dotnet new` للطرفية وweb وAPI والخدمات والبيانات وإنترنت الأشياء. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Templates?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Templates) |
| **البيانات والمراسلة** | | |
| [NewLife.Redis](https://github.com/NewLifeX/NewLife.Redis) | عميل Redis بزمن استجابة بالميكروثانية، وملايين العمليات في الثانية، وقوائم رسائل مدمجة. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Redis?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Redis) |
| [NewLife.RocketMQ](https://github.com/NewLifeX/NewLife.RocketMQ) | عميل RocketMQ مُدار بالكامل لـ Apache RocketMQ وخدمة رسائل Alibaba Cloud. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.RocketMQ?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.RocketMQ) |
| [NewLife.MQTT](https://github.com/NewLifeX/NewLife.MQTT) | عميل وخادم MQTT، بما في ذلك الاتصال بـ Alibaba Cloud IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.MQTT?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.MQTT) |
| [NewLife.NovaDb](https://github.com/NewLifeX/NewLife.NovaDb) | قاعدة بيانات هجينة بلغة C#: مدمجة أو شبكية، وعلائقية، وسلاسل زمنية، وقائمة رسائل، وKV في واحد. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.NovaDb?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.NovaDb) |
| [NewLife.MySql](https://github.com/NewLifeX/NewLife.MySql) | برنامج تشغيل MySql وطبقة مساعدة مبنية على مكدّس بيانات NewLife. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.MySql?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.MySql) |
| **Web والذكاء الاصطناعي والأنظمة الموزّعة** | | |
| [NewLife.Cube](https://github.com/NewLifeX/NewLife.Cube) | إطار منصة إدارة سريعة: الصلاحيات، والدخول الموحّد SSO، وخادم OAuth، وتوليد واجهات CRUD. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Cube?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Cube) |
| [Stardust](https://github.com/NewLifeX/Stardust) | منصة خدمات موزّعة: إدارة العقد، وAPM، ومركز الإعدادات، ومركز التسجيل، ومركز النشر. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Stardust?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Stardust) |
| [AntJob](https://github.com/NewLifeX/AntJob) | منصة موزّعة للمهام وجدولة البيانات الفورية، مع التقسيم وإعادة المحاولة وإعادة التعبئة. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.AntJob?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.AntJob) |
| [NewLife.AI](https://github.com/NewLifeX/NewLife.AI) | عميل موحّد لـ 46 مزوّد LLM بالإضافة إلى ChatAI، تطبيق ويب جاهز للمحادثة. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.AI?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.AI) |
| **الصناعة وإنترنت الأشياء** | | |
| [NewLife.IoT](https://github.com/NewLifeX/NewLife.IoT) | مكتبة معايير إنترنت الأشياء التي تعرّف عقود البروتوكولات وتجريدات الأجهزة. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.IoT?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.IoT) |
| [NewLife.Modbus](https://github.com/NewLifeX/NewLife.Modbus) | تنفيذ Modbus TCP/RTU/ASCII فوق NewLife.IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Modbus?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Modbus) |
| [NewLife.Siemens](https://github.com/NewLifeX/NewLife.Siemens) | بروتوكول Siemens PLC فوق NewLife.IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Siemens?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Siemens) |
| [NewLife.Omron](https://github.com/NewLifeX/NewLife.Omron) | بروتوكول Omron PLC فوق NewLife.IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Omron?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Omron) |
| [NewLife.Melsec](https://github.com/NewLifeX/NewLife.Melsec) | بروتوكول Mitsubishi Melsec PLC فوق NewLife.IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Melsec?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Melsec) |
| [NewLife.OPC](https://github.com/NewLifeX/NewLife.OPC) | عميل OPC للوصول إلى البيانات الصناعية. | — |
| [NewLife.BACnet](https://github.com/NewLifeX/NewLife.BACnet) | مكدّس بروتوكول BACnet لأتمتة المباني. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.BACnet?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.BACnet) |
| [ZeroIoT](https://github.com/NewLifeX/ZeroIoT) | منصة إنترنت أشياء مفتوحة المصدر وخفيفة، مع إدارة البوابات والأجهزة. | — |
| **تليماتيكا المركبات** | | |
| [NewLife.JT808](https://github.com/NewLifeX/NewLife.JT808) | بروتوكول تحديد مواقع المركبات JT/T 808: أكثر من 70 جسم رسالة، 2011/2013/2019. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.JT808?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.JT808) |
| [NewLife.JT809](https://github.com/NewLifeX/NewLife.JT809) | بروتوكول JT/T 809 بين المنصات لجهات إدارة النقل. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.JT809?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.JT809) |
| [NewLife.JT1078](https://github.com/NewLifeX/NewLife.JT1078) | فيديو داخل المركبة JT/T 1078: تحليل RTP وإعادة التجميع إلى FLV/fMP4 والبث المباشر. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.JT1078?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.JT1078) |
| **مكوّنات المجال** | | |
| [NewLife.Office](https://github.com/NewLifeX/NewLife.Office) | حزمة واحدة لـ Excel وWord وPPT وPDF وأكثر من 15 صيغة مستندات. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Office?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Office) |
| [NewLife.Audio](https://github.com/NewLifeX/NewLife.Audio) | صوت عبر المنصات: مرمّزات، وإدخال/إخراج الأجهزة، وDSP، وحاويات، وبث. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Audio?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Audio) |
| [NewLife.Video](https://github.com/NewLifeX/NewLife.Video) | مرمّزات الفيديو وصيغ الحاويات ومعالجة الإطارات والبث. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Video?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Video) |
| [NewLife.Map](https://github.com/NewLifeX/NewLife.Map) | واجهة موحّدة فوق Baidu وAMap وTencent وTianditu مع الترميز الجغرافي وتحويل الإحداثيات. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Map?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Map) |
| [NewLife.IP](https://github.com/NewLifeX/NewLife.IP) | تحديد الموقع الجغرافي لعناوين IPv4 بقاعدة بيانات غير متصلة مُعيّنة في الذاكرة، وواجهة API عبر الإنترنت كبديل. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.IP?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.IP) |
| **الأدوات والمنتجات** | | |
| [CrazyCoder](https://github.com/NewLifeX/XCoder) | صندوق أدوات المطوّر: الشبكات والمنفذ التسلسلي والتشفير والتعبيرات النمطية وModbus وMQTT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.XCoder?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.XCoder) |
| [GitCandy](https://github.com/NewLifeX/GitCandy) | نظام استضافة ذاتية لإدارة الشيفرة المصدرية Git. | — |
| [LuckyClover](https://github.com/NewLifeX/LuckyClover) | يثبّت بيئة تشغيل .NET المطابقة تلقائيًا عند التشغيل من مضيف أقدم. | — |

> تعرض الشارات الحزمة الرئيسية لكل مشروع؛ أما حزم السمات والإضافات والحزم التابعة فمذكورة في صفحاتها على NuGet. الحزم التي تتجاوز 30 حزمة وغير المذكورة هنا (FTP وLoRa وإضافات Office وبرامج تشغيل البروتوكولات وغيرها) متاحة في [منظمة NewLifeX](https://github.com/NewLifeX) وعلى [newlifex.com](https://newlifex.com).

## الفريق

<img src="https://newlifex.com/logo.png" width="48" align="right" alt="NewLife" />

تأسّست NewLife في عام 2002، وأمضت أكثر من عقدين في منظومة .NET وإنترنت الأشياء، وتدير ما يقارب 100 مشروع مفتوح المصدر (34 منها مشاريع رئيسية في هذه الخريطة)، وتقدّم حلولًا متكاملة للبرمجيات والأجهزة لقطاعات الكهرباء والخدمات اللوجستية والرعاية الصحية والتحكّم الصناعي والإنترنت. ونلتزم بمبدأ "لا أسبقية في التعلّم — الإتقان يكسب الاحترام".

نُزّلت المكوّنات مفتوحة المصدر <!--nuget-total:start-->6.48M<!--nuget-total:end--> مرة من NuGet (على مستوى المنظومة بأكملها، حتى 2026-10).

الموقع: https://newlifex.com · المنظمة مفتوحة المصدر: https://github.com/NewLifeX

<details>
<summary>حساب WeChat الرسمي: 智能大石头 (امسح للاشتراك)</summary>

![智能大石头](https://newlifex.com/stone.jpg)

</details>
