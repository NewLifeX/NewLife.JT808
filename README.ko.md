# NewLife.JT808 - JT/T 808 차량 추적 프로토콜

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
🌐 [English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [Español](README.es.md) | [Deutsch](README.de.md) | [日本語](README.ja.md) | [Français](README.fr.md) | [Português](README.pt.md) | [Русский](README.ru.md) | [العربية](README.ar.md)
<!-- lang:end -->

> 이 번역은 지연될 수 있습니다. [English README](README.md)를 참조하세요.

JT/T 808 차량 추적 프로토콜의 완전한 구현으로, 2011/2013/2019 버전과 70개 이상의 표준 메시지 본문, SuBiao(장쑤) 및 YueBiao(광둥) 능동 안전 확장, JT/T 1078 오디오/비디오 메시지, JT/T 19056 주행 기록계 프로토콜, 그리고 메시지 큐 연동을 지원합니다.

소스: https://github.com/NewLifeX/NewLife.JT808  
NuGet: [NewLife.JT808](https://www.nuget.org/packages/NewLife.JT808)  
문서: [요구사항](Doc/需求文档.md) / [아키텍처](Doc/架构设计.md) / [기능 목록](Doc/功能清单.md) / [사용 가이드](Doc/使用文档.md) / [다국어](Doc/多语言本地化.md)

## 주요 기능

- **완전한 프로토콜**: 0x7E 구분자, 0x7D 이스케이프, XOR 체크섬, 하위 패킷 재조립을 포함한 JT/T 808-2011/2013/2019 프레임 파싱
- **풍부한 메시지 본문**: 터미널 상향과 플랫폼 하향을 모두 포괄하는 70개 이상의 표준 메시지 본문(0x0001~0x8A00)
- **세 가지 버전 호환**: `Version` 필드와 `[JT2011]`/`[JT2019]` 특성을 통한 자동 필드 적응
- **이중 직렬화 모드**: 단순한 본문은 특성 기반 리플렉션, 수동 제어가 필요한 복잡한 본문은 `IAccessor`
- **TLV/KLV 파싱**: 강력한 형식의 사전 변환을 갖춘 Type-Length-Value 및 Kind-Length-Value 코덱 내장
- **네트워크 계층**: TCP 점착 패킷 코덱 `JTCodec`과 자동 핸들러 라우팅을 갖춘 `JT808Client`
- **SuBiao 확장**: ADAS/DSM/TPMS/BSD 능동 안전 경보, 주변기기 상태 및 첨부 프로토콜
- **YueBiao 확장**: 전용 경보 보고(0x1FC4), USB 패스스루, 위치 확장, 벤더 파라미터를 포함한 광둥 능동 안전 표준
- **JT/T 1078 기초**: 오디오/비디오 메시지 본문 모델과 열거형
- **주행 기록계**: JT/T 19056 상향 및 하향 패킷 코덱
- **BCD 인코딩**: BCD 문자열과 타임스탬프를 위한 사용자 지정 직렬화 특성
- **GBK 인코딩**: `CodePagesEncodingProvider` 자동 등록을 포함한 GBK 문자열 코덱 내장
- **Web API 관리**: 통합 명령 전달, 온라인 세션 관리, 블랙리스트 및 토큰 인증
- **메시지 큐 추상화**: `IMessageBus`가 생산자/소비자, 세션 이벤트, 하향 핸들러를 생성하며, `RedisMessageBus`는 기본 내장 전송 수단이고 RocketMQ/Kafka 어댑터는 `JT808:Messaging` 구성 스위치를 통해 `JT808.Server`에 연결됩니다
- **클러스터 명령 전달**: `ClusterState`가 Redis를 통해 온라인 세션, 노드 하트비트, 블랙리스트를 공유하고 명령을 터미널을 보유한 노드로 라우팅하며, `ICommandBus`를 사용자 지정 전송을 위한 확장 지점으로 유지합니다
- **클라이언트 강화**: 자동 재연결, 구성 가능한 하트비트, 송수신 카운터
- **멀티 프레임워크 지원**: `net45 / net461 / netstandard2.0 / netstandard2.1 / net8.0`
- **AOT 친화적**: 코덱, 팩토리, 직렬화 경로를 커버하는 NativeAOT 스모크 프로젝트
- **다국어**: 기본 제공되는 런타임 11개 언어, 영어 원문 대체, AOT 안전 테이블
- **RocketMQ 연동**: `JT808.Server` 프로젝트의 생산자/소비자 어댑터(NuGet 패키지에 포함되지 않음), 알리바바 클라우드, 화웨이 클라우드, 텐센트 클라우드 및 ACL 인증 지원
- **Kafka 연동**: `JT808.Server` 프로젝트의 생산자/소비자 어댑터(NuGet 패키지에 포함되지 않음), 소비자 그룹과 ACK 지원을 갖춘 `Confluent.Kafka` 기반

## 지원 프로토콜 버전

| 버전 | 플래그 | 설명 |
|---------|------|-------------|
| JT/T 808-2011 | `Version=0` | 일부 메시지 구조가 다른 초기 버전 |
| JT/T 808-2013 | `Version=0` | 가장 널리 배포된 버전 |
| JT/T 808-2019 | `Version=1` | 새로운 필드와 메시지 유형이 추가된 최신 버전 |

## 메시지 매트릭스

| # | 메시지 ID | 이름 | 방향 | 완료 |
|:---:|:------:|---------|:----:|:----:|
| 1 | 0x0001 | 터미널 일반 응답 | 상향 | ✅ |
| 2 | 0x8001 | 플랫폼 일반 응답 | 하향 | ✅ |
| 3 | 0x0002 | 터미널 하트비트 | 상향 | ✅ |
| 4 | 0x8003 | 하위 패킷 재전송 요청 | 하향 | ✅ |
| 5 | 0x0004 | 서버 시간 조회 요청 | 상향 | ✅ |
| 6 | 0x0100 | 터미널 등록 | 상향 | ✅ |
| 7 | 0x8100 | 터미널 등록 응답 | 하향 | ✅ |
| 8 | 0x0003 | 터미널 등록 해제 | 상향 | ✅ |
| 9 | 0x0102 | 터미널 인증 | 상향 | ✅ |
| 10 | 0x8103 | 터미널 파라미터 설정 | 하향 | ✅ |
| 11 | 0x8104 | 터미널 파라미터 조회 | 하향 | ✅ |
| 12 | 0x0104 | 터미널 파라미터 조회 응답 | 상향 | ✅ |
| 13 | 0x8105 | 터미널 제어 | 하향 | ✅ |
| 14 | 0x8106 | 지정 터미널 파라미터 조회 | 하향 | ✅ |
| 15 | 0x8107 | 터미널 속성 조회 | 하향 | ✅ |
| 16 | 0x0107 | 터미널 속성 조회 응답 | 상향 | ✅ |
| 17 | 0x8108 | 터미널 업그레이드 패키지 전달 | 하향 | ✅ |
| 18 | 0x0108 | 터미널 업그레이드 결과 통지 | 상향 | ✅ |
| 19 | 0x0200 | 위치 정보 보고 | 상향 | ✅ |
| 20 | 0x8201 | 위치 조회 | 하향 | ✅ |
| 21 | 0x0201 | 위치 조회 응답 | 상향 | ✅ |
| 22 | 0x8202 | 임시 위치 추적 제어 | 하향 | ✅ |
| 23 | 0x8203 | 수동 경보 확인 | 하향 | ✅ |
| 24 | 0x8300 | 문자 메시지 전달 | 하향 | ✅ |
| 25 | 0x8301 | 이벤트 설정 | 하향 | ✅ |
| 26 | 0x0301 | 이벤트 보고 | 상향 | ✅ |
| 27 | 0x8302 | 질문 전달 | 하향 | ✅ |
| 28 | 0x0302 | 질문 응답 | 상향 | ✅ |
| 29 | 0x8303 | 정보 주문형 메뉴 설정 | 하향 | ✅ |
| 30 | 0x0303 | 정보 주문형 / 취소 | 상향 | ✅ |
| 31 | 0x8304 | 정보 서비스 | 하향 | ✅ |
| 32 | 0x8400 | 전화 콜백 | 하향 | ✅ |
| 33 | 0x8401 | 전화번호부 설정 | 하향 | ✅ |
| 34 | 0x8500 | 차량 제어 | 하향 | ✅ |
| 35 | 0x0500 | 차량 제어 응답 | 상향 | ✅ |
| 36 | 0x8600 | 원형 영역 설정 | 하향 | ✅ |
| 37 | 0x8601 | 원형 영역 삭제 | 하향 | ✅ |
| 38 | 0x8602 | 사각형 영역 설정 | 하향 | ✅ |
| 39 | 0x8603 | 사각형 영역 삭제 | 하향 | ✅ |
| 40 | 0x8604 | 다각형 영역 설정 | 하향 | ✅ |
| 41 | 0x8605 | 다각형 영역 삭제 | 하향 | ✅ |
| 42 | 0x8606 | 경로 설정 | 하향 | ✅ |
| 43 | 0x8607 | 경로 삭제 | 하향 | ✅ |
| 44 | 0x8608 | 영역 또는 경로 데이터 조회 | 하향 | ✅ |
| 45 | 0x0608 | 영역 또는 경로 데이터 조회 응답 | 상향 | ✅ |
| 46 | 0x8700 | 주행 기록계 데이터 수집 명령 | 하향 | ✅ |
| 47 | 0x0700 | 주행 기록계 데이터 업로드 | 상향 | ✅ |
| 48 | 0x8701 | 주행 기록계 파라미터 전달 명령 | 하향 | ✅ |
| 49 | 0x0701 | 전자 운송장 보고 | 상향 | ✅ |
| 50 | 0x8702 | 운전자 신원 보고 요청 | 하향 | ✅ |
| 51 | 0x0702 | 운전자 신원 정보 보고 | 상향 | ✅ |
| 52 | 0x0704 | 배치 위치 데이터 업로드 | 상향 | ✅ |
| 53 | 0x0705 | CAN 버스 데이터 업로드 | 상향 | ✅ |
| 54 | 0x0800 | 멀티미디어 이벤트 보고 | 상향 | ✅ |
| 55 | 0x0801 | 멀티미디어 데이터 업로드 | 상향 | ✅ |
| 56 | 0x8800 | 멀티미디어 데이터 업로드 응답 | 하향 | ✅ |
| 57 | 0x8801 | 카메라 즉시 촬영 명령 | 하향 | ✅ |
| 58 | 0x0805 | 카메라 즉시 촬영 응답 | 상향 | ✅ |
| 59 | 0x8802 | 저장된 멀티미디어 데이터 검색 | 하향 | ✅ |
| 60 | 0x0802 | 저장된 멀티미디어 데이터 검색 응답 | 상향 | ✅ |
| 61 | 0x8803 | 저장된 멀티미디어 데이터 업로드 | 하향 | ✅ |
| 62 | 0x8804 | 녹화 시작 명령 | 하향 | ✅ |
| 63 | 0x8805 | 단일 저장 멀티미디어 데이터 검색 업로드 | 하향 | ✅ |
| 64 | 0x8900 | 하향 패스스루 | 하향 | ✅ |
| 65 | 0x0900 | 상향 패스스루 | 상향 | ✅ |
| 66 | 0x0901 | 압축 데이터 보고 | 상향 | ✅ |
| 67 | 0x8A00 | 플랫폼 RSA 공개 키 | 하향 | ✅ |
| 68 | 0x0005 | 터미널 하위 패킷 재전송 요청 | 상향 | ✅ |
| 69 | 0x8204 | 링크 감지 | 하향 | ✅ |
| 70 | 0x6006 | 문자 메시지 전달 응답 | 상향 | ✅ |
| 71 | 0x1080 | 사용자 지정 메시지 | — | ✅ |

## 빠른 시작

### NuGet 패키지 설치

```bash
dotnet add package NewLife.JT808
```

### 상향 메시지 파싱

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

### 하향 메시지 생성

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

### 복잡한 본문 수동 직렬화(IAccessor)

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

### 다국어

이 라이브러리는 11개 런타임 언어(중국어 간체/번체, 영어, 스페인어, 독일어, 일본어, 프랑스어, 포르투갈어, 러시아어, 한국어, 아랍어)를 기본 제공합니다. 메시지는 영어 원문을 키로 사용하며, 번역이 없으면 영어 원문으로 대체됩니다.

```csharp
using System.Globalization;
using NewLife;

// Follow the process UI culture by default; pin it explicitly if needed
Localization.Culture = CultureInfo.GetCultureInfo("ja");

// ASP.NET Core hosts can resolve the request language from Accept-Language
using var scope = RequestCultureScope.Create(Request.Headers.AcceptLanguage);
```

### NativeAOT

`MessageFactory.Init()`은 프로토콜 어셈블리를 스캔하여 메시지 본문을 검색합니다. NativeAOT 또는 트리밍된 앱은 해당 어셈블리를 유지해야 합니다:

```xml
<ItemGroup>
  <TrimmerRootAssembly Include="NewLife.JT808" />
</ItemGroup>
```

이 리포지토리의 `AotSmoke` 프로젝트는 NativeAOT로 게시되며 Windows/Linux(x64 및 arm64)에서 16개 시나리오를 실행합니다.

## 생태계 지도

NewLife는 독립적으로 버전이 관리되는 오픈소스 프로젝트의 계층형 생태계입니다. 아래 지도는 모든 NewLife 리포지토리에서 그대로 동일하게 공유되며, 각 프로젝트는 코드 리포지토리와 기본 NuGet 패키지로 연결됩니다.

| 프로젝트 | 한 줄 소개 | 다운로드 |
| --- | --- | --- |
| **기반** | | |
| [NewLife.Core](https://github.com/NewLifeX/X) | 로깅, 구성, 캐시, 네트워크, 직렬화, 스케줄링, APM 추적. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Core?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Core) |
| [NewLife.XCode](https://github.com/NewLifeX/NewLife.XCode) | 데이터 미들웨어: 단일 테이블 수백억 건, 자동 분할 테이블, 읽기/쓰기 분리, 10여 종 데이터베이스 지원. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.XCode?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.XCode) |
| [NewLife.Net](https://github.com/NewLifeX/NewLife.Net) | 고성능 네트워크 라이브러리: 단일 노드 177만 pps, 14.5 Gbps(TCP 1KB, 2026-10 실측), 400만 Tcp 장기 연결. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Net?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Net) |
| [NewLife.Remoting](https://github.com/NewLifeX/NewLife.Remoting) | CS 애플리케이션과 IoT 장치를 위한 프로토콜 통신 및 RPC 프레임워크. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Remoting?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Remoting) |
| [NewLife.Agent](https://github.com/NewLifeX/NewLife.Agent) | .NET 애플리케이션을 운영체제 데몬으로 설치: Windows 서비스와 Linux systemd. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Agent?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Agent) |
| [NewLife.Templates](https://github.com/NewLifeX/NewLife.Zero) | 콘솔, Web, API, 서비스, 데이터, IoT용 `dotnet new` 프로젝트 템플릿. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Templates?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Templates) |
| **데이터 및 메시징** | | |
| [NewLife.Redis](https://github.com/NewLifeX/NewLife.Redis) | Redis 클라이언트: 마이크로초 수준 지연, 백만 수준 처리량, 메시지 큐 내장. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Redis?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Redis) |
| [NewLife.RocketMQ](https://github.com/NewLifeX/NewLife.RocketMQ) | Apache RocketMQ와 알리바바 클라우드 메시지 큐를 지원하는 순수 관리형 RocketMQ 클라이언트. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.RocketMQ?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.RocketMQ) |
| [NewLife.MQTT](https://github.com/NewLifeX/NewLife.MQTT) | MQTT 클라이언트와 서버, 알리바바 클라우드 IoT 연결 포함. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.MQTT?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.MQTT) |
| [NewLife.NovaDb](https://github.com/NewLifeX/NewLife.NovaDb) | C# 하이브리드 데이터베이스: 임베디드/네트워크 두 모드, 관계형, 시계열, 메시지 큐, KV를 하나로. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.NovaDb?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.NovaDb) |
| [NewLife.MySql](https://github.com/NewLifeX/NewLife.MySql) | NewLife 데이터 스택 위에 구축한 MySql 드라이버와 도우미 계층. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.MySql?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.MySql) |
| **Web, AI 및 분산** | | |
| [NewLife.Cube](https://github.com/NewLifeX/NewLife.Cube) | 빠른 관리 플랫폼 프레임워크: 권한, SSO, OAuth 서버, CRUD 스캐폴딩. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Cube?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Cube) |
| [Stardust](https://github.com/NewLifeX/Stardust) | 분산 서비스 플랫폼: 노드 관리, APM, 구성 센터, 등록 센터, 배포 센터. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Stardust?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Stardust) |
| [AntJob](https://github.com/NewLifeX/AntJob) | 샤딩, 재시도, 백필을 지원하는 분산 작업 및 실시간 데이터 스케줄링 플랫폼. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.AntJob?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.AntJob) |
| [NewLife.AI](https://github.com/NewLifeX/NewLife.AI) | 46개 LLM 공급자를 통합 연결하는 클라이언트와 바로 사용할 수 있는 ChatAI 웹 채팅 애플리케이션. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.AI?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.AI) |
| **산업 및 IoT** | | |
| [NewLife.IoT](https://github.com/NewLifeX/NewLife.IoT) | 프로토콜 계약과 장치 추상을 정의하는 IoT 표준 라이브러리. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.IoT?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.IoT) |
| [NewLife.Modbus](https://github.com/NewLifeX/NewLife.Modbus) | NewLife.IoT 기반으로 구현한 Modbus TCP/RTU/ASCII. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Modbus?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Modbus) |
| [NewLife.Siemens](https://github.com/NewLifeX/NewLife.Siemens) | NewLife.IoT 기반으로 구현한 지멘스 PLC 프로토콜. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Siemens?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Siemens) |
| [NewLife.Omron](https://github.com/NewLifeX/NewLife.Omron) | NewLife.IoT 기반으로 구현한 오므론 PLC 프로토콜. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Omron?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Omron) |
| [NewLife.Melsec](https://github.com/NewLifeX/NewLife.Melsec) | NewLife.IoT 기반으로 구현한 미쓰비시 Melsec PLC 프로토콜. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Melsec?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Melsec) |
| [NewLife.OPC](https://github.com/NewLifeX/NewLife.OPC) | 산업 데이터 수집용 OPC 클라이언트. | — |
| [NewLife.BACnet](https://github.com/NewLifeX/NewLife.BACnet) | BACnet 빌딩 자동화 프로토콜 스택. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.BACnet?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.BACnet) |
| [ZeroIoT](https://github.com/NewLifeX/ZeroIoT) | 게이트웨이와 장치 관리를 갖춘 경량 오픈소스 IoT 플랫폼. | — |
| **차량 텔레매틱스** | | |
| [NewLife.JT808](https://github.com/NewLifeX/NewLife.JT808) | JT/T 808 차량 위치 프로토콜: 70개 이상 메시지 본문, 2011/2013/2019 지원. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.JT808?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.JT808) |
| [NewLife.JT809](https://github.com/NewLifeX/NewLife.JT809) | 운송 관리 부서를 위한 JT/T 809 플랫폼 간 프로토콜. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.JT809?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.JT809) |
| [NewLife.JT1078](https://github.com/NewLifeX/NewLife.JT1078) | JT/T 1078 차량용 비디오: RTP 파싱, FLV/fMP4 리먹싱, 라이브 스트리밍. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.JT1078?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.JT1078) |
| **도메인 컴포넌트** | | |
| [NewLife.Office](https://github.com/NewLifeX/NewLife.Office) | Excel, Word, PPT, PDF 등 15개 이상 문서 형식을 하나의 패키지로. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Office?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Office) |
| [NewLife.Audio](https://github.com/NewLifeX/NewLife.Audio) | 크로스 플랫폼 오디오: 코덱, 장치 IO, DSP, 컨테이너, 스트리밍. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Audio?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Audio) |
| [NewLife.Video](https://github.com/NewLifeX/NewLife.Video) | 비디오 코덱, 컨테이너 형식, 프레임 처리, 스트리밍. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Video?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Video) |
| [NewLife.Map](https://github.com/NewLifeX/NewLife.Map) | 바이두/고덕/텐센트/톈디투 지도 통합 인터페이스, 지오코딩과 좌표 변환 지원. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Map?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Map) |
| [NewLife.IP](https://github.com/NewLifeX/NewLife.IP) | IPv4 위치 정보: 오프라인 메모리 매핑 데이터베이스와 온라인 API 이중 소스. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.IP?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.IP) |
| **도구 및 제품** | | |
| [CrazyCoder](https://github.com/NewLifeX/XCoder) | 개발자 도구 모음: 네트워크, 시리얼 포트, 암호화, 정규식, Modbus, MQTT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.XCoder?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.XCoder) |
| [GitCandy](https://github.com/NewLifeX/GitCandy) | 셀프 호스팅 Git 소스 코드 관리 시스템. | — |
| [LuckyClover](https://github.com/NewLifeX/LuckyClover) | 구버전 호스트에서 시작할 때 일치하는 .NET 런타임을 자동으로 설치합니다. | — |

> 배지는 각 프로젝트의 기본 패키지를 나타냅니다. 테마, 확장, 위성 패키지는 해당 NuGet 페이지에서 확인하세요. 여기에 나열하지 않은 30여 개 패키지(FTP, LoRa, Office 확장, 각종 프로토콜 드라이버 등)는 [NewLifeX 조직](https://github.com/NewLifeX)과 [newlifex.com](https://newlifex.com)에서 확인할 수 있습니다.

## 팀

<img src="https://newlifex.com/logo.png" width="48" align="right" alt="NewLife" />

뉴라이프 팀(NewLife)은 2002년에 설립되어 20여 년간 .NET과 IoT 기술 생태계에 집중하며 100여 개의 오픈소스 프로젝트를 주도하고(이 중 34개가 이 지도의 핵심 프로젝트), 전력, 물류, 의료, 산업 제어, 인터넷 등 산업에 소프트웨어와 하드웨어를 아우르는 통합 솔루션을 제공해 왔습니다. "학문에는 선후가 없으며, 이룬 사람이 스승이다"라는 신조를 따릅니다.

오픈소스 컴포넌트는 NuGet에서 누적 <!--nuget-total:start-->6.48M<!--nuget-total:end-->회 다운로드되었습니다(생태계 전체 기준, 2026-10).

웹사이트: https://newlifex.com · 오픈소스 조직: https://github.com/NewLifeX

<details>
<summary>위챗 공식 계정: 智能大石头(스캔하여 팔로우)</summary>

![智能大石头](https://newlifex.com/stone.jpg)

</details>
