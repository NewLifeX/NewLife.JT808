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
![platform](https://img.shields.io/badge/platform-Windows%20%7C%20Linux%20%7C%20macOS-blue)
[![contributors](https://img.shields.io/github/contributors/NewLifeX/NewLife.JT808?logo=github)](https://github.com/NewLifeX/NewLife.JT808/graphs/contributors)

<!-- lang:start -->
🌐 [English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [Deutsch](README.de.md) | [日本語](README.ja.md) | [Français](README.fr.md) | [Português](README.pt.md) | [Русский](README.ru.md) | [한국어](README.ko.md) | [العربية](README.ar.md)
<!-- lang:end -->

> Esta traducción puede estar desactualizada; consulte el [English README](README.md).

Implementación completa del protocolo de localización vehicular JT/T 808, que cubre las versiones 2011/2013/2019 con más de 70 cuerpos de mensaje estándar, las extensiones de seguridad activa SuBiao (Jiangsu) y YueBiao (Guangdong), los mensajes de audio/vídeo JT/T 1078, el protocolo de tacógrafo JT/T 19056 y la integración con colas de mensajes.

Fuente: https://github.com/NewLifeX/NewLife.JT808  
NuGet: [NewLife.JT808](https://www.nuget.org/packages/NewLife.JT808)  
Documentación: [Requisitos](Doc/需求文档.md) / [Arquitectura](Doc/架构设计.md) / [Lista de funcionalidades](Doc/功能清单.md) / [Guía de usuario](Doc/使用文档.md) / [Localización](Doc/多语言本地化.md)

## Características principales

- **Protocolo completo**: análisis de tramas JT/T 808-2011/2013/2019 con delimitadores 0x7E, escape 0x7D, checksum XOR y reensamblado de subpaquetes
- **Cuerpos de mensaje abundantes**: más de 70 cuerpos de mensaje estándar (0x0001~0x8A00) que cubren tanto el enlace ascendente del terminal como el enlace descendente de la plataforma
- **Compatibilidad con tres versiones**: adaptación automática de campos mediante el campo `Version` y los atributos `[JT2011]`/`[JT2019]`
- **Dos modos de serialización**: reflexión basada en atributos para cuerpos simples, `IAccessor` para cuerpos complejos que requieren control manual
- **Análisis TLV/KLV**: códecs Type-Length-Value y Kind-Length-Value integrados, con conversión a diccionarios fuertemente tipados
- **Capa de red**: códec de paquetes TCP pegados `JTCodec` y `JT808Client` con enrutamiento automático de controladores
- **Extensión SuBiao**: alarmas de seguridad activa ADAS/DSM/TPMS/BSD, estado de periféricos y protocolo de adjuntos
- **Extensión YueBiao**: estándar de seguridad activa de Guangdong con reporte de alarmas específico (0x1FC4), pass-through USB, extensiones de posición y parámetros de fabricante
- **Fundamentos de JT/T 1078**: modelos de cuerpo de mensaje de audio/vídeo y enumeraciones
- **Tacógrafo de conducción**: códec de paquetes de enlace ascendente y descendente JT/T 19056
- **Codificación BCD**: atributos de serialización personalizados para cadenas BCD y marcas de tiempo
- **Codificación GBK**: códec de cadenas GBK integrado con registro automático de `CodePagesEncodingProvider`
- **Gestión mediante Web API**: envío unificado de comandos, gestión de sesiones en línea, lista negra y autenticación por token
- **Abstracción de cola de mensajes**: `IMessageBus` crea productores/consumidores, eventos de sesión y controladores de enlace descendente; `RedisMessageBus` es el transporte interno integrado y los adaptadores RocketMQ/Kafka de `JT808.Server` se conectan mediante el conmutador de configuración `JT808:Messaging`
- **Envío de comandos en clúster**: `ClusterState` comparte sesiones en línea, latidos de nodo y lista negra a través de Redis, enruta los comandos al nodo propietario del terminal y mantiene `ICommandBus` como punto de extensión para transportes personalizados
- **Cliente reforzado**: reconexión automática, latido configurable y contadores de envío/recepción
- **Soporte de múltiples frameworks**: `net45 / net461 / netstandard2.0 / netstandard2.1 / net8.0`
- **Compatible con AOT**: proyecto de humo NativeAOT que cubre las rutas de códec, fábrica y serialización
- **Localización**: 11 idiomas en tiempo de ejecución listos para usar, texto fuente en inglés como respaldo y tablas seguras para AOT
- **Integración con RocketMQ**: adaptador productor/consumidor en el proyecto `JT808.Server` (no incluido en el paquete NuGet), compatible con Alibaba Cloud, Huawei Cloud, Tencent Cloud y autenticación ACL
- **Integración con Kafka**: adaptador productor/consumidor en el proyecto `JT808.Server` (no incluido en el paquete NuGet), basado en `Confluent.Kafka`, con grupos de consumidores y soporte de ACK

## Versiones del protocolo compatibles

| Versión | Indicador | Descripción |
|---------|------|-------------|
| JT/T 808-2011 | `Version=0` | Versión temprana con algunos diseños de mensaje diferentes |
| JT/T 808-2013 | `Version=0` | La versión más ampliamente desplegada |
| JT/T 808-2019 | `Version=1` | Última versión con nuevos campos y tipos de mensaje |

## Matriz de mensajes

| # | ID de mensaje | Nombre | Dirección | Completado |
|:---:|:------:|---------|:----:|:----:|
| 1 | 0x0001 | Respuesta general del terminal | Ascendente | ✅ |
| 2 | 0x8001 | Respuesta general de la plataforma | Descendente | ✅ |
| 3 | 0x0002 | Latido del terminal | Ascendente | ✅ |
| 4 | 0x8003 | Solicitud de retransmisión de subpaquete | Descendente | ✅ |
| 5 | 0x0004 | Solicitud de consulta de la hora del servidor | Ascendente | ✅ |
| 6 | 0x0100 | Registro del terminal | Ascendente | ✅ |
| 7 | 0x8100 | Respuesta de registro del terminal | Descendente | ✅ |
| 8 | 0x0003 | Cancelación del registro del terminal | Ascendente | ✅ |
| 9 | 0x0102 | Autenticación del terminal | Ascendente | ✅ |
| 10 | 0x8103 | Configurar parámetros del terminal | Descendente | ✅ |
| 11 | 0x8104 | Consultar parámetros del terminal | Descendente | ✅ |
| 12 | 0x0104 | Respuesta de consulta de parámetros del terminal | Ascendente | ✅ |
| 13 | 0x8105 | Control del terminal | Descendente | ✅ |
| 14 | 0x8106 | Consultar parámetros específicos del terminal | Descendente | ✅ |
| 15 | 0x8107 | Consultar atributos del terminal | Descendente | ✅ |
| 16 | 0x0107 | Respuesta de consulta de atributos del terminal | Ascendente | ✅ |
| 17 | 0x8108 | Enviar paquete de actualización al terminal | Descendente | ✅ |
| 18 | 0x0108 | Notificación del resultado de actualización del terminal | Ascendente | ✅ |
| 19 | 0x0200 | Reporte de posición | Ascendente | ✅ |
| 20 | 0x8201 | Consulta de posición | Descendente | ✅ |
| 21 | 0x0201 | Respuesta de consulta de posición | Ascendente | ✅ |
| 22 | 0x8202 | Control de seguimiento temporal de posición | Descendente | ✅ |
| 23 | 0x8203 | Confirmación manual de alarma | Descendente | ✅ |
| 24 | 0x8300 | Enviar mensaje de texto | Descendente | ✅ |
| 25 | 0x8301 | Configuración de eventos | Descendente | ✅ |
| 26 | 0x0301 | Reporte de eventos | Ascendente | ✅ |
| 27 | 0x8302 | Enviar pregunta | Descendente | ✅ |
| 28 | 0x0302 | Respuesta a la pregunta | Ascendente | ✅ |
| 29 | 0x8303 | Configuración del menú de información bajo demanda | Descendente | ✅ |
| 30 | 0x0303 | Información bajo demanda / cancelación | Ascendente | ✅ |
| 31 | 0x8304 | Servicio de información | Descendente | ✅ |
| 32 | 0x8400 | Devolución de llamada telefónica | Descendente | ✅ |
| 33 | 0x8401 | Configurar agenda telefónica | Descendente | ✅ |
| 34 | 0x8500 | Control del vehículo | Descendente | ✅ |
| 35 | 0x0500 | Respuesta de control del vehículo | Ascendente | ✅ |
| 36 | 0x8600 | Configurar área circular | Descendente | ✅ |
| 37 | 0x8601 | Eliminar área circular | Descendente | ✅ |
| 38 | 0x8602 | Configurar área rectangular | Descendente | ✅ |
| 39 | 0x8603 | Eliminar área rectangular | Descendente | ✅ |
| 40 | 0x8604 | Configurar área poligonal | Descendente | ✅ |
| 41 | 0x8605 | Eliminar área poligonal | Descendente | ✅ |
| 42 | 0x8606 | Configurar ruta | Descendente | ✅ |
| 43 | 0x8607 | Eliminar ruta | Descendente | ✅ |
| 44 | 0x8608 | Consultar datos de área o ruta | Descendente | ✅ |
| 45 | 0x0608 | Respuesta de consulta de datos de área o ruta | Ascendente | ✅ |
| 46 | 0x8700 | Comando de recolección de datos del tacógrafo | Descendente | ✅ |
| 47 | 0x0700 | Carga de datos del tacógrafo | Ascendente | ✅ |
| 48 | 0x8701 | Comando de entrega de parámetros del tacógrafo | Descendente | ✅ |
| 49 | 0x0701 | Reporte de carta de porte electrónica | Ascendente | ✅ |
| 50 | 0x8702 | Solicitud de reporte de identidad del conductor | Descendente | ✅ |
| 51 | 0x0702 | Reporte de información de identidad del conductor | Ascendente | ✅ |
| 52 | 0x0704 | Carga por lotes de datos de posición | Ascendente | ✅ |
| 53 | 0x0705 | Carga de datos del bus CAN | Ascendente | ✅ |
| 54 | 0x0800 | Reporte de evento multimedia | Ascendente | ✅ |
| 55 | 0x0801 | Carga de datos multimedia | Ascendente | ✅ |
| 56 | 0x8800 | Respuesta de carga de datos multimedia | Descendente | ✅ |
| 57 | 0x8801 | Comando de captura inmediata de la cámara | Descendente | ✅ |
| 58 | 0x0805 | Respuesta de captura inmediata de la cámara | Ascendente | ✅ |
| 59 | 0x8802 | Búsqueda de datos multimedia almacenados | Descendente | ✅ |
| 60 | 0x0802 | Respuesta de búsqueda de datos multimedia almacenados | Ascendente | ✅ |
| 61 | 0x8803 | Carga de datos multimedia almacenados | Descendente | ✅ |
| 62 | 0x8804 | Comando de inicio de grabación | Descendente | ✅ |
| 63 | 0x8805 | Carga de recuperación de un único dato multimedia almacenado | Descendente | ✅ |
| 64 | 0x8900 | Pass-through descendente | Descendente | ✅ |
| 65 | 0x0900 | Pass-through ascendente | Ascendente | ✅ |
| 66 | 0x0901 | Reporte de datos comprimidos | Ascendente | ✅ |
| 67 | 0x8A00 | Clave pública RSA de la plataforma | Descendente | ✅ |
| 68 | 0x0005 | Solicitud de retransmisión de subpaquete del terminal | Ascendente | ✅ |
| 69 | 0x8204 | Detección de enlace | Descendente | ✅ |
| 70 | 0x6006 | Respuesta de entrega de mensaje de texto | Ascendente | ✅ |
| 71 | 0x1080 | Mensaje personalizado | — | ✅ |

## Inicio rápido

### Instalar el paquete NuGet

```bash
dotnet add package NewLife.JT808
```

### Analizar un mensaje de enlace ascendente

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

### Construir un mensaje de enlace descendente

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

### Serializar manualmente un cuerpo complejo (IAccessor)

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

### Localización

La biblioteca incluye 11 idiomas en tiempo de ejecución (chino simplificado/tradicional, inglés, español, alemán, japonés, francés, portugués, ruso, coreano y árabe). Los mensajes se indexan por el texto fuente en inglés y recurren a él cuando falta una traducción.

```csharp
using System.Globalization;
using NewLife;

// Follow the process UI culture by default; pin it explicitly if needed
Localization.Culture = CultureInfo.GetCultureInfo("ja");

// ASP.NET Core hosts can resolve the request language from Accept-Language
using var scope = RequestCultureScope.Create(Request.Headers.AcceptLanguage);
```

### NativeAOT

`MessageFactory.Init()` descubre los cuerpos de mensaje escaneando el ensamblado del protocolo. Las aplicaciones NativeAOT o recortadas (trimmed) deben conservar ese ensamblado:

```xml
<ItemGroup>
  <TrimmerRootAssembly Include="NewLife.JT808" />
</ItemGroup>
```

El proyecto `AotSmoke` de este repositorio se publica con NativeAOT y ejecuta 16 escenarios en Windows/Linux (x64 y arm64).

## Mapa del ecosistema

NewLife es un ecosistema por capas de proyectos de código abierto con versionado independiente. El mapa siguiente se comparte literalmente en todos los repositorios de NewLife; cada proyecto enlaza a su repositorio y a su paquete principal de NuGet.

| Proyecto | Qué hace | Descargas |
| --- | --- | --- |
| **Fundación** | | |
| [NewLife.Core](https://github.com/NewLifeX/X) | Registro de logs, configuración, caché, red, serialización, programación de tareas y seguimiento APM. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Core?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Core) |
| [NewLife.XCode](https://github.com/NewLifeX/NewLife.XCode) | Middleware de datos: tablas únicas a escala de decenas de miles de millones, particionado automático, separación lectura/escritura y más de 10 bases de datos. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.XCode?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.XCode) |
| [NewLife.Net](https://github.com/NewLifeX/NewLife.Net) | Biblioteca de red de alto rendimiento: 1,77 M pps / 14,5 Gbps en una máquina (TCP 1KB, 2026-10), 4 M conexiones TCP persistentes. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Net?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Net) |
| [NewLife.Remoting](https://github.com/NewLifeX/NewLife.Remoting) | Framework de comunicación por protocolo y RPC para aplicaciones cliente/servidor y dispositivos IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Remoting?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Remoting) |
| [NewLife.Agent](https://github.com/NewLifeX/NewLife.Agent) | Instala aplicaciones .NET como demonios del sistema operativo: servicios de Windows y systemd de Linux. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Agent?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Agent) |
| [NewLife.Templates](https://github.com/NewLifeX/NewLife.Zero) | Plantillas de proyecto `dotnet new` para consola, web, API, servicio, datos e IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Templates?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Templates) |
| **Datos y mensajería** | | |
| [NewLife.Redis](https://github.com/NewLifeX/NewLife.Redis) | Cliente Redis con latencia de microsegundos, millones de operaciones por segundo y colas de mensajes integradas. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Redis?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Redis) |
| [NewLife.RocketMQ](https://github.com/NewLifeX/NewLife.RocketMQ) | Cliente RocketMQ totalmente administrado para Apache RocketMQ y Alibaba Cloud Message Queue. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.RocketMQ?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.RocketMQ) |
| [NewLife.MQTT](https://github.com/NewLifeX/NewLife.MQTT) | Cliente y servidor MQTT, incluida la conectividad con Alibaba Cloud IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.MQTT?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.MQTT) |
| [NewLife.NovaDb](https://github.com/NewLifeX/NewLife.NovaDb) | Base de datos híbrida en C#: embebida o en red, relacional, series temporales, cola de mensajes y KV en una sola. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.NovaDb?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.NovaDb) |
| [NewLife.MySql](https://github.com/NewLifeX/NewLife.MySql) | Controlador y capa de ayuda de MySql construidos sobre la pila de datos de NewLife. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.MySql?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.MySql) |
| **Web, IA y distribuido** | | |
| [NewLife.Cube](https://github.com/NewLifeX/NewLife.Cube) | Framework rápido para plataformas de administración: permisos, SSO, servidor OAuth y andamiaje CRUD. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Cube?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Cube) |
| [Stardust](https://github.com/NewLifeX/Stardust) | Plataforma distribuida de servicios: gestión de nodos, APM, centro de configuración, registro y centro de despliegues. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Stardust?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Stardust) |
| [AntJob](https://github.com/NewLifeX/AntJob) | Plataforma de tareas distribuidas y programación de datos en tiempo real con particionado, reintentos y relleno histórico. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.AntJob?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.AntJob) |
| [NewLife.AI](https://github.com/NewLifeX/NewLife.AI) | Cliente unificado para 46 proveedores de LLM más ChatAI, una aplicación web de chat lista para usar. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.AI?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.AI) |
| **Industrial e IoT** | | |
| [NewLife.IoT](https://github.com/NewLifeX/NewLife.IoT) | Biblioteca estándar de IoT que define contratos de protocolo y abstracciones de dispositivos. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.IoT?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.IoT) |
| [NewLife.Modbus](https://github.com/NewLifeX/NewLife.Modbus) | Implementación de Modbus TCP/RTU/ASCII sobre NewLife.IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Modbus?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Modbus) |
| [NewLife.Siemens](https://github.com/NewLifeX/NewLife.Siemens) | Protocolo para PLC Siemens sobre NewLife.IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Siemens?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Siemens) |
| [NewLife.Omron](https://github.com/NewLifeX/NewLife.Omron) | Protocolo para PLC Omron sobre NewLife.IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Omron?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Omron) |
| [NewLife.Melsec](https://github.com/NewLifeX/NewLife.Melsec) | Protocolo para PLC Mitsubishi Melsec sobre NewLife.IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Melsec?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Melsec) |
| [NewLife.OPC](https://github.com/NewLifeX/NewLife.OPC) | Cliente OPC para el acceso a datos industriales. | — |
| [NewLife.BACnet](https://github.com/NewLifeX/NewLife.BACnet) | Pila de protocolo BACnet para la automatización de edificios. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.BACnet?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.BACnet) |
| [ZeroIoT](https://github.com/NewLifeX/ZeroIoT) | Plataforma IoT de código abierto y ligera con gestión de pasarelas y dispositivos. | — |
| **Telemática vehicular** | | |
| [NewLife.JT808](https://github.com/NewLifeX/NewLife.JT808) | Protocolo de posicionamiento vehicular JT/T 808: más de 70 cuerpos de mensaje, 2011/2013/2019. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.JT808?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.JT808) |
| [NewLife.JT809](https://github.com/NewLifeX/NewLife.JT809) | Protocolo plataforma a plataforma JT/T 809 para autoridades de transporte. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.JT809?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.JT809) |
| [NewLife.JT1078](https://github.com/NewLifeX/NewLife.JT1078) | Vídeo a bordo JT/T 1078: análisis RTP, remultiplexado FLV/fMP4 y transmisión en directo. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.JT1078?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.JT1078) |
| **Componentes de dominio** | | |
| [NewLife.Office](https://github.com/NewLifeX/NewLife.Office) | Un solo paquete para Excel, Word, PPT, PDF y más de 15 formatos de documento. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Office?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Office) |
| [NewLife.Audio](https://github.com/NewLifeX/NewLife.Audio) | Audio multiplataforma: códecs, E/S de dispositivos, DSP, contenedores y streaming. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Audio?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Audio) |
| [NewLife.Video](https://github.com/NewLifeX/NewLife.Video) | Códecs de vídeo, formatos de contenedor, procesamiento de fotogramas y streaming. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Video?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Video) |
| [NewLife.Map](https://github.com/NewLifeX/NewLife.Map) | Interfaz unificada sobre Baidu, AMap, Tencent y Tianditu con geocodificación y conversión de coordenadas. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Map?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Map) |
| [NewLife.IP](https://github.com/NewLifeX/NewLife.IP) | Geolocalización IPv4 con una base de datos offline mapeada en memoria y una API en línea de respaldo. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.IP?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.IP) |
| **Herramientas y productos** | | |
| [CrazyCoder](https://github.com/NewLifeX/XCoder) | Caja de herramientas del desarrollador: red, puerto serie, criptografía, expresiones regulares, Modbus y MQTT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.XCoder?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.XCoder) |
| [GitCandy](https://github.com/NewLifeX/GitCandy) | Sistema de gestión de código fuente Git autoalojado. | — |
| [LuckyClover](https://github.com/NewLifeX/LuckyClover) | Instala automáticamente el runtime de .NET correspondiente cuando se ejecuta desde un host más antiguo. | — |

> Las insignias muestran el paquete principal de cada proyecto; los paquetes de temas, extensiones y satélites figuran en sus páginas de NuGet. Los más de 30 paquetes no listados aquí (FTP, LoRa, extensiones de Office, controladores de protocolo, etc.) están disponibles en la [organización NewLifeX](https://github.com/NewLifeX) y en [newlifex.com](https://newlifex.com).

## Equipo

<img src="https://newlifex.com/logo.png" width="48" align="right" alt="NewLife" />

NewLife se fundó en 2002 y lleva más de dos décadas dedicado al ecosistema .NET e IoT, mantiene cerca de 100 proyectos de código abierto (34 de ellos principales) y ofrece soluciones integradas de software y hardware para los sectores de energía, logística, salud, control industrial e Internet. Nos guía el lema «en el aprendizaje no hay antigüedad: quien domina es maestro».

Los componentes de código abierto acumulan <!--nuget-total:start-->6.48M<!--nuget-total:end--> descargas en NuGet (todo el ecosistema, a fecha de 2026-10).

Sitio web: https://newlifex.com · Organización de código abierto: https://github.com/NewLifeX

<details>
<summary>Cuenta oficial de WeChat: 智能大石头 (escanea para seguir)</summary>

![智能大石头](https://newlifex.com/stone.jpg)

</details>