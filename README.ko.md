# NewLife.JT808 - JT/T 808 차량 위치 추적 프로토콜

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
🌐 [English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [Español](README.es.md) | [Deutsch](README.de.md) | [日本語](README.ja.md) | [Français](README.fr.md) | [Português](README.pt.md) | [Русский](README.ru.md) | [العربية](README.ar.md)
<!-- lang:end -->

> 이 번역은 지연될 수 있습니다. [English README](README.md)를 참조하세요.

JT/T 808 차량 위치 추적 프로토콜의 완전한 구현으로, 2011/2013/2019 버전과 70개 이상의 표준 메시지 본문, SuBiao/YueBiao 능동 안전 확장, JT/T 1078 오디오/비디오 메시지, JT/T 19056 주행 기록계 프로토콜 및 메시지 큐 연동을 제공합니다.

## 주요 기능

- **완전한 프로토콜**: 0x7E 구분자, 0x7D 이스케이프, XOR 체크섬, 하위 패킷 재조립
- **풍부한 메시지**: 상향/하향 70개 이상의 표준 메시지 본문
- **세 가지 버전 호환**: `Version` 필드와 `[JT2011]`/`[JT2019]` 특성으로 자동 적응
- **이중 직렬화**: 특성 기반 직렬화와 복잡한 본문용 `IAccessor`
- **TLV/KLV**: 범용 코덱과 강력한 형식의 사전 변환
- **네트워크 계층**: `JTCodec` 점착 패킷 코덱과 `JT808Client` 자동 라우팅
- **AOT 지원**: NativeAOT 스모크 프로젝트로 핵심 경로 검증
- **다국어**: 런타임 11개 언어, 영어 원문으로 대체

## 지원 프로토콜 버전

| 버전 | 식별자 | 설명 |
|------|--------|------|
| JT/T 808-2011 | `Version=0` | 일부 메시지 구조가 다른 초기 버전 |
| JT/T 808-2013 | `Version=0` | 가장 널리 사용되는 버전 |
| JT/T 808-2019 | `Version=1` | 새로운 필드와 메시지를 추가한 최신 버전 |

## 설치

```bash
dotnet add package NewLife.JT808
```

## 빠른 시작

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

## 링크

- 소스 코드: https://github.com/NewLifeX/NewLife.JT808
- NuGet: https://www.nuget.org/packages/NewLife.JT808
- 문서: [English](README.md) | [简体中文](README.zh-CN.md)

> NewLife Team · https://newlifex.com
