# NewLife.JT808 - Протокол спутникового мониторинга транспорта JT/T 808

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
🌐 [English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [Español](README.es.md) | [Deutsch](README.de.md) | [日本語](README.ja.md) | [Français](README.fr.md) | [Português](README.pt.md) | [한국어](README.ko.md) | [العربية](README.ar.md)
<!-- lang:end -->

> Этот перевод может отставать; см. [English README](README.md).

Полная реализация протокола JT/T 808 для мониторинга транспорта: версии 2011/2013/2019, более 70 стандартных тел сообщений, расширения активной безопасности SuBiao/YueBiao, сообщения аудио/видео JT/T 1078, протокол тахографа JT/T 19056 и интеграция с очередями сообщений.

## Основные возможности

- **Полный протокол**: кадры 0x7E, экранирование 0x7D, XOR-контрольная сумма и сборка подпакетов
- **Богатый набор сообщений**: более 70 стандартных тел для восходящего и нисходящего потоков
- **Три версии**: автоматическая адаптация полей через `Version` и `[JT2011]`/`[JT2019]`
- **Двойная сериализация**: на основе атрибутов и `IAccessor` для сложных тел
- **TLV/KLV**: универсальные кодеки со строго типизированными словарями
- **Сетевой уровень**: кодек `JTCodec` и клиент `JT808Client` с автоматической маршрутизацией
- **Поддержка AOT**: проект NativeAOT-проверки основных сценариев
- **Локализация**: 11 языков во время выполнения с откатом на английский

## Версии протокола

| Версия | Признак | Описание |
|--------|---------|----------|
| JT/T 808-2011 | `Version=0` | Ранняя версия с отличиями в структуре сообщений |
| JT/T 808-2013 | `Version=0` | Наиболее распространённая версия |
| JT/T 808-2019 | `Version=1` | Новейшая версия с новыми полями и сообщениями |

## Установка

```bash
dotnet add package NewLife.JT808
```

## Быстрый старт

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

## Ссылки

- Исходный код: https://github.com/NewLifeX/NewLife.JT808
- NuGet: https://www.nuget.org/packages/NewLife.JT808
- Документация: [English](README.md) | [简体中文](README.zh-CN.md)

> NewLife Team · https://newlifex.com
