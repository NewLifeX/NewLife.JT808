# NewLife.JT808 - JT/T 808 車輛定位協定

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
🌐 [English](README.md) | [简体中文](README.zh-CN.md) | [Español](README.es.md) | [Deutsch](README.de.md) | [日本語](README.ja.md) | [Français](README.fr.md) | [Português](README.pt.md) | [Русский](README.ru.md) | [한국어](README.ko.md) | [العربية](README.ar.md)
<!-- lang:end -->

> 本翻譯可能落後，請以 [English README](README.md) 為準。

完整實作 JT/T 808 車輛定位協定，涵蓋 2011/2013/2019 三個版本、70+ 標準訊息內容、蘇標（江蘇）與粵標（廣東）主動安全擴充、JT/T 1078 音視訊訊息、JT/T 19056 行車記錄器協定，以及訊息佇列整合。

原始碼：https://github.com/NewLifeX/NewLife.JT808  
NuGet：[NewLife.JT808](https://www.nuget.org/packages/NewLife.JT808)  
文件：[需求文件](Doc/需求文档.md) / [架構設計](Doc/架构设计.md) / [功能清單](Doc/功能清单.md) / [使用文件](Doc/使用文档.md) / [多語言本地化](Doc/多语言本地化.md)

## 主要特點

- **協定完整**：JT/T 808-2011/2013/2019 框架解析，支援 0x7E 邊界、0x7D 轉義、XOR 檢查碼與分包重組
- **訊息內容豐富**：70+ 標準訊息內容（0x0001~0x8A00），涵蓋終端上行與平台下行
- **三版本相容**：透過 `Version` 欄位與 `[JT2011]`/`[JT2019]` 特性自動適配欄位差異
- **序列化雙模式**：簡單訊息採特性驅動反射，複雜訊息實作 `IAccessor` 手動控制
- **TLV/KLV 解析**：內建 Type-Length-Value 與 Kind-Length-Value 編解碼，支援強型別字典轉換
- **網路層**：TCP 黏包編解碼器 `JTCodec` 與 `JT808Client`，自動路由處理器
- **蘇標擴充**：ADAS/DSM/TPMS/BSD 主動安全警報、周邊設備狀態與附件協定
- **粵標擴充**：廣東主動安全標準，專用警報回報（0x1FC4）、USB 透傳、位置擴充與廠商參數
- **JT/T 1078 基礎**：音視訊訊息內容模型與列舉
- **行車記錄器**：JT/T 19056 上行與下行封包編解碼
- **BCD 編碼**：BCD 字串與時間戳的自訂序列化特性
- **GBK 編碼**：內建 GBK 字串編解碼，自動註冊 `CodePagesEncodingProvider`
- **Web API 管理**：統一命令下發、線上工作階段管理、黑名單與權杖驗證
- **訊息佇列抽象**：`IMessageBus` 建立生產者/消費者、工作階段事件與下行處理器；`RedisMessageBus` 為內建內部傳輸，`JT808.Server` 中的 RocketMQ/Kafka 轉接器透過 `JT808:Messaging` 設定開關接入
- **叢集命令下發**：`ClusterState` 透過 Redis 共享線上工作階段、節點心跳與黑名單，將命令路由到終端所屬節點，並保留 `ICommandBus` 作為自訂傳輸的擴充點
- **用戶端強化**：自動重連、可設定的心跳與收發計數器
- **多框架支援**：`net45 / net461 / netstandard2.0 / netstandard2.1 / net8.0`
- **AOT 友善**：NativeAOT 冒煙專案涵蓋編解碼、工廠與序列化路徑
- **多語言**：內建 11 種執行階段語言，英文原文作為後備，資料表 AOT 安全
- **RocketMQ 整合**：`JT808.Server` 專案中的生產者/消費者轉接器（不隨 NuGet 套件發行），支援阿里雲、華為雲、騰訊雲與 ACL 驗證
- **Kafka 整合**：`JT808.Server` 專案中的生產者/消費者轉接器（不隨 NuGet 套件發行），基於 `Confluent.Kafka`，支援消費者群組與 ACK

## 支援的協定版本

| 版本 | 標識 | 說明 |
|------|------|------|
| JT/T 808-2011 | `Version=0` | 早期版本，部分訊息結構不同 |
| JT/T 808-2013 | `Version=0` | 目前最廣泛部署的版本 |
| JT/T 808-2019 | `Version=1` | 最新版本，新增欄位與訊息類型 |

## 訊息矩陣

| # | 訊息 ID | 訊息名稱 | 方向 | 完成 |
|:---:|:------:|---------|:----:|:----:|
| 1 | 0x0001 | 終端一般應答 | 上行 | ✅ |
| 2 | 0x8001 | 平台一般應答 | 下行 | ✅ |
| 3 | 0x0002 | 終端心跳 | 上行 | ✅ |
| 4 | 0x8003 | 重傳分包請求 | 下行 | ✅ |
| 5 | 0x0004 | 查詢伺服器時間請求 | 上行 | ✅ |
| 6 | 0x0100 | 終端註冊 | 上行 | ✅ |
| 7 | 0x8100 | 終端註冊應答 | 下行 | ✅ |
| 8 | 0x0003 | 終端登出 | 上行 | ✅ |
| 9 | 0x0102 | 終端鑑權 | 上行 | ✅ |
| 10 | 0x8103 | 設定終端參數 | 下行 | ✅ |
| 11 | 0x8104 | 查詢終端參數 | 下行 | ✅ |
| 12 | 0x0104 | 查詢終端參數應答 | 上行 | ✅ |
| 13 | 0x8105 | 終端控制 | 下行 | ✅ |
| 14 | 0x8106 | 查詢指定終端參數 | 下行 | ✅ |
| 15 | 0x8107 | 查詢終端屬性 | 下行 | ✅ |
| 16 | 0x0107 | 查詢終端屬性應答 | 上行 | ✅ |
| 17 | 0x8108 | 下發終端升級包 | 下行 | ✅ |
| 18 | 0x0108 | 終端升級結果通知 | 上行 | ✅ |
| 19 | 0x0200 | 位置資訊回報 | 上行 | ✅ |
| 20 | 0x8201 | 位置查詢 | 下行 | ✅ |
| 21 | 0x0201 | 位置查詢應答 | 上行 | ✅ |
| 22 | 0x8202 | 臨時位置追蹤控制 | 下行 | ✅ |
| 23 | 0x8203 | 人工確認警報 | 下行 | ✅ |
| 24 | 0x8300 | 下發文字訊息 | 下行 | ✅ |
| 25 | 0x8301 | 事件設定 | 下行 | ✅ |
| 26 | 0x0301 | 事件回報 | 上行 | ✅ |
| 27 | 0x8302 | 下發提問 | 下行 | ✅ |
| 28 | 0x0302 | 提問應答 | 上行 | ✅ |
| 29 | 0x8303 | 資訊點播選單設定 | 下行 | ✅ |
| 30 | 0x0303 | 資訊點播/取消 | 上行 | ✅ |
| 31 | 0x8304 | 資訊服務 | 下行 | ✅ |
| 32 | 0x8400 | 電話回撥 | 下行 | ✅ |
| 33 | 0x8401 | 設定電話簿 | 下行 | ✅ |
| 34 | 0x8500 | 車輛控制 | 下行 | ✅ |
| 35 | 0x0500 | 車輛控制應答 | 上行 | ✅ |
| 36 | 0x8600 | 設定圓形區域 | 下行 | ✅ |
| 37 | 0x8601 | 刪除圓形區域 | 下行 | ✅ |
| 38 | 0x8602 | 設定矩形區域 | 下行 | ✅ |
| 39 | 0x8603 | 刪除矩形區域 | 下行 | ✅ |
| 40 | 0x8604 | 設定多邊形區域 | 下行 | ✅ |
| 41 | 0x8605 | 刪除多邊形區域 | 下行 | ✅ |
| 42 | 0x8606 | 設定路線 | 下行 | ✅ |
| 43 | 0x8607 | 刪除路線 | 下行 | ✅ |
| 44 | 0x8608 | 查詢區域或路線資料 | 下行 | ✅ |
| 45 | 0x0608 | 查詢區域或路線資料應答 | 上行 | ✅ |
| 46 | 0x8700 | 行車記錄器資料採集命令 | 下行 | ✅ |
| 47 | 0x0700 | 行車記錄器資料上傳 | 上行 | ✅ |
| 48 | 0x8701 | 行車記錄器參數下發命令 | 下行 | ✅ |
| 49 | 0x0701 | 電子運單回報 | 上行 | ✅ |
| 50 | 0x8702 | 要求回報駕駛員身份 | 下行 | ✅ |
| 51 | 0x0702 | 駕駛員身份資訊回報 | 上行 | ✅ |
| 52 | 0x0704 | 批次位置資料上傳 | 上行 | ✅ |
| 53 | 0x0705 | CAN 匯流排資料上傳 | 上行 | ✅ |
| 54 | 0x0800 | 多媒體事件回報 | 上行 | ✅ |
| 55 | 0x0801 | 多媒體資料上傳 | 上行 | ✅ |
| 56 | 0x8800 | 多媒體資料上傳應答 | 下行 | ✅ |
| 57 | 0x8801 | 攝影機立即拍攝命令 | 下行 | ✅ |
| 58 | 0x0805 | 攝影機立即拍攝應答 | 上行 | ✅ |
| 59 | 0x8802 | 儲存多媒體資料搜尋 | 下行 | ✅ |
| 60 | 0x0802 | 儲存多媒體資料搜尋應答 | 上行 | ✅ |
| 61 | 0x8803 | 儲存多媒體資料上傳 | 下行 | ✅ |
| 62 | 0x8804 | 開始錄影命令 | 下行 | ✅ |
| 63 | 0x8805 | 單筆儲存多媒體資料擷取上傳 | 下行 | ✅ |
| 64 | 0x8900 | 下行透傳 | 下行 | ✅ |
| 65 | 0x0900 | 上行透傳 | 上行 | ✅ |
| 66 | 0x0901 | 壓縮資料回報 | 上行 | ✅ |
| 67 | 0x8A00 | 平台 RSA 公開金鑰 | 下行 | ✅ |
| 68 | 0x0005 | 終端重傳分包請求 | 上行 | ✅ |
| 69 | 0x8204 | 鏈路檢測 | 下行 | ✅ |
| 70 | 0x6006 | 文字訊息下發應答 | 上行 | ✅ |
| 71 | 0x1080 | 自訂訊息 | — | ✅ |

## 快速開始

### 安裝 NuGet 套件

```bash
dotnet add package NewLife.JT808
```

### 解析上行訊息

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

### 建構下行訊息

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

### 手動序列化複雜訊息（IAccessor）

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

### 多語言

本函式庫內建 11 種執行階段語言（簡體中文、繁體中文、英文、西班牙文、德文、日文、法文、葡萄牙文、俄文、韓文與阿拉伯文）。訊息以英文原文作為索引鍵，缺少翻譯時回退至英文原文。

```csharp
using System.Globalization;
using NewLife;

// Follow the process UI culture by default; pin it explicitly if needed
Localization.Culture = CultureInfo.GetCultureInfo("ja");

// ASP.NET Core hosts can resolve the request language from Accept-Language
using var scope = RequestCultureScope.Create(Request.Headers.AcceptLanguage);
```

### NativeAOT

`MessageFactory.Init()` 透過掃描協定組件探索訊息內容。NativeAOT 或裁剪（trimmed）應用程式必須保留該組件：

```xml
<ItemGroup>
  <TrimmerRootAssembly Include="NewLife.JT808" />
</ItemGroup>
```

本儲存庫中的 `AotSmoke` 專案以 NativeAOT 發佈，並在 Windows/Linux（x64 與 arm64）上執行 16 個情境。

## 生態版圖

NewLife 生態系按相依分層，各專案獨立版本、獨立發行。下面的版圖在所有 NewLife 儲存庫中逐字一致；每個專案連結到程式碼儲存庫與主 NuGet 套件。

| 專案 | 一句話定位 | 下載量 |
| --- | --- | --- |
| **基礎底座** | | |
| [NewLife.Core](https://github.com/NewLifeX/X) | 日誌、設定、快取、網路、序列化、排程與 APM 追蹤。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Core?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Core) |
| [NewLife.XCode](https://github.com/NewLifeX/NewLife.XCode) | 資料中介層：單表百億級、自動分表、讀寫分離，支援 10 多種資料庫。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.XCode?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.XCode) |
| [NewLife.Net](https://github.com/NewLifeX/NewLife.Net) | 高效能網路庫：單機 177 萬 pps、14.5 Gbps（TCP 1KB，2026-10 實測），400 萬 Tcp 長連線。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Net?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Net) |
| [NewLife.Remoting](https://github.com/NewLifeX/NewLife.Remoting) | 協定通訊與 RPC 框架，面向 CS 應用程式與物聯網裝置。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Remoting?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Remoting) |
| [NewLife.Agent](https://github.com/NewLifeX/NewLife.Agent) | 將 .NET 應用程式安裝為作業系統常駐服務：Windows 服務與 Linux systemd。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Agent?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Agent) |
| [NewLife.Templates](https://github.com/NewLifeX/NewLife.Zero) | `dotnet new` 專案範本：主控台、Web、API、服務、資料與物聯網。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Templates?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Templates) |
| **資料儲存與訊息** | | |
| [NewLife.Redis](https://github.com/NewLifeX/NewLife.Redis) | Redis 用戶端：微秒級延遲、百萬級吞吐，內建訊息佇列。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Redis?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Redis) |
| [NewLife.RocketMQ](https://github.com/NewLifeX/NewLife.RocketMQ) | 純受控 RocketMQ 用戶端，支援 Apache RocketMQ 與阿里雲訊息佇列。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.RocketMQ?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.RocketMQ) |
| [NewLife.MQTT](https://github.com/NewLifeX/NewLife.MQTT) | MQTT 用戶端與服務端，含阿里雲物聯網接入。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.MQTT?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.MQTT) |
| [NewLife.NovaDb](https://github.com/NewLifeX/NewLife.NovaDb) | C# 混合式資料庫：內嵌或網路雙模，關係型、時序、訊息佇列與 KV 於一體。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.NovaDb?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.NovaDb) |
| [NewLife.MySql](https://github.com/NewLifeX/NewLife.MySql) | 建構於 NewLife 資料堆疊之上的 MySql 驅動與輔助層。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.MySql?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.MySql) |
| **Web、AI 與分散式** | | |
| [NewLife.Cube](https://github.com/NewLifeX/NewLife.Cube) | 快速後台開發框架：權限、SSO、OAuth 服務端與 CRUD 樣板。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Cube?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Cube) |
| [Stardust](https://github.com/NewLifeX/Stardust) | 分散式服務平台：節點管理、APM、設定中心、註冊中心與發佈中心。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Stardust?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Stardust) |
| [AntJob](https://github.com/NewLifeX/AntJob) | 分散式任務與即時資料排程平台，支援分片、重試與回溯補算。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.AntJob?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.AntJob) |
| [NewLife.AI](https://github.com/NewLifeX/NewLife.AI) | 統一接入 46 家大模型服務商，附帶開箱即用的 ChatAI Web 對話應用。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.AI?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.AI) |
| **工業與物聯網** | | |
| [NewLife.IoT](https://github.com/NewLifeX/NewLife.IoT) | 物聯網標準庫，定義通訊協定契約與裝置抽象。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.IoT?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.IoT) |
| [NewLife.Modbus](https://github.com/NewLifeX/NewLife.Modbus) | 建構於 NewLife.IoT 之上的 Modbus TCP/RTU/ASCII 實作。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Modbus?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Modbus) |
| [NewLife.Siemens](https://github.com/NewLifeX/NewLife.Siemens) | 建構於 NewLife.IoT 之上的西門子 PLC 協定。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Siemens?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Siemens) |
| [NewLife.Omron](https://github.com/NewLifeX/NewLife.Omron) | 建構於 NewLife.IoT 之上的歐姆龍 PLC 協定。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Omron?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Omron) |
| [NewLife.Melsec](https://github.com/NewLifeX/NewLife.Melsec) | 建構於 NewLife.IoT 之上的三菱 Melsec PLC 協定。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Melsec?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Melsec) |
| [NewLife.OPC](https://github.com/NewLifeX/NewLife.OPC) | 工業資料存取的 OPC 用戶端。 | — |
| [NewLife.BACnet](https://github.com/NewLifeX/NewLife.BACnet) | BACnet 樓宇自動化協定堆疊。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.BACnet?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.BACnet) |
| [ZeroIoT](https://github.com/NewLifeX/ZeroIoT) | 開源輕量級物聯網平台，含閘道與裝置管理。 | — |
| **車聯網** | | |
| [NewLife.JT808](https://github.com/NewLifeX/NewLife.JT808) | JT/T 808 車輛定位協定：70+ 訊息內容，涵蓋 2011/2013/2019 三個版本。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.JT808?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.JT808) |
| [NewLife.JT809](https://github.com/NewLifeX/NewLife.JT809) | JT/T 809 部標平台互聯協定，面向運輸管理部門。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.JT809?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.JT809) |
| [NewLife.JT1078](https://github.com/NewLifeX/NewLife.JT1078) | JT/T 1078 車載視訊：RTP 解析、FLV/fMP4 轉封裝與直播推送。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.JT1078?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.JT1078) |
| **領域元件** | | |
| [NewLife.Office](https://github.com/NewLifeX/NewLife.Office) | 一個套件涵蓋 Excel、Word、PPT、PDF 等 15+ 文件格式。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Office?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Office) |
| [NewLife.Audio](https://github.com/NewLifeX/NewLife.Audio) | 跨平台音訊：編解碼、裝置 IO、DSP、容器與串流處理。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Audio?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Audio) |
| [NewLife.Video](https://github.com/NewLifeX/NewLife.Video) | 視訊編解碼、封裝格式、影格處理與串流。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Video?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Video) |
| [NewLife.Map](https://github.com/NewLifeX/NewLife.Map) | 百度／高德／騰訊／天地圖統一介面，支援地理編碼與座標轉換。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Map?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Map) |
| [NewLife.IP](https://github.com/NewLifeX/NewLife.IP) | IPv4 歸屬地：離線記憶體映射資料庫 + 線上 API 雙來源。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.IP?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.IP) |
| **工具與產品** | | |
| [CrazyCoder](https://github.com/NewLifeX/XCoder) | 開發者工具箱：網路、序列埠、加解密、正規表示式、Modbus 與 MQTT。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.XCoder?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.XCoder) |
| [GitCandy](https://github.com/NewLifeX/GitCandy) | 自架 Git 原始碼管理系統。 | — |
| [LuckyClover](https://github.com/NewLifeX/LuckyClover) | 從較舊的宿主啟動時，自動安裝相符的 .NET 執行階段。 | — |

> 徽章取各專案主套件；主題套件、擴充套件與衛星套件見對應 NuGet 頁面。未在表中列出的 30 多個套件（FTP、LoRa、Office 擴充、各類協定驅動等）見 [NewLifeX 組織](https://github.com/NewLifeX) 與 [newlifex.com](https://newlifex.com)。

## 新生命開發團隊

<img src="https://newlifex.com/logo.png" width="48" align="right" alt="NewLife" />

新生命團隊成立於 2002 年，二十餘年專注 .NET 與物聯網技術生態，主導開源專案近百個（生態版圖收錄 34 個主幹專案），為電力、物流、醫療、工控、網際網路等行業提供軟硬體一體化解決方案；以「學無先後達者為師」為信條。

開源元件在 NuGet 累計下載 <!--nuget-total:start-->6.48M<!--nuget-total:end--> 次（全生態口徑，截至 2026-10）。

網站：https://newlifex.com · 開源組織：https://github.com/NewLifeX

<details>
<summary>微信公眾號：智能大石头（掃碼關注）</summary>

![智能大石头](https://newlifex.com/stone.jpg)

</details>
