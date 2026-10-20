# NewLife.JT808 - JT/T 808 车辆定位协议

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
🌐 [English](README.md) | [繁體中文](README.zh-TW.md) | [Español](README.es.md) | [Deutsch](README.de.md) | [日本語](README.ja.md) | [Français](README.fr.md) | [Português](README.pt.md) | [Русский](README.ru.md) | [한국어](README.ko.md) | [العربية](README.ar.md)
<!-- lang:end -->

JT/T 808 车辆定位协议完整实现，支持 2011/2013/2019 三个版本，涵盖 70+ 标准消息体、苏标/粤标主动安全扩展、JT/T 1078 音视频基础消息、JT/T 19056 行车记录仪协议及 RocketMQ 消息队列集成。

源码： https://github.com/NewLifeX/NewLife.JT808  
Nuget：[NewLife.JT808](https://www.nuget.org/packages/NewLife.JT808)  
文档：[需求文档](Doc/需求文档.md) / [架构设计](Doc/架构设计.md) / [功能清单](Doc/功能清单.md) / [使用文档](Doc/使用文档.md) / [多语言本地化](Doc/多语言本地化.md)

## 主要特点

- **协议完整**：实现 JT/T 808-2011/2013/2019 消息帧解析，支持 0x7E 帧边界、0x7D 转义、异或校验、分包处理
- **消息体丰富**：覆盖 70+ 标准消息体（0x0001~0x8A00），终端上行 + 平台下行全覆盖
- **三版本兼容**：通过 `Version` 字段 + `[JT2011]`/`[JT2019]` 特性实现三版本字段自动适配
- **双模式序列化**：简单消息体使用属性驱动反射序列化，复杂消息体实现 `IAccessor` 手动控制读写
- **TLV/KLV 解析**：内置 Type-Length-Value 和 Kind-Length-Value 通用编解码，支持强类型字典转换
- **网络通信层**：内置 TCP 粘包编解码器 `JTCodec` 和客户端 `JT808Client`，支持消息处理器自动路由
- **苏标扩展**：支持 ADAS/DSM/TPMS/BSD 主动安全报警信息，外设状态/信息解析，报警附件协议
- **粤标扩展**：支持广东省主动安全标准（粤标），含独立报警上报（0x1FC4）、USB 外设透传、位置附加信息、专用参数设置
- **音视频基础**：JT/T 1078 音视频消息体模型和枚举定义
- **行车记录仪**：JT/T 19056 上下行数据包编解码
- **BCD 编码**：BCD 字符串和时间自定义序列化特性，适配协议特有编码格式
- **GBK 编码**：内置 GBK 中文字符串编解码，自动注册 `CodePagesEncodingProvider`
- **Web API 管理**：支持统一下发指令、在线会话管理、黑名单管理、Token 鉴权，方便集成到后台系统
- **消息队列抽象**：IMsgProducer/IMsgConsumer/ISessionProducer 等接口解耦消息处理，内置 DefaultMsgQueue 内存实现、Redis Stream 生产/消费支持、Kafka 适配器
- **跨服务器指令下发**：ICommandBus 接口 + Stardust 事件总线，支持集群广播指令到指定终端
- **客户端增强**：自动重连、可配心跳、收发统计计数器，提升生产环境可靠性
- **AOT 兼容**：NativeAOT 冒烟工程覆盖编解码、消息工厂与序列化主路径
- **多语言**：内置 11 种运行时语言，英文源文兜底，词表压缩内嵌且 AOT 安全
- **多框架支持**：`net45 / net461 / netstandard2.0 / netstandard2.1 / net8.0` 全兼容
- **RocketMQ 集成**：基于 `NewLife.RocketMQ` 的消息队列生产/消费适配器，位于 `JT808.Server` 项目（不随 NuGet 包发布），支持阿里云/华为云/腾讯云/ACL 四种云厂商认证模式
- **Kafka 集成**：基于 `Confluent.Kafka` 的消息队列生产/消费适配器，支持消费组、ACK 机制（需要 .NET Standard 2.0+）

## 支持的协议版本

| 版本 | 标识 | 说明 |
|------|------|------|
| JT/T 808-2011 | `Version=0` | 早期版本，部分消息体结构不同 |
| JT/T 808-2013 | `Version=0` | 当前广泛使用的版本 |
| JT/T 808-2019 | `Version=1` | 最新版本，增加新版字段和消息类型 |

## 消息对照表

| 序号 | 消息ID | 消息名称 | 方向 | 完成 |
|:---:|:------:|---------|:----:|:----:|
| 1 | 0x0001 | 终端通用应答 | 上行 | ✅ |
| 2 | 0x8001 | 平台通用应答 | 下行 | ✅ |
| 3 | 0x0002 | 终端心跳 | 上行 | ✅ |
| 4 | 0x8003 | 补传分包请求 | 下行 | ✅ |
| 5 | 0x0004 | 查询服务器时间请求 | 上行 | ✅ |
| 6 | 0x0100 | 终端注册 | 上行 | ✅ |
| 7 | 0x8100 | 终端注册应答 | 下行 | ✅ |
| 8 | 0x0003 | 终端注销 | 上行 | ✅ |
| 9 | 0x0102 | 终端鉴权 | 上行 | ✅ |
| 10 | 0x8103 | 设置终端参数 | 下行 | ✅ |
| 11 | 0x8104 | 查询终端参数 | 下行 | ✅ |
| 12 | 0x0104 | 查询终端参数应答 | 上行 | ✅ |
| 13 | 0x8105 | 终端控制 | 下行 | ✅ |
| 14 | 0x8106 | 查询指定终端参数 | 下行 | ✅ |
| 15 | 0x8107 | 查询终端属性 | 下行 | ✅ |
| 16 | 0x0107 | 查询终端属性应答 | 上行 | ✅ |
| 17 | 0x8108 | 下发终端升级包 | 下行 | ✅ |
| 18 | 0x0108 | 终端升级结果通知 | 上行 | ✅ |
| 19 | 0x0200 | 位置信息汇报 | 上行 | ✅ |
| 20 | 0x8201 | 位置信息查询 | 下行 | ✅ |
| 21 | 0x0201 | 位置信息查询应答 | 上行 | ✅ |
| 22 | 0x8202 | 临时位置跟踪控制 | 下行 | ✅ |
| 23 | 0x8203 | 人工确认报警消息 | 下行 | ✅ |
| 24 | 0x8300 | 文本信息下发 | 下行 | ✅ |
| 25 | 0x8301 | 事件设置 | 下行 | ✅ |
| 26 | 0x0301 | 事件报告 | 上行 | ✅ |
| 27 | 0x8302 | 提问下发 | 下行 | ✅ |
| 28 | 0x0302 | 提问应答 | 上行 | ✅ |
| 29 | 0x8303 | 信息点播菜单设置 | 下行 | ✅ |
| 30 | 0x0303 | 信息点播/取消 | 上行 | ✅ |
| 31 | 0x8304 | 信息服务 | 下行 | ✅ |
| 32 | 0x8400 | 电话回拨 | 下行 | ✅ |
| 33 | 0x8401 | 设置电话本 | 下行 | ✅ |
| 34 | 0x8500 | 车辆控制 | 下行 | ✅ |
| 35 | 0x0500 | 车辆控制应答 | 上行 | ✅ |
| 36 | 0x8600 | 设置圆形区域 | 下行 | ✅ |
| 37 | 0x8601 | 删除圆形区域 | 下行 | ✅ |
| 38 | 0x8602 | 设置矩形区域 | 下行 | ✅ |
| 39 | 0x8603 | 删除矩形区域 | 下行 | ✅ |
| 40 | 0x8604 | 设置多边形区域 | 下行 | ✅ |
| 41 | 0x8605 | 删除多边形区域 | 下行 | ✅ |
| 42 | 0x8606 | 设置路线 | 下行 | ✅ |
| 43 | 0x8607 | 删除路线 | 下行 | ✅ |
| 44 | 0x8608 | 查询区域或线路数据 | 下行 | ✅ |
| 45 | 0x0608 | 查询区域或线路数据应答 | 上行 | ✅ |
| 46 | 0x8700 | 行驶记录仪数据采集命令 | 下行 | ✅ |
| 47 | 0x0700 | 行驶记录仪数据上传 | 上行 | ✅ |
| 48 | 0x8701 | 行驶记录仪参数下传命令 | 下行 | ✅ |
| 49 | 0x0701 | 电子运单上报 | 上行 | ✅ |
| 50 | 0x8702 | 上报驾驶员身份信息请求 | 下行 | ✅ |
| 51 | 0x0702 | 驾驶员身份信息采集上报 | 上行 | ✅ |
| 52 | 0x0704 | 定位数据批量上传 | 上行 | ✅ |
| 53 | 0x0705 | CAN总线数据上传 | 上行 | ✅ |
| 54 | 0x0800 | 多媒体事件信息上传 | 上行 | ✅ |
| 55 | 0x0801 | 多媒体数据上传 | 上行 | ✅ |
| 56 | 0x8800 | 多媒体数据上传应答 | 下行 | ✅ |
| 57 | 0x8801 | 摄像头立即拍摄命令 | 下行 | ✅ |
| 58 | 0x0805 | 摄像头立即拍摄命令应答 | 上行 | ✅ |
| 59 | 0x8802 | 存储多媒体数据检索 | 下行 | ✅ |
| 60 | 0x0802 | 存储多媒体数据检索应答 | 上行 | ✅ |
| 61 | 0x8803 | 存储多媒体数据上传 | 下行 | ✅ |
| 62 | 0x8804 | 录音开始命令 | 下行 | ✅ |
| 63 | 0x8805 | 单条存储多媒体数据检索上传 | 下行 | ✅ |
| 64 | 0x8900 | 数据下行透传 | 下行 | ✅ |
| 65 | 0x0900 | 数据上行透传 | 上行 | ✅ |
| 66 | 0x0901 | 数据压缩上报 | 上行 | ✅ |
| 67 | 0x8A00 | 平台RSA公钥 | 下行 | ✅ |
| 68 | 0x0005 | 终端补传分包请求 | 上行 | ✅ |
| 69 | 0x8204 | 链路检测 | 下行 | ✅ |
| 70 | 0x6006 | 文本信息下发响应 | 上行 | ✅ |
| 71 | 0x1080 | 自定义消息 | — | ✅ |

## 快速入门

### 安装 NuGet 包

```bash
dotnet add package NewLife.JT808
```

### 解析终端上报消息

```csharp
using NewLife.JT808.Protocols;
using NewLife.JT808.Models;

// 初始化消息工厂（扫描程序集注册所有消息类型）
var factory = MessageFactory.Instance;
factory.Init();

// 终端上报的原始二进制数据
var raw = new Byte[] { 0x7E, 0x02, 0x00, /* ... */, 0x7E };

// 解析为 JTMessage 帧
var jtMessage = new JTMessage();
var stream = new MemoryStream(raw);
jtMessage.Read(stream, null);

// 根据消息ID反序列化为具体业务模型
var body = factory.Parse(jtMessage);
if (body is T0200 position)
{
    Console.WriteLine($"纬度={position.Latitude / 1_000_000.0}");
    Console.WriteLine($"经度={position.Longitude / 1_000_000.0}");
    Console.WriteLine($"速度={position.Speed} km/h");
}
```

### 构建下发消息

```csharp
// 构建平台通用应答
var t8001 = new T8001
{
    Sequence = jtMessage.Sequence,
    Kind = jtMessage.Kind,
    Result = ResultKinds.Success
};

// 序列化为字节数组
var ms = new MemoryStream();
var writer = Helper.CreateWriter(ms, jtMessage);
writer.TryWrite(null, ref t8001);

// 封装为 JTMessage 帧
var reply = new JTMessage
{
    Kind = MessageKinds.平台通用应答,
    Mobile = jtMessage.Mobile,
    Sequence = (UInt16)(jtMessage.Sequence + 1),
    Payload = new ArrayPacket(ms.ToArray()),
    Reply = false
};

// 输出完整帧字节
var outMs = new MemoryStream();
reply.Write(outMs, null);
var frameBytes = outMs.ToArray();
```

### 手动序列化复杂消息体（IAccessor）

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
        // ... 更多字段
        return true;
    }

    public Boolean Write(Stream stream, Object context)
    {
        var writer = context as Binary ?? Helper.CreateWriter(stream);
        writer.Write((UInt32)Alarm);
        writer.Write((UInt32)Status);
        writer.Write(Latitude);
        writer.Write(Longitude);
        // ... 更多字段
        return true;
    }
}
```

## 多语言

库内置 11 种运行时语言（简繁中文、英文、西班牙语、德语、日语、法语、葡萄牙语、俄语、韩语与阿拉伯语）。消息以英文源文为键，缺少译文时自动回退英文。

```csharp
using System.Globalization;
using NewLife;

// 默认跟随进程 UI 文化，也可显式固定
Localization.Culture = CultureInfo.GetCultureInfo("ja");

// ASP.NET Core 宿主可按 Accept-Language 识别请求语言
using var scope = RequestCultureScope.Create(Request.Headers.AcceptLanguage);
```

### NativeAOT

`MessageFactory.Init()` 通过扫描程序集发现消息体，NativeAOT 或裁剪发布的应用需要保留协议库程序集：

```xml
<ItemGroup>
  <TrimmerRootAssembly Include="NewLife.JT808" />
</ItemGroup>
```

仓库内的 `AotSmoke` 工程以 NativeAOT 发布，并在 Windows/Linux（x64 与 arm64）运行 16 个冒烟场景。

## 生态版图

NewLife 生态按依赖分层，各项目独立版本、独立发版。下面的版图在所有 NewLife 仓库中逐字一致；每个项目链接到代码仓库与主 NuGet 包。

| 项目 | 一句话定位 | 下载量 |
| --- | --- | --- |
| **基础底座** | | |
| [NewLife.Core](https://github.com/NewLifeX/X) | 日志、配置、缓存、网络、序列化、定时调度与 APM 追踪。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Core?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Core) |
| [NewLife.XCode](https://github.com/NewLifeX/NewLife.XCode) | 数据中间件：单表百亿级、自动分表、读写分离，支持 10 多种数据库。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.XCode?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.XCode) |
| [NewLife.Net](https://github.com/NewLifeX/NewLife.Net) | 高性能网络库：单机 177 万 pps、14.5 Gbps（TCP 1KB，2026-10 实测），400 万 Tcp 长连接。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Net?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Net) |
| [NewLife.Remoting](https://github.com/NewLifeX/NewLife.Remoting) | 协议通信与 RPC 框架，面向 CS 应用与物联网设备。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Remoting?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Remoting) |
| [NewLife.Agent](https://github.com/NewLifeX/NewLife.Agent) | 把 .NET 应用安装为操作系统守护进程：Windows 服务与 Linux systemd。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Agent?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Agent) |
| [NewLife.Templates](https://github.com/NewLifeX/NewLife.Zero) | `dotnet new` 项目模板：控制台、Web、WebApi、服务、数据与物联网。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Templates?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Templates) |
| **数据存储与消息** | | |
| [NewLife.Redis](https://github.com/NewLifeX/NewLife.Redis) | Redis 客户端：微秒级延迟、百万级吞吐，内置消息队列。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Redis?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Redis) |
| [NewLife.RocketMQ](https://github.com/NewLifeX/NewLife.RocketMQ) | 纯托管 RocketMQ 客户端，支持 Apache RocketMQ 与阿里云消息队列。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.RocketMQ?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.RocketMQ) |
| [NewLife.MQTT](https://github.com/NewLifeX/NewLife.MQTT) | MQTT 客户端与服务端，客户端支持阿里云物联网。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.MQTT?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.MQTT) |
| [NewLife.NovaDb](https://github.com/NewLifeX/NewLife.NovaDb) | C# 混合数据库：嵌入/网络双模，关系型、时序、消息队列与 KV 于一体。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.NovaDb?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.NovaDb) |
| [NewLife.MySql](https://github.com/NewLifeX/NewLife.MySql) | MySql 驱动与数据访问辅助层，基于 NewLife 数据栈实现。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.MySql?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.MySql) |
| **Web、AI 与分布式** | | |
| [NewLife.Cube](https://github.com/NewLifeX/NewLife.Cube) | 魔方快速开发平台：用户权限、SSO、OAuth 服务端与 CRUD 脚手架。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Cube?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Cube) |
| [Stardust](https://github.com/NewLifeX/Stardust) | 星尘分布式服务平台：节点管理、APM、配置中心、注册中心与发布中心。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Stardust?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Stardust) |
| [AntJob](https://github.com/NewLifeX/AntJob) | 蚂蚁调度：分布式任务与实时数据平台，支持分片、重试与回溯补算。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.AntJob?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.AntJob) |
| [NewLife.AI](https://github.com/NewLifeX/NewLife.AI) | 统一接入 46 家大模型服务商，附带开箱即用的 ChatAI Web 对话应用。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.AI?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.AI) |
| **工业与物联网** | | |
| [NewLife.IoT](https://github.com/NewLifeX/NewLife.IoT) | IoT 标准库，定义物联网通信协议契约与设备抽象。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.IoT?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.IoT) |
| [NewLife.Modbus](https://github.com/NewLifeX/NewLife.Modbus) | Modbus TCP/RTU/ASCII，基于 NewLife.IoT 实现。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Modbus?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Modbus) |
| [NewLife.Siemens](https://github.com/NewLifeX/NewLife.Siemens) | 西门子 PLC 协议，基于 NewLife.IoT 实现。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Siemens?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Siemens) |
| [NewLife.Omron](https://github.com/NewLifeX/NewLife.Omron) | 欧姆龙 PLC 协议，基于 NewLife.IoT 实现。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Omron?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Omron) |
| [NewLife.Melsec](https://github.com/NewLifeX/NewLife.Melsec) | 三菱 Melsec PLC 协议，基于 NewLife.IoT 实现。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Melsec?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Melsec) |
| [NewLife.OPC](https://github.com/NewLifeX/NewLife.OPC) | OPC 客户端，用于工业数据采集。 | — |
| [NewLife.BACnet](https://github.com/NewLifeX/NewLife.BACnet) | BACnet 楼宇自控协议栈。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.BACnet?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.BACnet) |
| [ZeroIoT](https://github.com/NewLifeX/ZeroIoT) | 开源轻量级 IoT 平台，含网关与设备管理。 | — |
| **车联网** | | |
| NewLife.JT808 | JT/T 808 车辆定位协议：70+ 消息体，覆盖 2011/2013/2019 三个版本。 | ![NuGet](https://img.shields.io/nuget/dt/NewLife.JT808?style=flat-square&logo=nuget&label=downloads) |
| NewLife.JT809 | JT/T 809 部标平台互联协议，面向运输管理部门。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.JT809?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.JT809) |
| NewLife.JT1078 | JT/T 1078 车载视频：RTP 解析、FLV/fMP4 转封装与直播推送。 | ![NuGet](https://img.shields.io/nuget/dt/NewLife.JT1078?style=flat-square&logo=nuget&label=downloads) |
| **领域组件** | | |
| [NewLife.Office](https://github.com/NewLifeX/NewLife.Office) | 一个包覆盖 Excel、Word、PPT、PDF 等 15+ 文档格式。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Office?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Office) |
| [NewLife.Audio](https://github.com/NewLifeX/NewLife.Audio) | 跨平台音频：编解码、设备 IO、DSP、容器与流式处理。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Audio?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Audio) |
| [NewLife.Video](https://github.com/NewLifeX/NewLife.Video) | 视频编解码、封装格式、帧处理与流媒体。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Video?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Video) |
| [NewLife.Map](https://github.com/NewLifeX/NewLife.Map) | 百度/高德/腾讯/天地图统一接口，支持地理编码与坐标转换。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Map?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Map) |
| [NewLife.IP](https://github.com/NewLifeX/NewLife.IP) | IPv4 归属地：离线内存映射库 + 在线 API 双源。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.IP?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.IP) |
| **工具与产品** | | |
| [CrazyCoder](https://github.com/NewLifeX/XCoder) | 码神工具：网络、串口、加解密、正则、Modbus 与 MQTT。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.XCoder?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.XCoder) |
| [GitCandy](https://github.com/NewLifeX/GitCandy) | 自托管 Git 源代码管理系统。 | — |
| [LuckyClover](https://github.com/NewLifeX/LuckyClover) | 从低版本宿主启动时，自动安装匹配的 .NET 运行时。 | — |

> 徽章取各项目主包；主题包、扩展包与卫星包见对应 NuGet 页面。JT808/JT1078 正在发布准备中，包上线后徽章自动显示下载量。未在表中列出的 30 多个包（FTP、LoRa、Office 扩展、各类协议驱动等）见 [NewLifeX 组织](https://github.com/NewLifeX) 与 [newlifex.com](https://newlifex.com)。

## 新生命开发团队

<img src="https://newlifex.com/logo.png" width="48" align="right" alt="NewLife" />

新生命团队成立于 2002 年，二十余年专注 .NET 与物联网技术生态，主导开源项目近百个（生态版图收录 34 个主干项目），为电力、物流、医疗、工控、互联网等行业提供软硬件一体化解决方案；以“学无先后达者为师”为信条。

开源组件在 NuGet 累计下载 <!--nuget-total:start-->6.48M<!--nuget-total:end--> 次（全生态口径，截至 2026-10）。

网站：https://newlifex.com · 开源组织：https://github.com/NewLifeX

<details>
<summary>微信公众号：智能大石头（扫码关注）</summary>

![智能大石头](https://newlifex.com/stone.jpg)

</details>
