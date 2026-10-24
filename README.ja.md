# NewLife.JT808 - JT/T 808 車両位置情報プロトコル

![License: MIT](https://img.shields.io/badge/license-MIT-blue.svg?logo=github)
![NuGet Downloads](https://img.shields.io/nuget/dt/NewLife.JT808?logo=nuget)
![NuGet](https://img.shields.io/nuget/v/NewLife.JT808?logo=nuget)
![NuGet prerelease](https://img.shields.io/nuget/vpre/NewLife.JT808?label=dev%20nuget&logo=nuget)

[![aot](https://img.shields.io/github/actions/workflow/status/NewLifeX/NewLife.JT808/aot.yml?branch=master&label=aot&logo=dotnet)](https://github.com/NewLifeX/NewLife.JT808/actions/workflows/aot.yml)
[![test](https://img.shields.io/github/actions/workflow/status/NewLifeX/NewLife.JT808/test.yml?branch=master&label=test&logo=github)](https://github.com/NewLifeX/NewLife.JT808/actions/workflows/test.yml)

<!-- metrics:start -->
[![AOT smoke](https://img.shields.io/badge/AOT%20smoke-16%2F16-brightgreen?logo=dotnet)](AotSmoke/Program.cs)
[![TFM](https://img.shields.io/badge/TFM-5%20%28net45%20%E2%86%92%20net8.0%29-512BD4?logo=dotnet)](Newlife.JT808/NewLife.JT808.csproj)
[![locales](https://img.shields.io/badge/locales-11-2ea44f?logo=dotnet)](Doc/多语言本地化.md)
<!-- metrics:end -->
![platform](https://img.shields.io/badge/platform-Windows%20%7C%20Linux%20%7C%20macOS-blue)
[![contributors](https://img.shields.io/github/contributors/NewLifeX/NewLife.JT808?logo=github)](https://github.com/NewLifeX/NewLife.JT808/graphs/contributors)

<!-- lang:start -->
🌐 [English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [Español](README.es.md) | [Deutsch](README.de.md) | [Français](README.fr.md) | [Português](README.pt.md) | [Русский](README.ru.md) | [한국어](README.ko.md) | [العربية](README.ar.md)
<!-- lang:end -->

> 本翻訳は遅れる場合があります。[English README](README.md) を参照してください。

JT/T 808 車両位置情報プロトコルの完全実装です。2011/2013/2019 の各バージョンに対応し、70 以上の標準メッセージ本文、SuBiao（江蘇省）と YueBiao（広東省）の主動安全拡張、JT/T 1078 音声・映像メッセージ、JT/T 19056 ドライブレコーダープロトコル、メッセージキュー連携を提供します。

ソースコード： https://github.com/NewLifeX/NewLife.JT808  
NuGet：[NewLife.JT808](https://www.nuget.org/packages/NewLife.JT808)  
ドキュメント：[要件定義](Doc/需求文档.md) / [アーキテクチャ](Doc/架构设计.md) / [機能一覧](Doc/功能清单.md) / [ユーザーガイド](Doc/使用文档.md) / [ローカライズ](Doc/多语言本地化.md)

## 主な特長

- **プロトコル完全対応**：0x7E デリミタ、0x7D エスケープ、XOR チェックサム、サブパケット再結合を含む JT/T 808-2011/2013/2019 のフレーム解析
- **豊富なメッセージ本文**：端末の上りとプラットフォームの下りを網羅する 70 以上の標準メッセージ本文（0x0001〜0x8A00）
- **3 バージョン互換**：`Version` フィールドと `[JT2011]`/`[JT2019]` 属性によるフィールドの自動適応
- **2 つのシリアライズ方式**：単純な本文は属性駆動リフレクション、手動制御が必要な複雑な本文は `IAccessor`
- **TLV/KLV 解析**：Type-Length-Value と Kind-Length-Value のコーデックを内蔵し、強く型付けされた辞書変換に対応
- **ネットワーク層**：TCP 粘着パケットコーデック `JTCodec` と、ハンドラーの自動ルーティングを備えた `JT808Client`
- **SuBiao 拡張**：ADAS/DSM/TPMS/BSD 主動安全警報、周辺機器状態、添付ファイルプロトコル
- **YueBiao 拡張**：専用警報報告（0x1FC4）、USB パススルー、位置拡張情報、ベンダーパラメータを備えた広東省主動安全標準
- **JT/T 1078 基礎**：音声・映像メッセージ本文のモデルと列挙型
- **ドライブレコーダー**：JT/T 19056 の上り・下りデータパケットコーデック
- **BCD エンコーディング**：BCD 文字列とタイムスタンプ用のカスタムシリアライズ属性
- **GBK エンコーディング**：`CodePagesEncodingProvider` の自動登録を伴う GBK 文字列コーデックを内蔵
- **Web API 管理**：統合コマンド配信、オンラインセッション管理、ブラックリスト、トークン認証
- **メッセージキュー抽象化**：`IMessageBus` がプロデューサー／コンシューマー、セッションイベント、下りハンドラーを生成します。`RedisMessageBus` は内蔵の内部トランスポートであり、`JT808.Server` の RocketMQ/Kafka アダプターは `JT808:Messaging` 構成スイッチを通じて組み込まれます
- **クラスタコマンド配信**：`ClusterState` はオンラインセッション、ノードハートビート、ブラックリストを Redis 経由で共有し、端末を保有するノードへコマンドをルーティングします。`ICommandBus` はカスタムトランスポート用の拡張ポイントとして維持されます
- **クライアント堅牢化**：自動再接続、設定可能なハートビート、送受信カウンター
- **マルチフレームワーク対応**：`net45 / net461 / netstandard2.0 / netstandard2.1 / net8.0`
- **AOT 対応**：コーデック、ファクトリ、シリアライズの各経路を網羅する NativeAOT スモークプロジェクト
- **ローカライズ**：そのまま使える 11 言語の実行時ローカライズ、英語原文へのフォールバック、AOT セーフなテーブル
- **RocketMQ 連携**：`JT808.Server` プロジェクト内のプロデューサー／コンシューマーアダプター（NuGet パッケージには含まれません）。Alibaba Cloud、Huawei Cloud、Tencent Cloud、ACL 認証に対応
- **Kafka 連携**：`JT808.Server` プロジェクト内のプロデューサー／コンシューマーアダプター（NuGet パッケージには含まれません）。`Confluent.Kafka` をベースに、コンシューマーグループと ACK に対応

## 対応プロトコルバージョン

| バージョン | 識別子 | 説明 |
|---------|------|-------------|
| JT/T 808-2011 | `Version=0` | 初期バージョン。一部のメッセージレイアウトが異なる |
| JT/T 808-2013 | `Version=0` | 現在最も広く導入されているバージョン |
| JT/T 808-2019 | `Version=1` | 最新バージョン。新しいフィールドとメッセージ種別を追加 |

## メッセージマトリクス

| # | メッセージ ID | 名称 | 方向 | 実装済み |
|:---:|:------:|---------|:----:|:----:|
| 1 | 0x0001 | 端末汎用応答 | 上り | ✅ |
| 2 | 0x8001 | プラットフォーム汎用応答 | 下り | ✅ |
| 3 | 0x0002 | 端末ハートビート | 上り | ✅ |
| 4 | 0x8003 | サブパケット再送要求 | 下り | ✅ |
| 5 | 0x0004 | サーバー時刻照会要求 | 上り | ✅ |
| 6 | 0x0100 | 端末登録 | 上り | ✅ |
| 7 | 0x8100 | 端末登録応答 | 下り | ✅ |
| 8 | 0x0003 | 端末登録解除 | 上り | ✅ |
| 9 | 0x0102 | 端末認証 | 上り | ✅ |
| 10 | 0x8103 | 端末パラメータ設定 | 下り | ✅ |
| 11 | 0x8104 | 端末パラメータ照会 | 下り | ✅ |
| 12 | 0x0104 | 端末パラメータ照会応答 | 上り | ✅ |
| 13 | 0x8105 | 端末制御 | 下り | ✅ |
| 14 | 0x8106 | 指定端末パラメータ照会 | 下り | ✅ |
| 15 | 0x8107 | 端末属性照会 | 下り | ✅ |
| 16 | 0x0107 | 端末属性照会応答 | 上り | ✅ |
| 17 | 0x8108 | 端末アップグレードパッケージ配信 | 下り | ✅ |
| 18 | 0x0108 | 端末アップグレード結果通知 | 上り | ✅ |
| 19 | 0x0200 | 位置情報報告 | 上り | ✅ |
| 20 | 0x8201 | 位置情報照会 | 下り | ✅ |
| 21 | 0x0201 | 位置情報照会応答 | 上り | ✅ |
| 22 | 0x8202 | 一時位置追跡制御 | 下り | ✅ |
| 23 | 0x8203 | 警報イベントの手動確認 | 下り | ✅ |
| 24 | 0x8300 | テキストメッセージ配信 | 下り | ✅ |
| 25 | 0x8301 | イベント設定 | 下り | ✅ |
| 26 | 0x0301 | イベント報告 | 上り | ✅ |
| 27 | 0x8302 | 質問配信 | 下り | ✅ |
| 28 | 0x0302 | 質問応答 | 上り | ✅ |
| 29 | 0x8303 | 情報オンデマンドメニュー設定 | 下り | ✅ |
| 30 | 0x0303 | 情報オンデマンド／キャンセル | 上り | ✅ |
| 31 | 0x8304 | 情報サービス | 下り | ✅ |
| 32 | 0x8400 | 電話コールバック | 下り | ✅ |
| 33 | 0x8401 | 電話帳設定 | 下り | ✅ |
| 34 | 0x8500 | 車両制御 | 下り | ✅ |
| 35 | 0x0500 | 車両制御応答 | 上り | ✅ |
| 36 | 0x8600 | 円形エリア設定 | 下り | ✅ |
| 37 | 0x8601 | 円形エリア削除 | 下り | ✅ |
| 38 | 0x8602 | 矩形エリア設定 | 下り | ✅ |
| 39 | 0x8603 | 矩形エリア削除 | 下り | ✅ |
| 40 | 0x8604 | 多角形エリア設定 | 下り | ✅ |
| 41 | 0x8605 | 多角形エリア削除 | 下り | ✅ |
| 42 | 0x8606 | ルート設定 | 下り | ✅ |
| 43 | 0x8607 | ルート削除 | 下り | ✅ |
| 44 | 0x8608 | エリアまたはルートデータの照会 | 下り | ✅ |
| 45 | 0x0608 | エリアまたはルートデータの照会応答 | 上り | ✅ |
| 46 | 0x8700 | ドライブレコーダーデータ収集コマンド | 下り | ✅ |
| 47 | 0x0700 | ドライブレコーダーデータアップロード | 上り | ✅ |
| 48 | 0x8701 | ドライブレコーダーパラメータ配信コマンド | 下り | ✅ |
| 49 | 0x0701 | 電子運送伝票の報告 | 上り | ✅ |
| 50 | 0x8702 | 運転者身元情報の報告要求 | 下り | ✅ |
| 51 | 0x0702 | 運転者身元情報の報告 | 上り | ✅ |
| 52 | 0x0704 | 位置データの一括アップロード | 上り | ✅ |
| 53 | 0x0705 | CAN バスデータのアップロード | 上り | ✅ |
| 54 | 0x0800 | マルチメディアイベント報告 | 上り | ✅ |
| 55 | 0x0801 | マルチメディアデータのアップロード | 上り | ✅ |
| 56 | 0x8800 | マルチメディアデータのアップロード応答 | 下り | ✅ |
| 57 | 0x8801 | カメラ即時撮影コマンド | 下り | ✅ |
| 58 | 0x0805 | カメラ即時撮影コマンド応答 | 上り | ✅ |
| 59 | 0x8802 | 保存済みマルチメディアデータの検索 | 下り | ✅ |
| 60 | 0x0802 | 保存済みマルチメディアデータの検索応答 | 上り | ✅ |
| 61 | 0x8803 | 保存済みマルチメディアデータのアップロード | 下り | ✅ |
| 62 | 0x8804 | 録音開始コマンド | 下り | ✅ |
| 63 | 0x8805 | 単一保存マルチメディアデータの検索アップロード | 下り | ✅ |
| 64 | 0x8900 | 下りデータパススルー | 下り | ✅ |
| 65 | 0x0900 | 上りデータパススルー | 上り | ✅ |
| 66 | 0x0901 | 圧縮データの報告 | 上り | ✅ |
| 67 | 0x8A00 | プラットフォーム RSA 公開鍵 | 下り | ✅ |
| 68 | 0x0005 | 端末サブパケット再送要求 | 上り | ✅ |
| 69 | 0x8204 | リンク検出 | 下り | ✅ |
| 70 | 0x6006 | テキストメッセージ配信応答 | 上り | ✅ |
| 71 | 0x1080 | カスタムメッセージ | — | ✅ |

## クイックスタート

### NuGet パッケージのインストール

```bash
dotnet add package NewLife.JT808
```

### 上りメッセージの解析

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

### 下りメッセージの構築

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

### 複雑なメッセージ本文を手動でシリアライズする（IAccessor）

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

### ローカライズ

このライブラリは 11 言語（簡体字中国語／繁体字中国語、英語、スペイン語、ドイツ語、日本語、フランス語、ポルトガル語、ロシア語、韓国語、アラビア語）の実行時ローカライズを同梱しています。メッセージは英語の原文をキーとしており、翻訳が存在しない場合は英語の原文にフォールバックします。

```csharp
using System.Globalization;
using NewLife;

// Follow the process UI culture by default; pin it explicitly if needed
Localization.Culture = CultureInfo.GetCultureInfo("ja");

// ASP.NET Core hosts can resolve the request language from Accept-Language
using var scope = RequestCultureScope.Create(Request.Headers.AcceptLanguage);
```

### NativeAOT

`MessageFactory.Init()` はプロトコルアセンブリを走査してメッセージ本文を検出します。NativeAOT やトリミングを適用したアプリでは、そのアセンブリを保持する必要があります：

```xml
<ItemGroup>
  <TrimmerRootAssembly Include="NewLife.JT808" />
</ItemGroup>
```

このリポジトリの `AotSmoke` プロジェクトは NativeAOT で発行し、Windows/Linux（x64 と arm64）で 16 のシナリオを実行します。

## エコシステムマップ

NewLife は、独立したバージョン管理を行うオープンソースプロジェクトの階層型エコシステムです。以下のマップはすべての NewLife リポジトリで逐語的に同一の内容を共有しており、各プロジェクトはコードリポジトリと主要な NuGet パッケージにリンクしています。

| プロジェクト | 概要 | ダウンロード数 |
| --- | --- | --- |
| **基盤** | | |
| [NewLife.Core](https://github.com/NewLifeX/X) | ログ、構成、キャッシュ、ネットワーク、シリアライズ、スケジューリング、APM トレーシング。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Core?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Core) |
| [NewLife.XCode](https://github.com/NewLifeX/NewLife.XCode) | データミドルウェア：単一テーブルで数百億件級、自動シャーディング、読み書き分離、10 種類以上のデータベースに対応。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.XCode?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.XCode) |
| [NewLife.Net](https://github.com/NewLifeX/NewLife.Net) | 高性能ネットワークライブラリ：単一マシンで 177 万 pps、14.5 Gbps（TCP 1KB、2026-10 実測）、400 万 Tcp 長接続。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Net?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Net) |
| [NewLife.Remoting](https://github.com/NewLifeX/NewLife.Remoting) | CS アプリケーションと IoT デバイス向けのプロトコル通信と RPC フレームワーク。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Remoting?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Remoting) |
| [NewLife.Agent](https://github.com/NewLifeX/NewLife.Agent) | .NET アプリケーションを OS デーモンとしてインストール：Windows サービスと Linux systemd。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Agent?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Agent) |
| [NewLife.Templates](https://github.com/NewLifeX/NewLife.Zero) | コンソール、Web、API、サービス、データ、IoT 向けの `dotnet new` プロジェクトテンプレート。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Templates?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Templates) |
| **データとメッセージング** | | |
| [NewLife.Redis](https://github.com/NewLifeX/NewLife.Redis) | マイクロ秒級の遅延、数百万 ops/s のスループット、メッセージキュー内蔵の Redis クライアント。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Redis?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Redis) |
| [NewLife.RocketMQ](https://github.com/NewLifeX/NewLife.RocketMQ) | Apache RocketMQ と Alibaba Cloud Message Queue に対応した純マネージド RocketMQ クライアント。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.RocketMQ?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.RocketMQ) |
| [NewLife.MQTT](https://github.com/NewLifeX/NewLife.MQTT) | MQTT クライアントとサーバー。Alibaba Cloud IoT への接続を含みます。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.MQTT?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.MQTT) |
| [NewLife.NovaDb](https://github.com/NewLifeX/NewLife.NovaDb) | C# 製ハイブリッドデータベース：組み込み/ネットワークの両モード、リレーショナル、時系列、メッセージキュー、KV を 1 つに。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.NovaDb?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.NovaDb) |
| [NewLife.MySql](https://github.com/NewLifeX/NewLife.MySql) | NewLife データスタック上に構築された MySql ドライバーとヘルパー層。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.MySql?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.MySql) |
| **Web、AI、分散** | | |
| [NewLife.Cube](https://github.com/NewLifeX/NewLife.Cube) | 高速管理プラットフォームフレームワーク：権限、SSO、OAuth サーバー、CRUD スキャフォールディング。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Cube?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Cube) |
| [Stardust](https://github.com/NewLifeX/Stardust) | 分散サービスプラットフォーム：ノード管理、APM、構成センター、登録センター、リリースセンター。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Stardust?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Stardust) |
| [AntJob](https://github.com/NewLifeX/AntJob) | シャーディング、リトライ、バックフィルに対応した分散タスクとリアルタイムデータのスケジューリングプラットフォーム。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.AntJob?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.AntJob) |
| [NewLife.AI](https://github.com/NewLifeX/NewLife.AI) | 46 社の LLM プロバイダーに統一接続するクライアントと、すぐに使える Web チャットアプリ ChatAI。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.AI?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.AI) |
| **産業と IoT** | | |
| [NewLife.IoT](https://github.com/NewLifeX/NewLife.IoT) | プロトコル契約とデバイス抽象を定義する IoT 標準ライブラリ。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.IoT?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.IoT) |
| [NewLife.Modbus](https://github.com/NewLifeX/NewLife.Modbus) | NewLife.IoT 上に実装された Modbus TCP/RTU/ASCII。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Modbus?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Modbus) |
| [NewLife.Siemens](https://github.com/NewLifeX/NewLife.Siemens) | NewLife.IoT 上に実装されたシーメンス PLC プロトコル。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Siemens?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Siemens) |
| [NewLife.Omron](https://github.com/NewLifeX/NewLife.Omron) | NewLife.IoT 上に実装されたオムロン PLC プロトコル。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Omron?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Omron) |
| [NewLife.Melsec](https://github.com/NewLifeX/NewLife.Melsec) | NewLife.IoT 上に実装された三菱 Melsec PLC プロトコル。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Melsec?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Melsec) |
| [NewLife.OPC](https://github.com/NewLifeX/NewLife.OPC) | 産業データアクセス向けの OPC クライアント。 | — |
| [NewLife.BACnet](https://github.com/NewLifeX/NewLife.BACnet) | BACnet ビル管理プロトコルスタック。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.BACnet?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.BACnet) |
| [ZeroIoT](https://github.com/NewLifeX/ZeroIoT) | ゲートウェイとデバイス管理を備えた軽量なオープンソース IoT プラットフォーム。 | — |
| **車載テレマティクス** | | |
| [NewLife.JT808](https://github.com/NewLifeX/NewLife.JT808) | JT/T 808 車両位置情報プロトコル：70 以上のメッセージボディ、2011/2013/2019 に対応。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.JT808?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.JT808) |
| [NewLife.JT809](https://github.com/NewLifeX/NewLife.JT809) | 運輸管理部門向けの JT/T 809 プラットフォーム間プロトコル。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.JT809?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.JT809) |
| [NewLife.JT1078](https://github.com/NewLifeX/NewLife.JT1078) | JT/T 1078 車載ビデオ：RTP 解析、FLV/fMP4 リマックス、ライブ配信。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.JT1078?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.JT1078) |
| **ドメインコンポーネント** | | |
| [NewLife.Office](https://github.com/NewLifeX/NewLife.Office) | Excel、Word、PPT、PDF など 15 以上のドキュメント形式を 1 パッケージで。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Office?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Office) |
| [NewLife.Audio](https://github.com/NewLifeX/NewLife.Audio) | クロスプラットフォーム音声：コーデック、デバイス IO、DSP、コンテナ、ストリーミング。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Audio?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Audio) |
| [NewLife.Video](https://github.com/NewLifeX/NewLife.Video) | ビデオコーデック、コンテナ形式、フレーム処理、ストリーミング。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Video?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Video) |
| [NewLife.Map](https://github.com/NewLifeX/NewLife.Map) | 百度地図、高徳地図、テンセント地図、天地図を統一インターフェイス化。ジオコーディングと座標変換に対応。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Map?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Map) |
| [NewLife.IP](https://github.com/NewLifeX/NewLife.IP) | IPv4 位置情報：オフラインのメモリマップデータベースとオンライン API のフォールバック。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.IP?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.IP) |
| **ツールと製品** | | |
| [CrazyCoder](https://github.com/NewLifeX/XCoder) | 開発者向けツールボックス：ネットワーク、シリアルポート、暗号化、正規表現、Modbus、MQTT。 | [![NuGet](https://img.shields.io/nuget/dt/NewLife.XCoder?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.XCoder) |
| [GitCandy](https://github.com/NewLifeX/GitCandy) | セルフホスト型 Git ソースコード管理システム。 | — |
| [LuckyClover](https://github.com/NewLifeX/LuckyClover) | 古いホストから起動した際に対応する .NET ランタイムを自動インストールします。 | — |

> バッジは各プロジェクトの主要パッケージを示します。テーマ、拡張、サテライトパッケージは各 NuGet ページを参照してください。ここに記載していない 30 以上のパッケージ（FTP、LoRa、Office 拡張、各種プロトコルドライバーなど）は [NewLifeX 組織](https://github.com/NewLifeX) と [newlifex.com](https://newlifex.com) で公開しています。

## 新生命開発チーム

<img src="https://newlifex.com/logo.png" width="48" align="right" alt="NewLife" />

新生命チームは 2002 年に設立され、20 年以上にわたり .NET と IoT の技術エコシステムに注力し、100 近いオープンソースプロジェクトを主導し（うち 34 が主要プロジェクト）、電力、物流、医療、産業制御、インターネットなどの業界にソフトウェアとハードウェアを一体化したソリューションを提供しています。「学問に先後なし、達する者が師である」を信条としています。

オープンソースコンポーネントは NuGet で累計 <!--nuget-total:start-->6.48M<!--nuget-total:end--> 回ダウンロードされています（エコシステム全体、2026-10 時点）。

ウェブサイト：https://newlifex.com · オープンソース組織：https://github.com/NewLifeX

<details>
<summary>WeChat 公式アカウント：智能大石头（スキャンしてフォロー）</summary>

![智能大石头](https://newlifex.com/stone.jpg)

</details>

## ライセンス

本プロジェクトは [MIT](LICENSE) ライセンスで公開されています。著作権表示を保持する限り、自由に使用・改変・再配布できます。

> このプロジェクトが役に立ったら、Star やチームへの共有、Issue/PR での改善提案を歓迎します。
