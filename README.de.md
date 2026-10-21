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
![platform](https://img.shields.io/badge/platform-Windows%20%7C%20Linux%20%7C%20macOS-blue)
[![contributors](https://img.shields.io/github/contributors/NewLifeX/NewLife.JT808?logo=github)](https://github.com/NewLifeX/NewLife.JT808/graphs/contributors)

<!-- lang:start -->
🌐 [English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [Español](README.es.md) | [日本語](README.ja.md) | [Français](README.fr.md) | [Português](README.pt.md) | [Русский](README.ru.md) | [한국어](README.ko.md) | [العربية](README.ar.md)
<!-- lang:end -->

> Diese Übersetzung kann veraltet sein; maßgeblich ist das [English README](README.md).

Eine vollständige Implementierung des JT/T 808-Fahrzeugortungsprotokolls, die die Versionen 2011/2013/2019 mit über 70 standardisierten Nachrichtenkörpern, die Active-Safety-Erweiterungen SuBiao (Jiangsu) und YueBiao (Guangdong), JT/T 1078-Audio-/Video-Nachrichten, das JT/T 19056-Fahrtschreiberprotokoll sowie die Integration von Message-Queues abdeckt.

Source: https://github.com/NewLifeX/NewLife.JT808  
NuGet: [NewLife.JT808](https://www.nuget.org/packages/NewLife.JT808)  
Docs: [Anforderungen](Doc/需求文档.md) / [Architektur](Doc/架构设计.md) / [Funktionsliste](Doc/功能清单.md) / [Benutzerhandbuch](Doc/使用文档.md) / [Lokalisierung](Doc/多语言本地化.md)

## Hauptmerkmale

- **Vollständiges Protokoll**: Rahmung nach JT/T 808-2011/2013/2019 mit 0x7E-Begrenzern, 0x7D-Escaping, XOR-Prüfsumme und Reassemblierung von Teilpaketen
- **Umfangreiche Nachrichtenkörper**: über 70 standardisierte Nachrichtenkörper (0x0001~0x8A00) für Uplink vom Terminal und Downlink von der Plattform
- **Kompatibilität mit drei Versionen**: automatische Feldanpassung über das Feld `Version` sowie die Attribute `[JT2011]`/`[JT2019]`
- **Zwei Serialisierungsmodi**: attributgesteuerte Reflexion für einfache Körper, `IAccessor` für komplexe Körper mit manueller Steuerung
- **TLV/KLV-Analyse**: integrierte Type-Length-Value- und Kind-Length-Value-Codecs mit stark typisierter Wörterbuchkonvertierung
- **Netzwerkschicht**: TCP-Klebepaket-Codec `JTCodec` und `JT808Client` mit automatischem Handler-Routing
- **SuBiao-Erweiterung**: Active-Safety-Alarme ADAS/DSM/TPMS/BSD, Peripheriestatus und Anhangprotokoll
- **YueBiao-Erweiterung**: Guangdong-Standard für aktive Sicherheit mit eigener Alarmmeldung (0x1FC4), USB-Pass-Through, Positionserweiterungen und Herstellerparametern
- **JT/T 1078-Grundlagen**: Modelle und Enums für Audio-/Video-Nachrichtenkörper
- **Fahrtschreiber**: Codec für Uplink- und Downlink-Pakete nach JT/T 19056
- **BCD-Kodierung**: benutzerdefinierte Serialisierungsattribute für BCD-Zeichenketten und Zeitstempel
- **GBK-Kodierung**: integrierter GBK-Zeichenketten-Codec mit automatischer Registrierung des `CodePagesEncodingProvider`
- **Web-API-Verwaltung**: einheitliche Befehlszustellung, Verwaltung von Online-Sitzungen, Blacklist und Token-Authentifizierung
- **Message-Queue-Abstraktion**: `IMessageBus` erzeugt Producer/Consumer, Sitzungsereignisse und Downlink-Handler; `RedisMessageBus` ist der integrierte interne Transport, während RocketMQ-/Kafka-Adapter im Projekt `JT808.Server` über den Konfigurationsschalter `JT808:Messaging` eingebunden werden
- **Befehlszustellung im Cluster**: `ClusterState` teilt Online-Sitzungen, Knoten-Heartbeats und die Blacklist über Redis, leitet Befehle an den Knoten weiter, der das Terminal besitzt, und hält `ICommandBus` als Erweiterungspunkt für eigene Transporte bereit
- **Gehärteter Client**: automatische Wiederverbindung, konfigurierbarer Heartbeat sowie Sende- und Empfangszähler
- **Unterstützung mehrerer Frameworks**: `net45 / net461 / netstandard2.0 / netstandard2.1 / net8.0`
- **AOT-freundlich**: NativeAOT-Smoke-Projekt für Codec-, Factory- und Serialisierungspfade
- **Lokalisierung**: 11 Laufzeitsprachen sofort einsatzbereit, englischer Ausgangstext als Rückfall, AOT-sichere Tabellen
- **RocketMQ-Integration**: Producer-/Consumer-Adapter im Projekt `JT808.Server` (nicht im NuGet-Paket enthalten) mit Unterstützung für Alibaba Cloud, Huawei Cloud, Tencent Cloud und ACL-Authentifizierung
- **Kafka-Integration**: Producer-/Consumer-Adapter im Projekt `JT808.Server` (nicht im NuGet-Paket enthalten), basierend auf `Confluent.Kafka` mit Consumergruppen und ACK-Unterstützung

## Unterstützte Protokollversionen

| Version | Kennzeichen | Beschreibung |
|---------|-------------|--------------|
| JT/T 808-2011 | `Version=0` | Frühe Version mit einigen abweichenden Nachrichtenlayouts |
| JT/T 808-2013 | `Version=0` | Am weitesten verbreitete Version |
| JT/T 808-2019 | `Version=1` | Neueste Version mit neuen Feldern und Nachrichtentypen |

## Nachrichtenmatrix

| # | Nachrichten-ID | Name | Richtung | Fertig |
|:---:|:------:|---------|:----:|:----:|
| 1 | 0x0001 | Allgemeine Antwort des Terminals | Uplink | ✅ |
| 2 | 0x8001 | Allgemeine Antwort der Plattform | Downlink | ✅ |
| 3 | 0x0002 | Heartbeat des Terminals | Uplink | ✅ |
| 4 | 0x8003 | Anforderung zur erneuten Übertragung von Teilpaketen | Downlink | ✅ |
| 5 | 0x0004 | Anfrage nach der Serverzeit | Uplink | ✅ |
| 6 | 0x0100 | Registrierung des Terminals | Uplink | ✅ |
| 7 | 0x8100 | Antwort auf die Registrierung des Terminals | Downlink | ✅ |
| 8 | 0x0003 | Abmeldung des Terminals | Uplink | ✅ |
| 9 | 0x0102 | Authentifizierung des Terminals | Uplink | ✅ |
| 10 | 0x8103 | Terminalparameter setzen | Downlink | ✅ |
| 11 | 0x8104 | Terminalparameter abfragen | Downlink | ✅ |
| 12 | 0x0104 | Antwort auf die Abfrage der Terminalparameter | Uplink | ✅ |
| 13 | 0x8105 | Steuerung des Terminals | Downlink | ✅ |
| 14 | 0x8106 | Bestimmte Terminalparameter abfragen | Downlink | ✅ |
| 15 | 0x8107 | Terminalattribute abfragen | Downlink | ✅ |
| 16 | 0x0107 | Antwort auf die Abfrage der Terminalattribute | Uplink | ✅ |
| 17 | 0x8108 | Upgrade-Paket an das Terminal übermitteln | Downlink | ✅ |
| 18 | 0x0108 | Benachrichtigung über das Upgrade-Ergebnis des Terminals | Uplink | ✅ |
| 19 | 0x0200 | Positionsmeldung | Uplink | ✅ |
| 20 | 0x8201 | Positionsabfrage | Downlink | ✅ |
| 21 | 0x0201 | Antwort auf die Positionsabfrage | Uplink | ✅ |
| 22 | 0x8202 | Steuerung der temporären Positionsverfolgung | Downlink | ✅ |
| 23 | 0x8203 | Alarm manuell bestätigen | Downlink | ✅ |
| 24 | 0x8300 | Textnachricht übermitteln | Downlink | ✅ |
| 25 | 0x8301 | Ereignis einrichten | Downlink | ✅ |
| 26 | 0x0301 | Ereignismeldung | Uplink | ✅ |
| 27 | 0x8302 | Frage übermitteln | Downlink | ✅ |
| 28 | 0x0302 | Antwort auf die Frage | Uplink | ✅ |
| 29 | 0x8303 | On-Demand-Informationsmenü einrichten | Downlink | ✅ |
| 30 | 0x0303 | On-Demand-Information / Abbrechen | Uplink | ✅ |
| 31 | 0x8304 | Informationsdienst | Downlink | ✅ |
| 32 | 0x8400 | Telefonrückruf | Downlink | ✅ |
| 33 | 0x8401 | Telefonbuch setzen | Downlink | ✅ |
| 34 | 0x8500 | Fahrzeugsteuerung | Downlink | ✅ |
| 35 | 0x0500 | Antwort auf die Fahrzeugsteuerung | Uplink | ✅ |
| 36 | 0x8600 | Kreisförmigen Bereich setzen | Downlink | ✅ |
| 37 | 0x8601 | Kreisförmigen Bereich löschen | Downlink | ✅ |
| 38 | 0x8602 | Rechteckigen Bereich setzen | Downlink | ✅ |
| 39 | 0x8603 | Rechteckigen Bereich löschen | Downlink | ✅ |
| 40 | 0x8604 | Polygonen Bereich setzen | Downlink | ✅ |
| 41 | 0x8605 | Polygonen Bereich löschen | Downlink | ✅ |
| 42 | 0x8606 | Route setzen | Downlink | ✅ |
| 43 | 0x8607 | Route löschen | Downlink | ✅ |
| 44 | 0x8608 | Bereichs- oder Routendaten abfragen | Downlink | ✅ |
| 45 | 0x0608 | Antwort auf die Abfrage von Bereichs- oder Routendaten | Uplink | ✅ |
| 46 | 0x8700 | Befehl zum Sammeln von Fahrtschreiberdaten | Downlink | ✅ |
| 47 | 0x0700 | Hochladen von Fahrtschreiberdaten | Uplink | ✅ |
| 48 | 0x8701 | Befehl zur Übermittlung von Fahrtschreiberparametern | Downlink | ✅ |
| 49 | 0x0701 | Meldung des elektronischen Frachtbriefs | Uplink | ✅ |
| 50 | 0x8702 | Anforderung zur Meldung der Fahreridentität | Downlink | ✅ |
| 51 | 0x0702 | Meldung der Fahreridentitätsinformationen | Uplink | ✅ |
| 52 | 0x0704 | Stapelweises Hochladen von Positionsdaten | Uplink | ✅ |
| 53 | 0x0705 | Hochladen von CAN-Bus-Daten | Uplink | ✅ |
| 54 | 0x0800 | Multimedia-Ereignismeldung | Uplink | ✅ |
| 55 | 0x0801 | Hochladen von Multimediadaten | Uplink | ✅ |
| 56 | 0x8800 | Antwort auf das Hochladen von Multimediadaten | Downlink | ✅ |
| 57 | 0x8801 | Befehl zur sofortigen Aufnahme durch die Kamera | Downlink | ✅ |
| 58 | 0x0805 | Antwort auf die sofortige Aufnahme durch die Kamera | Uplink | ✅ |
| 59 | 0x8802 | Suche nach gespeicherten Multimediadaten | Downlink | ✅ |
| 60 | 0x0802 | Antwort auf die Suche nach gespeicherten Multimediadaten | Uplink | ✅ |
| 61 | 0x8803 | Hochladen gespeicherter Multimediadaten | Downlink | ✅ |
| 62 | 0x8804 | Befehl zum Start der Aufzeichnung | Downlink | ✅ |
| 63 | 0x8805 | Hochladen einzelner abgerufener gespeicherter Multimediadaten | Downlink | ✅ |
| 64 | 0x8900 | Downstream-Pass-Through | Downlink | ✅ |
| 65 | 0x0900 | Upstream-Pass-Through | Uplink | ✅ |
| 66 | 0x0901 | Meldung komprimierter Daten | Uplink | ✅ |
| 67 | 0x8A00 | Öffentlicher RSA-Schlüssel der Plattform | Downlink | ✅ |
| 68 | 0x0005 | Anforderung des Terminals zur erneuten Übertragung von Teilpaketen | Uplink | ✅ |
| 69 | 0x8204 | Verbindungserkennung | Downlink | ✅ |
| 70 | 0x6006 | Antwort auf die Zustellung einer Textnachricht | Uplink | ✅ |
| 71 | 0x1080 | Benutzerdefinierte Nachricht | — | ✅ |

## Schnellstart

### NuGet-Paket installieren

```bash
dotnet add package NewLife.JT808
```

### Eine Uplink-Nachricht analysieren

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

### Eine Downlink-Nachricht erstellen

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

### Einen komplexen Nachrichtenkörper manuell serialisieren (IAccessor)

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

### Lokalisierung

Die Bibliothek enthält 11 Laufzeitsprachen (vereinfachtes und traditionelles Chinesisch, Englisch, Spanisch, Deutsch, Japanisch, Französisch, Portugiesisch, Russisch, Koreanisch und Arabisch). Nachrichten werden über den englischen Ausgangstext adressiert und fallen darauf zurück, wenn eine Übersetzung fehlt.

```csharp
using System.Globalization;
using NewLife;

// Follow the process UI culture by default; pin it explicitly if needed
Localization.Culture = CultureInfo.GetCultureInfo("ja");

// ASP.NET Core hosts can resolve the request language from Accept-Language
using var scope = RequestCultureScope.Create(Request.Headers.AcceptLanguage);
```

### NativeAOT

`MessageFactory.Init()` ermittelt die Nachrichtenkörper durch Scannen der Protokollassembly. NativeAOT- oder getrimmte Anwendungen müssen diese Assembly beibehalten:

```xml
<ItemGroup>
  <TrimmerRootAssembly Include="NewLife.JT808" />
</ItemGroup>
```

Das Projekt `AotSmoke` in diesem Repository wird mit NativeAOT veröffentlicht und führt unter Windows/Linux (x64 und arm64) 16 Szenarien aus.

## Ökosystem-Karte

NewLife ist ein geschichtetes Ökosystem unabhängig versionierter Open-Source-Projekte. Die folgende Karte wird wortgleich in allen NewLife-Repositories verwendet; jedes Projekt verlinkt auf sein Repository und sein primäres NuGet-Paket.

| Projekt | Funktion | Downloads |
| --- | --- | --- |
| **Grundlage** | | |
| [NewLife.Core](https://github.com/NewLifeX/X) | Protokollierung, Konfiguration, Caching, Netzwerk, Serialisierung, Zeitplanung und APM-Tracing. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Core?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Core) |
| [NewLife.XCode](https://github.com/NewLifeX/NewLife.XCode) | Daten-Middleware: einzelne Tabellen in der Größenordnung von zig Milliarden Zeilen, automatisches Sharding, Lese-/Schreibtrennung, über 10 Datenbanken. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.XCode?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.XCode) |
| [NewLife.Net](https://github.com/NewLifeX/NewLife.Net) | Hochperformante Netzwerkbibliothek: 1,77 Mio. pps / 14,5 Gbps auf einer Maschine (TCP 1KB, 2026-10), 4 Mio. langlebige TCP-Verbindungen. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Net?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Net) |
| [NewLife.Remoting](https://github.com/NewLifeX/NewLife.Remoting) | Protokollkommunikation und RPC-Framework für Client/Server-Anwendungen und IoT-Geräte. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Remoting?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Remoting) |
| [NewLife.Agent](https://github.com/NewLifeX/NewLife.Agent) | Installiert .NET-Anwendungen als Betriebssystem-Dienste: Windows-Dienste und Linux-systemd. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Agent?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Agent) |
| [NewLife.Templates](https://github.com/NewLifeX/NewLife.Zero) | `dotnet new`-Projektvorlagen für Konsole, Web, API, Dienst, Daten und IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Templates?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Templates) |
| **Daten & Messaging** | | |
| [NewLife.Redis](https://github.com/NewLifeX/NewLife.Redis) | Redis-Client mit Mikrosekunden-Latenz, Millionen Operationen pro Sekunde und integrierten Nachrichtenwarteschlangen. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Redis?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Redis) |
| [NewLife.RocketMQ](https://github.com/NewLifeX/NewLife.RocketMQ) | Vollständig verwalteter RocketMQ-Client für Apache RocketMQ und Alibaba Cloud Message Queue. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.RocketMQ?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.RocketMQ) |
| [NewLife.MQTT](https://github.com/NewLifeX/NewLife.MQTT) | MQTT-Client und -Server, einschließlich Alibaba Cloud IoT-Anbindung. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.MQTT?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.MQTT) |
| [NewLife.NovaDb](https://github.com/NewLifeX/NewLife.NovaDb) | Hybriddatenbank in C#: eingebettet oder im Netzwerk, relational, Zeitreihen, Nachrichtenwarteschlange und KV in einem. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.NovaDb?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.NovaDb) |
| [NewLife.MySql](https://github.com/NewLifeX/NewLife.MySql) | MySql-Treiber und Hilfsschicht auf Basis des NewLife-Datenstacks. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.MySql?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.MySql) |
| **Web, KI & verteilt** | | |
| [NewLife.Cube](https://github.com/NewLifeX/NewLife.Cube) | Schnelles Framework für Administrationsplattformen: Berechtigungen, SSO, OAuth-Server und CRUD-Gerüst. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Cube?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Cube) |
| [Stardust](https://github.com/NewLifeX/Stardust) | Verteilte Dienstplattform: Knotenverwaltung, APM, Konfigurationszentrum, Registry und Release-Center. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Stardust?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Stardust) |
| [AntJob](https://github.com/NewLifeX/AntJob) | Plattform für verteilte Aufgaben und Echtzeit-Datenplanung mit Sharding, Wiederholungen und Backfill. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.AntJob?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.AntJob) |
| [NewLife.AI](https://github.com/NewLifeX/NewLife.AI) | Einheitlicher Client für 46 LLM-Anbieter plus ChatAI, eine sofort einsatzbereite Web-Chat-Anwendung. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.AI?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.AI) |
| **Industrie & IoT** | | |
| [NewLife.IoT](https://github.com/NewLifeX/NewLife.IoT) | IoT-Standardbibliothek mit Protokollverträgen und Geräteabstraktionen. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.IoT?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.IoT) |
| [NewLife.Modbus](https://github.com/NewLifeX/NewLife.Modbus) | Modbus-TCP/RTU/ASCII-Implementierung auf Basis von NewLife.IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Modbus?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Modbus) |
| [NewLife.Siemens](https://github.com/NewLifeX/NewLife.Siemens) | Siemens-SPS-Protokoll auf Basis von NewLife.IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Siemens?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Siemens) |
| [NewLife.Omron](https://github.com/NewLifeX/NewLife.Omron) | Omron-SPS-Protokoll auf Basis von NewLife.IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Omron?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Omron) |
| [NewLife.Melsec](https://github.com/NewLifeX/NewLife.Melsec) | Mitsubishi-Melsec-SPS-Protokoll auf Basis von NewLife.IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Melsec?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Melsec) |
| [NewLife.OPC](https://github.com/NewLifeX/NewLife.OPC) | OPC-Client für industriellen Datenzugriff. | — |
| [NewLife.BACnet](https://github.com/NewLifeX/NewLife.BACnet) | BACnet-Protokollstack für die Gebäudeautomation. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.BACnet?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.BACnet) |
| [ZeroIoT](https://github.com/NewLifeX/ZeroIoT) | Leichtgewichtige Open-Source-IoT-Plattform mit Gateway- und Geräteverwaltung. | — |
| **Fahrzeugtelematik** | | |
| [NewLife.JT808](https://github.com/NewLifeX/NewLife.JT808) | JT/T-808-Fahrzeugortungsprotokoll: über 70 Nachrichtenkörper, 2011/2013/2019. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.JT808?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.JT808) |
| [NewLife.JT809](https://github.com/NewLifeX/NewLife.JT809) | JT/T-809-Plattform-zu-Plattform-Protokoll für Verkehrsbehörden. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.JT809?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.JT809) |
| [NewLife.JT1078](https://github.com/NewLifeX/NewLife.JT1078) | JT/T-1078-Fahrzeugvideo: RTP-Parsing, FLV/fMP4-Remuxing und Live-Streaming. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.JT1078?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.JT1078) |
| **Domänenkomponenten** | | |
| [NewLife.Office](https://github.com/NewLifeX/NewLife.Office) | Ein Paket für Excel, Word, PPT, PDF und über 15 Dokumentformate. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Office?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Office) |
| [NewLife.Audio](https://github.com/NewLifeX/NewLife.Audio) | Plattformübergreifendes Audio: Codecs, Geräte-I/O, DSP, Container und Streaming. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Audio?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Audio) |
| [NewLife.Video](https://github.com/NewLifeX/NewLife.Video) | Videocodecs, Containerformate, Frame-Verarbeitung und Streaming. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Video?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Video) |
| [NewLife.Map](https://github.com/NewLifeX/NewLife.Map) | Einheitliche Schnittstelle für Baidu, AMap, Tencent und Tianditu mit Geokodierung und Koordinatenumrechnung. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Map?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Map) |
| [NewLife.IP](https://github.com/NewLifeX/NewLife.IP) | IPv4-Geolokalisierung mit einer offline speichergemappten Datenbank und einer Online-API als Fallback. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.IP?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.IP) |
| **Werkzeuge & Produkte** | | |
| [CrazyCoder](https://github.com/NewLifeX/XCoder) | Entwickler-Toolbox: Netzwerk, serielle Schnittstelle, Krypto, Regex, Modbus und MQTT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.XCoder?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.XCoder) |
| [GitCandy](https://github.com/NewLifeX/GitCandy) | Selbst gehostetes Git-Quellcode-Verwaltungssystem. | — |
| [LuckyClover](https://github.com/NewLifeX/LuckyClover) | Installiert automatisch die passende .NET-Laufzeit, wenn es von einem älteren Host gestartet wird. | — |

> Die Badges zeigen das primäre Paket jedes Projekts; Theme-, Erweiterungs- und Satellitenpakete sind auf den jeweiligen NuGet-Seiten aufgeführt. Die hier nicht aufgeführten über 30 Pakete (FTP, LoRa, Office-Erweiterungen, Protokolltreiber und mehr) sind in der [NewLifeX-Organisation](https://github.com/NewLifeX) und auf [newlifex.com](https://newlifex.com) verfügbar.

## Team

<img src="https://newlifex.com/logo.png" width="48" align="right" alt="NewLife" />

NewLife wurde 2002 gegründet und widmet sich seit mehr als zwei Jahrzehnten dem .NET- und IoT-Ökosystem, betreut fast 100 Open-Source-Projekte (34 davon Hauptprojekte) und liefert integrierte Hard- und Softwarelösungen für die Bereiche Energie, Logistik, Gesundheitswesen, Industrieautomation und Internet. Unser Leitspruch lautet „Lernen kennt kein Dienstalter – wer es beherrscht, wird zum Lehrmeister“.

Die Open-Source-Komponenten wurden <!--nuget-total:start-->6.48M<!--nuget-total:end--> Mal von NuGet heruntergeladen (ökosystemweit, Stand 2026-10).

Website: https://newlifex.com · Open-Source-Organisation: https://github.com/NewLifeX

<details>
<summary>WeChat-Offizieller Account: 智能大石头 (zum Folgen scannen)</summary>

![智能大石头](https://newlifex.com/stone.jpg)

</details>
