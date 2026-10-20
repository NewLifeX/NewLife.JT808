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

<!-- lang:start -->
🌐 [English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [Español](README.es.md) | [Deutsch](README.de.md) | [日本語](README.ja.md) | [Français](README.fr.md) | [Português](README.pt.md) | [Русский](README.ru.md) | [한국어](README.ko.md)
<!-- lang:end -->

> قد تكون هذه الترجمة متأخرة؛ راجع [English README](README.md).

تطبيق كامل لبروتوكول JT/T 808 لتتبع المركبات، يدعم إصدارات 2011/2013/2019 مع أكثر من 70 جسم رسالة قياسي، وامتدادات السلامة النشطة SuBiao/YueBiao، ورسائل الصوت والفيديو JT/T 1078، وبروتوكول مسجل القيادة JT/T 19056، والتكامل مع قوائم انتظار الرسائل.

## المزايا الرئيسية

- **بروتوكول كامل**: حدود الإطار 0x7E، وتهريب 0x7D، والمجموع الاختباري XOR، وإعادة تجميع الحزم الفرعية
- **رسائل غنية**: أكثر من 70 جسم رسالة قياسي للاتجاهين الصاعد والهابط
- **توافق ثلاثي الإصدارات**: تكيّف تلقائي للحقول عبر `Version` والسمات `[JT2011]`/`[JT2019]`
- **تسلسل مزدوج**: قائم على السمات و`IAccessor` للأجسام المعقدة
- **TLV/KLV**: مرمزات عامة مع تحويل إلى قواميس قوية النوع
- **طبقة الشبكة**: مرمز `JTCodec` وعميل `JT808Client` مع توجيه تلقائي
- **متوافق مع AOT**: مشروع فحص NativeAOT للمسارات الأساسية
- **تعدد اللغات**: 11 لغة وقت التشغيل مع الرجوع إلى الإنجليزية

## إصدارات البروتوكول

| الإصدار | العلامة | الوصف |
|---------|---------|-------|
| JT/T 808-2011 | `Version=0` | إصدار مبكر مع اختلافات في بعض هياكل الرسائل |
| JT/T 808-2013 | `Version=0` | الإصدار الأكثر انتشارًا |
| JT/T 808-2019 | `Version=1` | أحدث إصدار مع حقول ورسائل جديدة |

## التثبيت

```bash
dotnet add package NewLife.JT808
```

## البدء السريع

```bash
dotnet add package NewLife.JT808
```

```csharp
using NewLife.JT808.Protocols;
using NewLife.JT808.Models;

var factory = MessageFactory.Instance;
factory.Init();

var jtMessage = new JTMessage();
jtMessage.Read(new MemoryStream(raw), null);

if (factory.Parse(jtMessage) is T0200 position)
{
    Console.WriteLine($"Latitude={position.Latitude / 1_000_000.0}");
    Console.WriteLine($"Longitude={position.Longitude / 1_000_000.0}");
    Console.WriteLine($"Speed={position.Speed} km/h");
}
```

## الروابط

- الشيفرة المصدرية: https://github.com/NewLifeX/NewLife.JT808
- NuGet: https://www.nuget.org/packages/NewLife.JT808
- التوثيق: [English](README.md) | [简体中文](README.zh-CN.md)

> NewLife Team · https://newlifex.com
