# NewLife.JT808 - JT/T 808 車両位置情報プロトコル

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
🌐 [English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [Español](README.es.md) | [Deutsch](README.de.md) | [Français](README.fr.md) | [Português](README.pt.md) | [Русский](README.ru.md) | [한국어](README.ko.md) | [العربية](README.ar.md)
<!-- lang:end -->

> この翻訳は遅れている場合があります。[English README](README.md) を参照してください。

JT/T 808 車両位置情報プロトコルの完全実装です。2011/2013/2019 の各バージョンに対応し、70 以上の標準メッセージ本文、蘇標/粤標の主動安全拡張、JT/T 1078 音声・映像メッセージ、JT/T 19056 ドライブレコーダープロトコル、メッセージキュー連携を提供します。

## 主な特長

- **プロトコル完全対応**：0x7E デリミタ、0x7D エスケープ、XOR チェックサム、分割パケット再結合
- **豊富なメッセージ**：70 以上の標準メッセージ本文。上り・下りを網羅
- **3 バージョン互換**：`Version` と `[JT2011]`/`[JT2019]` によるフィールド自動適応
- **2 種類のシリアライズ**：属性駆動と `IAccessor` による手動制御
- **TLV/KLV**：汎用コーデックと強い型付き辞書変換
- **ネットワーク層**：`JTCodec` と `JT808Client` による自動ルーティング
- **AOT 対応**：NativeAOT スモークプロジェクトで主要経路を検証
- **多言語**：11 言語の実行時ローカライズ。英語原文にフォールバック

## 対応プロトコルバージョン

| バージョン | 識別子 | 説明 |
|-----------|--------|------|
| JT/T 808-2011 | `Version=0` | 初期バージョン。一部メッセージ構造が異なる |
| JT/T 808-2013 | `Version=0` | 現在最も広く使われているバージョン |
| JT/T 808-2019 | `Version=1` | 最新バージョン。新しいフィールドとメッセージを追加 |

## インストール

```bash
dotnet add package NewLife.JT808
```

## クイックスタート

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

## リンク

- ソースコード: https://github.com/NewLifeX/NewLife.JT808
- NuGet: https://www.nuget.org/packages/NewLife.JT808
- ドキュメント: [English](README.md) | [简体中文](README.zh-CN.md)

> NewLife Team · https://newlifex.com
