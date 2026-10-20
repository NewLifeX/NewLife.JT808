# NewLife.JT808 - Protocolo de localização veicular JT/T 808

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
🌐 [English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [Español](README.es.md) | [Deutsch](README.de.md) | [日本語](README.ja.md) | [Français](README.fr.md) | [Русский](README.ru.md) | [한국어](README.ko.md) | [العربية](README.ar.md)
<!-- lang:end -->

> Esta tradução pode estar desatualizada; consulte o [English README](README.md).

Implementação completa do protocolo JT/T 808 de localização veicular, com as versões 2011/2013/2019, mais de 70 corpos de mensagem padrão, extensões de segurança ativa SuBiao/YueBiao, mensagens de áudio/vídeo JT/T 1078, o protocolo de tacógrafo JT/T 19056 e integração com filas de mensagens.

## Recursos principais

- **Protocolo completo**: delimitadores 0x7E, escape 0x7D, checksum XOR e remontagem de subpacotes
- **Mensagens abundantes**: mais de 70 corpos padrão para uplink e downlink
- **Três versões**: adaptação automática via `Version` e atributos `[JT2011]`/`[JT2019]`
- **Serialização dupla**: por atributos e `IAccessor` para corpos complexos
- **TLV/KLV**: codecs genéricos com conversão para dicionários fortemente tipados
- **Camada de rede**: codec `JTCodec` e `JT808Client` com roteamento automático
- **Compatível com AOT**: projeto de smoke NativeAOT para os caminhos principais
- **Localização**: 11 idiomas em tempo de execução com fallback para inglês

## Versões do protocolo

| Versão | Indicador | Descrição |
|--------|-----------|-----------|
| JT/T 808-2011 | `Version=0` | Versão inicial com algumas diferenças de layout |
| JT/T 808-2013 | `Version=0` | Versão mais amplamente utilizada |
| JT/T 808-2019 | `Version=1` | Versão mais recente com novos campos e mensagens |

## Instalação

```bash
dotnet add package NewLife.JT808
```

## Início rápido

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

- Código-fonte: https://github.com/NewLifeX/NewLife.JT808
- NuGet: https://www.nuget.org/packages/NewLife.JT808
- Documentação: [English](README.md) | [简体中文](README.zh-CN.md)

> NewLife Team · https://newlifex.com
