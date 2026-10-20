# NewLife.JT808 - JT/T 808 Vehicle Tracking Protocol

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
🌐 [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [Español](README.es.md) | [Deutsch](README.de.md) | [日本語](README.ja.md) | [Français](README.fr.md) | [Português](README.pt.md) | [Русский](README.ru.md) | [한국어](README.ko.md) | [العربية](README.ar.md)
<!-- lang:end -->

A complete implementation of the JT/T 808 vehicle tracking protocol, covering the 2011/2013/2019 versions with 70+ standard message bodies, the SuBiao (Jiangsu) and YueBiao (Guangdong) active-safety extensions, JT/T 1078 audio/video messages, the JT/T 19056 driving-recorder protocol, and message-queue integration.

Source: https://github.com/NewLifeX/NewLife.JT808  
NuGet: [NewLife.JT808](https://www.nuget.org/packages/NewLife.JT808)  
Docs: [Requirements](Doc/需求文档.md) / [Architecture](Doc/架构设计.md) / [Feature list](Doc/功能清单.md) / [User guide](Doc/使用文档.md) / [Localization](Doc/多语言本地化.md)

## Key features

- **Complete protocol**: JT/T 808-2011/2013/2019 frame parsing with 0x7E delimiters, 0x7D escaping, XOR checksum and sub-packet reassembly
- **Rich message bodies**: 70+ standard message bodies (0x0001~0x8A00) covering both terminal uplink and platform downlink
- **Three-version compatibility**: automatic field adaptation through the `Version` field plus `[JT2011]`/`[JT2019]` attributes
- **Dual serialization modes**: attribute-driven reflection for simple bodies, `IAccessor` for complex bodies that need manual control
- **TLV/KLV parsing**: built-in Type-Length-Value and Kind-Length-Value codecs with strongly-typed dictionary conversion
- **Networking layer**: TCP sticky-packet codec `JTCodec` and `JT808Client` with automatic handler routing
- **SuBiao extension**: ADAS/DSM/TPMS/BSD active-safety alarms, peripheral status and attachment protocol
- **YueBiao extension**: Guangdong active-safety standard with dedicated alarm reporting (0x1FC4), USB pass-through, position extensions and vendor parameters
- **JT/T 1078 basics**: audio/video message body models and enums
- **Driving recorder**: JT/T 19056 uplink and downlink packet codec
- **BCD encoding**: custom serialization attributes for BCD strings and timestamps
- **GBK encoding**: built-in GBK string codec with automatic `CodePagesEncodingProvider` registration
- **Web API management**: unified command delivery, online session management, blacklist and token authentication
- **Message-queue abstraction**: `IMsgProducer`/`IMsgConsumer`/`ISessionProducer` interfaces with an in-memory `DefaultMsgQueue`; Redis Stream, RocketMQ and Kafka adapters ship in the `JT808.Server` project
- **Cluster command delivery**: `ClusterState` shares online sessions, node heartbeats and the blacklist through Redis, routes commands to the node that owns the terminal, and keeps `ICommandBus` as an extension point for custom transports
- **Client hardening**: automatic reconnection, configurable heartbeat and send/receive counters
- **Multi-framework support**: `net45 / net461 / netstandard2.0 / netstandard2.1 / net8.0`
- **AOT friendly**: NativeAOT smoke project covering codec, factory and serialization paths
- **Localization**: 11 runtime languages out of the box, English source text as fallback, AOT-safe tables
- **RocketMQ integration**: producer/consumer adapter in the `JT808.Server` project (not shipped in the NuGet package), supporting Alibaba Cloud, Huawei Cloud, Tencent Cloud and ACL authentication
- **Kafka integration**: producer/consumer adapter in the `JT808.Server` project (not shipped in the NuGet package), based on `Confluent.Kafka` with consumer groups and ACK support

## Supported protocol versions

| Version | Flag | Description |
|---------|------|-------------|
| JT/T 808-2011 | `Version=0` | Early version with a few different message layouts |
| JT/T 808-2013 | `Version=0` | The most widely deployed version |
| JT/T 808-2019 | `Version=1` | Latest version with new fields and message types |

## Message matrix

| # | Message ID | Name | Direction | Done |
|:---:|:------:|---------|:----:|:----:|
| 1 | 0x0001 | Terminal general response | Uplink | ✅ |
| 2 | 0x8001 | Platform general response | Downlink | ✅ |
| 3 | 0x0002 | Terminal heartbeat | Uplink | ✅ |
| 4 | 0x8003 | Retransmit sub-packet request | Downlink | ✅ |
| 5 | 0x0004 | Query server time request | Uplink | ✅ |
| 6 | 0x0100 | Terminal registration | Uplink | ✅ |
| 7 | 0x8100 | Terminal registration response | Downlink | ✅ |
| 8 | 0x0003 | Terminal deregistration | Uplink | ✅ |
| 9 | 0x0102 | Terminal authentication | Uplink | ✅ |
| 10 | 0x8103 | Set terminal parameters | Downlink | ✅ |
| 11 | 0x8104 | Query terminal parameters | Downlink | ✅ |
| 12 | 0x0104 | Query terminal parameters response | Uplink | ✅ |
| 13 | 0x8105 | Terminal control | Downlink | ✅ |
| 14 | 0x8106 | Query specified terminal parameters | Downlink | ✅ |
| 15 | 0x8107 | Query terminal attributes | Downlink | ✅ |
| 16 | 0x0107 | Query terminal attributes response | Uplink | ✅ |
| 17 | 0x8108 | Deliver terminal upgrade package | Downlink | ✅ |
| 18 | 0x0108 | Terminal upgrade result notification | Uplink | ✅ |
| 19 | 0x0200 | Position report | Uplink | ✅ |
| 20 | 0x8201 | Position query | Downlink | ✅ |
| 21 | 0x0201 | Position query response | Uplink | ✅ |
| 22 | 0x8202 | Temporary position tracking control | Downlink | ✅ |
| 23 | 0x8203 | Manually confirm alarm | Downlink | ✅ |
| 24 | 0x8300 | Deliver text message | Downlink | ✅ |
| 25 | 0x8301 | Event setup | Downlink | ✅ |
| 26 | 0x0301 | Event report | Uplink | ✅ |
| 27 | 0x8302 | Deliver question | Downlink | ✅ |
| 28 | 0x0302 | Question response | Uplink | ✅ |
| 29 | 0x8303 | Information on-demand menu setup | Downlink | ✅ |
| 30 | 0x0303 | Information on-demand / cancel | Uplink | ✅ |
| 31 | 0x8304 | Information service | Downlink | ✅ |
| 32 | 0x8400 | Phone callback | Downlink | ✅ |
| 33 | 0x8401 | Set phone book | Downlink | ✅ |
| 34 | 0x8500 | Vehicle control | Downlink | ✅ |
| 35 | 0x0500 | Vehicle control response | Uplink | ✅ |
| 36 | 0x8600 | Set circular area | Downlink | ✅ |
| 37 | 0x8601 | Delete circular area | Downlink | ✅ |
| 38 | 0x8602 | Set rectangular area | Downlink | ✅ |
| 39 | 0x8603 | Delete rectangular area | Downlink | ✅ |
| 40 | 0x8604 | Set polygonal area | Downlink | ✅ |
| 41 | 0x8605 | Delete polygonal area | Downlink | ✅ |
| 42 | 0x8606 | Set route | Downlink | ✅ |
| 43 | 0x8607 | Delete route | Downlink | ✅ |
| 44 | 0x8608 | Query area or route data | Downlink | ✅ |
| 45 | 0x0608 | Query area or route data response | Uplink | ✅ |
| 46 | 0x8700 | Driving recorder data collection command | Downlink | ✅ |
| 47 | 0x0700 | Driving recorder data upload | Uplink | ✅ |
| 48 | 0x8701 | Driving recorder parameter delivery command | Downlink | ✅ |
| 49 | 0x0701 | Electronic waybill report | Uplink | ✅ |
| 50 | 0x8702 | Report driver identity request | Downlink | ✅ |
| 51 | 0x0702 | Driver identity information report | Uplink | ✅ |
| 52 | 0x0704 | Batch position data upload | Uplink | ✅ |
| 53 | 0x0705 | CAN bus data upload | Uplink | ✅ |
| 54 | 0x0800 | Multimedia event report | Uplink | ✅ |
| 55 | 0x0801 | Multimedia data upload | Uplink | ✅ |
| 56 | 0x8800 | Multimedia data upload response | Downlink | ✅ |
| 57 | 0x8801 | Camera immediate capture command | Downlink | ✅ |
| 58 | 0x0805 | Camera immediate capture response | Uplink | ✅ |
| 59 | 0x8802 | Stored multimedia data search | Downlink | ✅ |
| 60 | 0x0802 | Stored multimedia data search response | Uplink | ✅ |
| 61 | 0x8803 | Stored multimedia data upload | Downlink | ✅ |
| 62 | 0x8804 | Start recording command | Downlink | ✅ |
| 63 | 0x8805 | Single stored multimedia data retrieval upload | Downlink | ✅ |
| 64 | 0x8900 | Downstream pass-through | Downlink | ✅ |
| 65 | 0x0900 | Upstream pass-through | Uplink | ✅ |
| 66 | 0x0901 | Compressed data report | Uplink | ✅ |
| 67 | 0x8A00 | Platform RSA public key | Downlink | ✅ |
| 68 | 0x0005 | Terminal retransmit sub-packet request | Uplink | ✅ |
| 69 | 0x8204 | Link detection | Downlink | ✅ |
| 70 | 0x6006 | Text message delivery response | Uplink | ✅ |
| 71 | 0x1080 | Custom message | — | ✅ |

## Quick start

### Install the NuGet package

```bash
dotnet add package NewLife.JT808
```

### Parse an uplink message

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

### Build a downlink message

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

### Manually serialize a complex body (IAccessor)

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

### Localization

The library ships 11 runtime languages (Simplified/Traditional Chinese, English, Spanish, German, Japanese, French, Portuguese, Russian, Korean and Arabic). Messages are keyed by the English source text and fall back to it when a translation is missing.

```csharp
using System.Globalization;
using NewLife;

// Follow the process UI culture by default; pin it explicitly if needed
Localization.Culture = CultureInfo.GetCultureInfo("ja");

// ASP.NET Core hosts can resolve the request language from Accept-Language
using var scope = RequestCultureScope.Create(Request.Headers.AcceptLanguage);
```

### NativeAOT

`MessageFactory.Init()` discovers message bodies by scanning the protocol assembly. NativeAOT or trimmed apps must keep that assembly:

```xml
<ItemGroup>
  <TrimmerRootAssembly Include="NewLife.JT808" />
</ItemGroup>
```

The `AotSmoke` project in this repository publishes with NativeAOT and runs 16 scenarios on Windows/Linux (x64 and arm64).

## Ecosystem Map

NewLife is a layered ecosystem of independently versioned open-source projects. The map below is shared verbatim by every NewLife repository; each project links to its repository and its primary NuGet package.

| Project | What it does | Downloads |
| --- | --- | --- |
| **Foundation** | | |
| [NewLife.Core](https://github.com/NewLifeX/X) | Logging, configuration, caching, networking, serialization, scheduling and APM tracing. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Core?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Core) |
| [NewLife.XCode](https://github.com/NewLifeX/NewLife.XCode) | Data middleware: single tables at the tens-of-billions scale, automatic sharding, read-write splitting, 10+ databases. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.XCode?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.XCode) |
| [NewLife.Net](https://github.com/NewLifeX/NewLife.Net) | High-performance network library: 1.77M pps / 14.5 Gbps on a single machine (TCP 1KB, 2026-10), 4M long-lived TCP connections. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Net?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Net) |
| [NewLife.Remoting](https://github.com/NewLifeX/NewLife.Remoting) | Protocol communication and RPC framework for client/server applications and IoT devices. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Remoting?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Remoting) |
| [NewLife.Agent](https://github.com/NewLifeX/NewLife.Agent) | Installs .NET applications as OS daemons: Windows services and Linux systemd. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Agent?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Agent) |
| [NewLife.Templates](https://github.com/NewLifeX/NewLife.Zero) | `dotnet new` project templates for console, web, API, service, data and IoT projects. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Templates?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Templates) |
| **Data & Messaging** | | |
| [NewLife.Redis](https://github.com/NewLifeX/NewLife.Redis) | Redis client with microsecond latency, millions of ops/s and built-in message queues. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Redis?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Redis) |
| [NewLife.RocketMQ](https://github.com/NewLifeX/NewLife.RocketMQ) | Pure managed RocketMQ client for Apache RocketMQ and Alibaba Cloud Message Queue. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.RocketMQ?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.RocketMQ) |
| [NewLife.MQTT](https://github.com/NewLifeX/NewLife.MQTT) | MQTT client and server, including Alibaba Cloud IoT connectivity. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.MQTT?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.MQTT) |
| [NewLife.NovaDb](https://github.com/NewLifeX/NewLife.NovaDb) | Hybrid database in C#: embedded or networked, relational, time-series, message queue and KV in one. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.NovaDb?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.NovaDb) |
| [NewLife.MySql](https://github.com/NewLifeX/NewLife.MySql) | MySql driver and helper layer built on the NewLife data stack. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.MySql?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.MySql) |
| **Web, AI & Distributed** | | |
| [NewLife.Cube](https://github.com/NewLifeX/NewLife.Cube) | Rapid admin-platform framework: permissions, SSO, OAuth server and CRUD scaffolding. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Cube?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Cube) |
| [Stardust](https://github.com/NewLifeX/Stardust) | Distributed service platform: node management, APM, configuration center, registry and release center. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Stardust?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Stardust) |
| [AntJob](https://github.com/NewLifeX/AntJob) | Distributed task and real-time data scheduling platform with sharding, retries and backfill. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.AntJob?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.AntJob) |
| [NewLife.AI](https://github.com/NewLifeX/NewLife.AI) | Unified client for 46 LLM providers plus ChatAI, a ready-to-run web chat application. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.AI?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.AI) |
| **Industrial & IoT** | | |
| [NewLife.IoT](https://github.com/NewLifeX/NewLife.IoT) | IoT standard library defining protocol contracts and device abstractions. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.IoT?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.IoT) |
| [NewLife.Modbus](https://github.com/NewLifeX/NewLife.Modbus) | Modbus TCP/RTU/ASCII implementation on top of NewLife.IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Modbus?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Modbus) |
| [NewLife.Siemens](https://github.com/NewLifeX/NewLife.Siemens) | Siemens PLC protocol on top of NewLife.IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Siemens?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Siemens) |
| [NewLife.Omron](https://github.com/NewLifeX/NewLife.Omron) | Omron PLC protocol on top of NewLife.IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Omron?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Omron) |
| [NewLife.Melsec](https://github.com/NewLifeX/NewLife.Melsec) | Mitsubishi Melsec PLC protocol on top of NewLife.IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Melsec?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Melsec) |
| [NewLife.OPC](https://github.com/NewLifeX/NewLife.OPC) | OPC client for industrial data access. | — |
| [NewLife.BACnet](https://github.com/NewLifeX/NewLife.BACnet) | BACnet building-automation protocol stack. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.BACnet?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.BACnet) |
| [ZeroIoT](https://github.com/NewLifeX/ZeroIoT) | Lightweight open-source IoT platform with gateway and device management. | — |
| **Vehicle Telematics** | | |
| [NewLife.JT808](https://github.com/NewLifeX/NewLife.JT808) | JT/T 808 vehicle positioning protocol: 70+ message bodies, 2011/2013/2019. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.JT808?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.JT808) |
| [NewLife.JT809](https://github.com/NewLifeX/NewLife.JT809) | JT/T 809 platform-to-platform protocol for transport authorities. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.JT809?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.JT809) |
| [NewLife.JT1078](https://github.com/NewLifeX/NewLife.JT1078) | JT/T 1078 in-vehicle video: RTP parsing, FLV/fMP4 remuxing and live streaming. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.JT1078?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.JT1078) |
| **Domain Components** | | |
| [NewLife.Office](https://github.com/NewLifeX/NewLife.Office) | One package for Excel, Word, PPT, PDF and 15+ document formats. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Office?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Office) |
| [NewLife.Audio](https://github.com/NewLifeX/NewLife.Audio) | Cross-platform audio: codecs, device I/O, DSP, containers and streaming. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Audio?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Audio) |
| [NewLife.Video](https://github.com/NewLifeX/NewLife.Video) | Video codecs, container formats, frame processing and streaming. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Video?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Video) |
| [NewLife.Map](https://github.com/NewLifeX/NewLife.Map) | Unified interface over Baidu, AMap, Tencent and Tianditu with geocoding and coordinate conversion. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Map?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Map) |
| [NewLife.IP](https://github.com/NewLifeX/NewLife.IP) | IPv4 geolocation with an offline memory-mapped database and an online API fallback. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.IP?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.IP) |
| **Tools & Products** | | |
| [CrazyCoder](https://github.com/NewLifeX/XCoder) | Developer toolbox: network, serial port, crypto, regex, Modbus and MQTT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.XCoder?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.XCoder) |
| [GitCandy](https://github.com/NewLifeX/GitCandy) | Self-hosted Git source code management system. | — |
| [LuckyClover](https://github.com/NewLifeX/LuckyClover) | Installs the matching .NET runtime automatically when launched from an older host. | — |

> Badges show the primary package of each project; theme, extension and satellite packages are listed on their NuGet pages. The 30+ packages not listed here (FTP, LoRa, Office extensions, protocol drivers and more) are available under the [NewLifeX organization](https://github.com/NewLifeX) and on [newlifex.com](https://newlifex.com).

## Team

<img src="https://newlifex.com/logo.png" width="48" align="right" alt="NewLife" />

Founded in 2002, the NewLife team has spent more than two decades on the .NET and IoT ecosystem, maintains nearly 100 open-source projects (34 of them core to this map), and delivers integrated software and hardware solutions for power, logistics, healthcare, industrial control and internet industries. We live by the motto "learning has no seniority — mastery earns respect".

Open-source components have been downloaded <!--nuget-total:start-->6.48M<!--nuget-total:end--> times from NuGet (ecosystem-wide, as of 2026-10).

Website: https://newlifex.com · Open source organization: https://github.com/NewLifeX

<details>
<summary>WeChat Official Account: 智能大石头 (scan to follow)</summary>

![智能大石头](https://newlifex.com/stone.jpg)

</details>