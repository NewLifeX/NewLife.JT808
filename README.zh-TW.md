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

<!-- lang:start -->
🌐 [English](README.md) | [简体中文](README.zh-CN.md) | [Español](README.es.md) | [Deutsch](README.de.md) | [日本語](README.ja.md) | [Français](README.fr.md) | [Português](README.pt.md) | [Русский](README.ru.md) | [한국어](README.ko.md) | [العربية](README.ar.md)
<!-- lang:end -->

> 本翻譯可能落後，以 [English README](README.md) 為準。

完整實作 JT/T 808 車輛定位協定，支援 2011/2013/2019 三個版本，涵蓋 70+ 標準訊息內容、蘇標/粵標主動安全擴充、JT/T 1078 音視訊訊息、JT/T 19056 行車記錄器協定與訊息佇列整合。

## 主要特點

- **協定完整**：0x7E 邊界、0x7D 轉義、異或檢查碼與分包處理
- **訊息豐富**：70+ 標準訊息內容，涵蓋終端上行與平台下行
- **三版本相容**：`Version` 欄位搭配 `[JT2011]`/`[JT2019]` 特性自動適配
- **序列化雙模式**：簡單訊息以特性驅動，複雜訊息實作 `IAccessor`
- **TLV/KLV**：通用編解碼並支援強型別字典轉換
- **網路層**：`JTCodec` 黏包編解碼器與 `JT808Client` 自動路由
- **AOT 相容**：NativeAOT 冒煙工程驗證核心路徑
- **多語言**：內建 11 種執行階段語言，英文原文作為後備

## 支援的協定版本

| 版本 | 標識 | 說明 |
|------|------|------|
| JT/T 808-2011 | `Version=0` | 早期版本，部分訊息結構不同 |
| JT/T 808-2013 | `Version=0` | 目前廣泛使用的版本 |
| JT/T 808-2019 | `Version=1` | 最新版本，新增欄位與訊息類型 |

## 安裝

```bash
dotnet add package NewLife.JT808
```

## 快速開始

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

## 連結

- 原始碼: https://github.com/NewLifeX/NewLife.JT808
- NuGet: https://www.nuget.org/packages/NewLife.JT808
- 文件: [English](README.md) | [简体中文](README.zh-CN.md)

> NewLife Team · https://newlifex.com
