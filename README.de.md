# NewLife.JT808 - JT/T 808 Fahrzeugortungsprotokoll

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
🌐 [English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [Español](README.es.md) | [日本語](README.ja.md) | [Français](README.fr.md) | [Português](README.pt.md) | [Русский](README.ru.md) | [한국어](README.ko.md) | [العربية](README.ar.md)
<!-- lang:end -->

> Diese Übersetzung kann veraltet sein; maßgeblich ist das [English README](README.md).

Vollständige Implementierung des JT/T 808-Fahrzeugortungsprotokolls für die Versionen 2011/2013/2019 mit über 70 standardisierten Nachrichtenkörpern, den SuBiao/YueBiao-Erweiterungen für aktive Sicherheit, JT/T 1078-Audio/Video-Nachrichten, dem JT/T 19056-Fahrtschreiberprotokoll und Message-Queue-Integration.

## Hauptmerkmale

- **Vollständiges Protokoll**: 0x7E-Rahmen, 0x7D-Escaping, XOR-Prüfsumme und Teilpaket-Reassemblierung
- **Umfangreiche Nachrichten**: über 70 Standardkörper für Uplink und Downlink
- **Drei Versionen**: automatische Feldanpassung über `Version` und `[JT2011]`/`[JT2019]`
- **Duale Serialisierung**: attributgesteuert sowie `IAccessor` für komplexe Körper
- **TLV/KLV**: generische Codecs mit stark typisierten Wörterbüchern
- **Netzwerkschicht**: `JTCodec` für Klebepakete und `JT808Client` mit automatischem Routing
- **AOT-freundlich**: NativeAOT-Smoke-Projekt für die Kernpfade
- **Lokalisierung**: 11 Laufzeitsprachen mit englischem Rückfall

## Protokollversionen

| Version | Kennzeichen | Beschreibung |
|---------|-------------|--------------|
| JT/T 808-2011 | `Version=0` | Frühe Version mit abweichenden Nachrichtenlayouts |
| JT/T 808-2013 | `Version=0` | Am weitesten verbreitete Version |
| JT/T 808-2019 | `Version=1` | Neueste Version mit neuen Feldern und Nachrichten |

## Installation

```bash
dotnet add package NewLife.JT808
```

## Schnellstart

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

## Links

- Quellcode: https://github.com/NewLifeX/NewLife.JT808
- NuGet: https://www.nuget.org/packages/NewLife.JT808
- Dokumentation: [English](README.md) | [简体中文](README.zh-CN.md)

> NewLife Team · https://newlifex.com
