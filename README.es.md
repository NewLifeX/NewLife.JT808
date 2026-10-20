# NewLife.JT808 - Protocolo de localización vehicular JT/T 808

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
🌐 [English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [Deutsch](README.de.md) | [日本語](README.ja.md) | [Français](README.fr.md) | [Português](README.pt.md) | [Русский](README.ru.md) | [한국어](README.ko.md) | [العربية](README.ar.md)
<!-- lang:end -->

> Esta traducción puede estar desactualizada; consulte el [English README](README.md).

Implementación completa del protocolo JT/T 808 de localización vehicular, con las versiones 2011/2013/2019, más de 70 cuerpos de mensaje estándar, extensiones de seguridad activa SuBiao/YueBiao, mensajes de audio/vídeo JT/T 1078, el protocolo de tacógrafo JT/T 19056 y la integración con colas de mensajes.

## Características principales

- **Protocolo completo**: delimitadores 0x7E, escape 0x7D, checksum XOR y reensamblado de subpaquetes
- **Mensajes abundantes**: más de 70 cuerpos estándar para enlace ascendente y descendente
- **Compatibilidad triple**: adaptación automática mediante `Version` y atributos `[JT2011]`/`[JT2019]`
- **Serialización dual**: reflexión por atributos y `IAccessor` para cuerpos complejos
- **TLV/KLV**: códecs genéricos con conversión a diccionarios fuertemente tipados
- **Capa de red**: códec de paquetes `JTCodec` y `JT808Client` con enrutamiento automático
- **Compatible con AOT**: proyecto de humo NativeAOT para las rutas principales
- **Localización**: 11 idiomas en tiempo de ejecución con respaldo al inglés

## Versiones del protocolo

| Versión | Indicador | Descripción |
|---------|-----------|-------------|
| JT/T 808-2011 | `Version=0` | Versión temprana con diferencias en algunos mensajes |
| JT/T 808-2013 | `Version=0` | La versión más desplegada |
| JT/T 808-2019 | `Version=1` | Última versión con nuevos campos y mensajes |

## Instalación

```bash
dotnet add package NewLife.JT808
```

## Inicio rápido

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

## Enlaces

- Código fuente: https://github.com/NewLifeX/NewLife.JT808
- NuGet: https://www.nuget.org/packages/NewLife.JT808
- Documentación: [English](README.md) | [简体中文](README.zh-CN.md)

> NewLife Team · https://newlifex.com
