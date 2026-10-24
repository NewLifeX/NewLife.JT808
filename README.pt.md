# NewLife.JT808 - Protocolo de localização veicular JT/T 808

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
🌐 [English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [Español](README.es.md) | [Deutsch](README.de.md) | [日本語](README.ja.md) | [Français](README.fr.md) | [Русский](README.ru.md) | [한국어](README.ko.md) | [العربية](README.ar.md)
<!-- lang:end -->

> Esta tradução pode estar desatualizada; consulte o [English README](README.md).

Implementação completa do protocolo JT/T 808 de localização veicular, cobrindo as versões 2011/2013/2019 com mais de 70 corpos de mensagem padrão, as extensões de segurança ativa SuBiao (Jiangsu) e YueBiao (Guangdong), as mensagens de áudio/vídeo JT/T 1078, o protocolo de tacógrafo JT/T 19056 e a integração com filas de mensagens.

Fonte: https://github.com/NewLifeX/NewLife.JT808  
NuGet: [NewLife.JT808](https://www.nuget.org/packages/NewLife.JT808)  
Documentação: [Requisitos](Doc/需求文档.md) / [Arquitetura](Doc/架构设计.md) / [Lista de funcionalidades](Doc/功能清单.md) / [Guia de usuário](Doc/使用文档.md) / [Localização](Doc/多语言本地化.md)

## Recursos principais

- **Protocolo completo**: análise de quadros JT/T 808-2011/2013/2019 com delimitadores 0x7E, escape 0x7D, checksum XOR e remontagem de subpacotes
- **Corpos de mensagem abundantes**: mais de 70 corpos de mensagem padrão (0x0001~0x8A00), cobrindo tanto o uplink do terminal quanto o downlink da plataforma
- **Compatibilidade entre três versões**: adaptação automática de campos pelo campo `Version` e pelos atributos `[JT2011]`/`[JT2019]`
- **Dois modos de serialização**: reflexão orientada por atributos para corpos simples, `IAccessor` para corpos complexos que exigem controle manual
- **Análise TLV/KLV**: codecs Type-Length-Value e Kind-Length-Value integrados, com conversão para dicionários fortemente tipados
- **Camada de rede**: codec de pacotes aderentes TCP `JTCodec` e `JT808Client` com roteamento automático de handlers
- **Extensão SuBiao**: alarmes de segurança ativa ADAS/DSM/TPMS/BSD, status de periféricos e protocolo de anexos
- **Extensão YueBiao**: padrão de segurança ativa de Guangdong com relatório de alarme dedicado (0x1FC4), pass-through USB, extensões de posição e parâmetros de fabricante
- **Fundamentos do JT/T 1078**: modelos de corpo de mensagem de áudio/vídeo e enumerações
- **Tacógrafo**: codec de pacotes de uplink e downlink JT/T 19056
- **Codificação BCD**: atributos de serialização personalizados para strings BCD e timestamps
- **Codificação GBK**: codec de strings GBK integrado com registro automático de `CodePagesEncodingProvider`
- **Gestão via Web API**: entrega unificada de comandos, gestão de sessões online, lista negra e autenticação por token
- **Abstração de fila de mensagens**: `IMessageBus` cria produtores/consumidores, eventos de sessão e handlers de downlink; `RedisMessageBus` é o transporte interno integrado e os adaptadores RocketMQ/Kafka em `JT808.Server` conectam-se pela chave de configuração `JT808:Messaging`
- **Entrega de comandos em cluster**: `ClusterState` compartilha sessões online, heartbeats de nós e a lista negra via Redis, encaminha comandos para o nó que possui o terminal e mantém `ICommandBus` como ponto de extensão para transportes personalizados
- **Cliente reforçado**: reconexão automática, heartbeat configurável e contadores de envio/recepção
- **Suporte a múltiplos frameworks**: `net45 / net461 / netstandard2.0 / netstandard2.1 / net8.0`
- **Compatível com AOT**: projeto de smoke NativeAOT cobrindo os caminhos de codec, factory e serialização
- **Localização**: 11 idiomas em tempo de execução prontos para uso, texto de origem em inglês como fallback e tabelas compatíveis com AOT
- **Integração com RocketMQ**: adaptador produtor/consumidor no projeto `JT808.Server` (não incluído no pacote NuGet), com suporte a Alibaba Cloud, Huawei Cloud, Tencent Cloud e autenticação ACL
- **Integração com Kafka**: adaptador produtor/consumidor no projeto `JT808.Server` (não incluído no pacote NuGet), baseado em `Confluent.Kafka` com grupos de consumidores e suporte a ACK

## Versões de protocolo suportadas

| Versão | Indicador | Descrição |
|---------|------|-------------|
| JT/T 808-2011 | `Version=0` | Versão inicial com alguns layouts de mensagem diferentes |
| JT/T 808-2013 | `Version=0` | Versão mais amplamente utilizada |
| JT/T 808-2019 | `Version=1` | Versão mais recente com novos campos e tipos de mensagem |

## Matriz de mensagens

| # | ID da mensagem | Nome | Direção | Concluído |
|:---:|:------:|---------|:----:|:----:|
| 1 | 0x0001 | Resposta geral do terminal | Ascendente | ✅ |
| 2 | 0x8001 | Resposta geral da plataforma | Descendente | ✅ |
| 3 | 0x0002 | Heartbeat do terminal | Ascendente | ✅ |
| 4 | 0x8003 | Solicitação de retransmissão de subpacote | Descendente | ✅ |
| 5 | 0x0004 | Consulta de hora do servidor | Ascendente | ✅ |
| 6 | 0x0100 | Registro do terminal | Ascendente | ✅ |
| 7 | 0x8100 | Resposta de registro do terminal | Descendente | ✅ |
| 8 | 0x0003 | Cancelamento de registro do terminal | Ascendente | ✅ |
| 9 | 0x0102 | Autenticação do terminal | Ascendente | ✅ |
| 10 | 0x8103 | Definir parâmetros do terminal | Descendente | ✅ |
| 11 | 0x8104 | Consultar parâmetros do terminal | Descendente | ✅ |
| 12 | 0x0104 | Resposta de consulta de parâmetros do terminal | Ascendente | ✅ |
| 13 | 0x8105 | Controle do terminal | Descendente | ✅ |
| 14 | 0x8106 | Consultar parâmetros específicos do terminal | Descendente | ✅ |
| 15 | 0x8107 | Consultar atributos do terminal | Descendente | ✅ |
| 16 | 0x0107 | Resposta de consulta de atributos do terminal | Ascendente | ✅ |
| 17 | 0x8108 | Enviar pacote de atualização do terminal | Descendente | ✅ |
| 18 | 0x0108 | Notificação de resultado da atualização do terminal | Ascendente | ✅ |
| 19 | 0x0200 | Relatório de posição | Ascendente | ✅ |
| 20 | 0x8201 | Consulta de posição | Descendente | ✅ |
| 21 | 0x0201 | Resposta de consulta de posição | Ascendente | ✅ |
| 22 | 0x8202 | Controle de rastreamento temporário de posição | Descendente | ✅ |
| 23 | 0x8203 | Confirmação manual de alarme | Descendente | ✅ |
| 24 | 0x8300 | Enviar mensagem de texto | Descendente | ✅ |
| 25 | 0x8301 | Configuração de eventos | Descendente | ✅ |
| 26 | 0x0301 | Relatório de evento | Ascendente | ✅ |
| 27 | 0x8302 | Enviar pergunta | Descendente | ✅ |
| 28 | 0x0302 | Resposta à pergunta | Ascendente | ✅ |
| 29 | 0x8303 | Configuração do menu de informações sob demanda | Descendente | ✅ |
| 30 | 0x0303 | Informações sob demanda / cancelamento | Ascendente | ✅ |
| 31 | 0x8304 | Serviço de informações | Descendente | ✅ |
| 32 | 0x8400 | Retorno de chamada telefônica | Descendente | ✅ |
| 33 | 0x8401 | Definir agenda telefônica | Descendente | ✅ |
| 34 | 0x8500 | Controle do veículo | Descendente | ✅ |
| 35 | 0x0500 | Resposta de controle do veículo | Ascendente | ✅ |
| 36 | 0x8600 | Definir área circular | Descendente | ✅ |
| 37 | 0x8601 | Excluir área circular | Descendente | ✅ |
| 38 | 0x8602 | Definir área retangular | Descendente | ✅ |
| 39 | 0x8603 | Excluir área retangular | Descendente | ✅ |
| 40 | 0x8604 | Definir área poligonal | Descendente | ✅ |
| 41 | 0x8605 | Excluir área poligonal | Descendente | ✅ |
| 42 | 0x8606 | Definir rota | Descendente | ✅ |
| 43 | 0x8607 | Excluir rota | Descendente | ✅ |
| 44 | 0x8608 | Consultar dados de área ou rota | Descendente | ✅ |
| 45 | 0x0608 | Resposta de consulta de dados de área ou rota | Ascendente | ✅ |
| 46 | 0x8700 | Comando de coleta de dados do tacógrafo | Descendente | ✅ |
| 47 | 0x0700 | Envio de dados do tacógrafo | Ascendente | ✅ |
| 48 | 0x8701 | Comando de entrega de parâmetros do tacógrafo | Descendente | ✅ |
| 49 | 0x0701 | Relatório de conhecimento eletrônico de carga | Ascendente | ✅ |
| 50 | 0x8702 | Solicitação de relatório de identidade do motorista | Descendente | ✅ |
| 51 | 0x0702 | Relatório de informações de identidade do motorista | Ascendente | ✅ |
| 52 | 0x0704 | Envio de dados de posição em lote | Ascendente | ✅ |
| 53 | 0x0705 | Envio de dados do barramento CAN | Ascendente | ✅ |
| 54 | 0x0800 | Relatório de evento multimídia | Ascendente | ✅ |
| 55 | 0x0801 | Envio de dados multimídia | Ascendente | ✅ |
| 56 | 0x8800 | Resposta ao envio de dados multimídia | Descendente | ✅ |
| 57 | 0x8801 | Comando de captura imediata da câmera | Descendente | ✅ |
| 58 | 0x0805 | Resposta de captura imediata da câmera | Ascendente | ✅ |
| 59 | 0x8802 | Busca de dados multimídia armazenados | Descendente | ✅ |
| 60 | 0x0802 | Resposta de busca de dados multimídia armazenados | Ascendente | ✅ |
| 61 | 0x8803 | Envio de dados multimídia armazenados | Descendente | ✅ |
| 62 | 0x8804 | Comando de iniciar gravação | Descendente | ✅ |
| 63 | 0x8805 | Recuperação e envio de um único dado multimídia armazenado | Descendente | ✅ |
| 64 | 0x8900 | Pass-through descendente | Descendente | ✅ |
| 65 | 0x0900 | Pass-through ascendente | Ascendente | ✅ |
| 66 | 0x0901 | Relatório de dados compactados | Ascendente | ✅ |
| 67 | 0x8A00 | Chave pública RSA da plataforma | Descendente | ✅ |
| 68 | 0x0005 | Solicitação de retransmissão de subpacote do terminal | Ascendente | ✅ |
| 69 | 0x8204 | Detecção de enlace | Descendente | ✅ |
| 70 | 0x6006 | Resposta à entrega de mensagem de texto | Ascendente | ✅ |
| 71 | 0x1080 | Mensagem personalizada | — | ✅ |

## Início rápido

### Instalar o pacote NuGet

```bash
dotnet add package NewLife.JT808
```

### Analisar uma mensagem de uplink

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

### Construir uma mensagem de downlink

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

### Serializar manualmente um corpo complexo (IAccessor)

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

### Localização

A biblioteca inclui 11 idiomas em tempo de execução (chinês simplificado/tradicional, inglês, espanhol, alemão, japonês, francês, português, russo, coreano e árabe). As mensagens são indexadas pelo texto de origem em inglês e recorrem a ele quando falta uma tradução.

```csharp
using System.Globalization;
using NewLife;

// Follow the process UI culture by default; pin it explicitly if needed
Localization.Culture = CultureInfo.GetCultureInfo("ja");

// ASP.NET Core hosts can resolve the request language from Accept-Language
using var scope = RequestCultureScope.Create(Request.Headers.AcceptLanguage);
```

### NativeAOT

`MessageFactory.Init()` descobre os corpos de mensagem examinando o assembly de protocolo. Aplicações NativeAOT ou com trimming devem manter esse assembly:

```xml
<ItemGroup>
  <TrimmerRootAssembly Include="NewLife.JT808" />
</ItemGroup>
```

O projeto `AotSmoke` neste repositório é publicado com NativeAOT e executa 16 cenários no Windows/Linux (x64 e arm64).

## Mapa do ecossistema

NewLife é um ecossistema em camadas de projetos open source versionados de forma independente. O mapa abaixo é compartilhado literalmente por todos os repositórios NewLife; cada projeto tem links para o seu repositório e para o seu pacote principal no NuGet.

| Projeto | O que faz | Downloads |
| --- | --- | --- |
| **Base** | | |
| [NewLife.Core](https://github.com/NewLifeX/X) | Log, configuração, cache, rede, serialização, agendamento e rastreamento APM. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Core?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Core) |
| [NewLife.XCode](https://github.com/NewLifeX/NewLife.XCode) | Middleware de dados: tabelas únicas na escala de dezenas de bilhões de linhas, particionamento automático, separação de leitura e escrita, mais de 10 bancos de dados. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.XCode?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.XCode) |
| [NewLife.Net](https://github.com/NewLifeX/NewLife.Net) | Biblioteca de rede de alto desempenho: 1,77M pps / 14,5 Gbps em uma máquina (TCP 1KB, 2026-10), 4M de conexões TCP persistentes. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Net?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Net) |
| [NewLife.Remoting](https://github.com/NewLifeX/NewLife.Remoting) | Framework de comunicação por protocolo e RPC para aplicações cliente/servidor e dispositivos IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Remoting?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Remoting) |
| [NewLife.Agent](https://github.com/NewLifeX/NewLife.Agent) | Instala aplicações .NET como daemons do sistema operacional: serviços do Windows e systemd do Linux. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Agent?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Agent) |
| [NewLife.Templates](https://github.com/NewLifeX/NewLife.Zero) | Modelos de projeto `dotnet new` para console, web, API, serviço, dados e IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Templates?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Templates) |
| **Dados e mensagens** | | |
| [NewLife.Redis](https://github.com/NewLifeX/NewLife.Redis) | Cliente Redis com latência em microssegundos, milhões de operações por segundo e filas de mensagens integradas. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Redis?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Redis) |
| [NewLife.RocketMQ](https://github.com/NewLifeX/NewLife.RocketMQ) | Cliente RocketMQ totalmente gerenciado para Apache RocketMQ e Alibaba Cloud Message Queue. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.RocketMQ?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.RocketMQ) |
| [NewLife.MQTT](https://github.com/NewLifeX/NewLife.MQTT) | Cliente e servidor MQTT, incluindo conectividade com o Alibaba Cloud IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.MQTT?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.MQTT) |
| [NewLife.NovaDb](https://github.com/NewLifeX/NewLife.NovaDb) | Banco de dados híbrido em C#: embarcado ou em rede, relacional, séries temporais, fila de mensagens e KV em um só. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.NovaDb?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.NovaDb) |
| [NewLife.MySql](https://github.com/NewLifeX/NewLife.MySql) | Driver MySql e camada auxiliar construídos sobre a pilha de dados NewLife. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.MySql?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.MySql) |
| **Web, IA e distribuído** | | |
| [NewLife.Cube](https://github.com/NewLifeX/NewLife.Cube) | Framework de plataforma administrativa rápida: permissões, SSO, servidor OAuth e scaffolding CRUD. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Cube?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Cube) |
| [Stardust](https://github.com/NewLifeX/Stardust) | Plataforma de serviços distribuídos: gestão de nós, APM, centro de configuração, registro e centro de publicação. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Stardust?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Stardust) |
| [AntJob](https://github.com/NewLifeX/AntJob) | Plataforma distribuída de tarefas e agendamento de dados em tempo real, com particionamento, repetição e reprocessamento. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.AntJob?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.AntJob) |
| [NewLife.AI](https://github.com/NewLifeX/NewLife.AI) | Cliente unificado para 46 provedores de LLM, além do ChatAI, uma aplicação web de chat pronta para uso. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.AI?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.AI) |
| **Industrial e IoT** | | |
| [NewLife.IoT](https://github.com/NewLifeX/NewLife.IoT) | Biblioteca padrão de IoT que define contratos de protocolo e abstrações de dispositivos. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.IoT?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.IoT) |
| [NewLife.Modbus](https://github.com/NewLifeX/NewLife.Modbus) | Implementação Modbus TCP/RTU/ASCII sobre NewLife.IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Modbus?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Modbus) |
| [NewLife.Siemens](https://github.com/NewLifeX/NewLife.Siemens) | Protocolo Siemens PLC sobre NewLife.IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Siemens?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Siemens) |
| [NewLife.Omron](https://github.com/NewLifeX/NewLife.Omron) | Protocolo Omron PLC sobre NewLife.IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Omron?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Omron) |
| [NewLife.Melsec](https://github.com/NewLifeX/NewLife.Melsec) | Protocolo Mitsubishi Melsec PLC sobre NewLife.IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Melsec?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Melsec) |
| [NewLife.OPC](https://github.com/NewLifeX/NewLife.OPC) | Cliente OPC para acesso a dados industriais. | — |
| [NewLife.BACnet](https://github.com/NewLifeX/NewLife.BACnet) | Pilha do protocolo de automação predial BACnet. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.BACnet?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.BACnet) |
| [ZeroIoT](https://github.com/NewLifeX/ZeroIoT) | Plataforma IoT de código aberto leve, com gestão de gateways e dispositivos. | — |
| **Telemática veicular** | | |
| [NewLife.JT808](https://github.com/NewLifeX/NewLife.JT808) | Protocolo de posicionamento veicular JT/T 808: mais de 70 corpos de mensagem, 2011/2013/2019. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.JT808?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.JT808) |
| [NewLife.JT809](https://github.com/NewLifeX/NewLife.JT809) | Protocolo plataforma-a-plataforma JT/T 809 para autoridades de transporte. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.JT809?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.JT809) |
| [NewLife.JT1078](https://github.com/NewLifeX/NewLife.JT1078) | Vídeo veicular JT/T 1078: análise RTP, remux para FLV/fMP4 e transmissão ao vivo. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.JT1078?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.JT1078) |
| **Componentes de domínio** | | |
| [NewLife.Office](https://github.com/NewLifeX/NewLife.Office) | Um pacote para Excel, Word, PPT, PDF e mais de 15 formatos de documento. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Office?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Office) |
| [NewLife.Audio](https://github.com/NewLifeX/NewLife.Audio) | Áudio multiplataforma: codecs, E/S de dispositivos, DSP, contêineres e streaming. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Audio?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Audio) |
| [NewLife.Video](https://github.com/NewLifeX/NewLife.Video) | Codecs de vídeo, formatos de contêiner, processamento de frames e streaming. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Video?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Video) |
| [NewLife.Map](https://github.com/NewLifeX/NewLife.Map) | Interface unificada sobre Baidu, AMap, Tencent e Tianditu, com geocodificação e conversão de coordenadas. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Map?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Map) |
| [NewLife.IP](https://github.com/NewLifeX/NewLife.IP) | Geolocalização IPv4 com banco de dados offline mapeado em memória e API online como alternativa. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.IP?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.IP) |
| **Ferramentas e produtos** | | |
| [CrazyCoder](https://github.com/NewLifeX/XCoder) | Caixa de ferramentas do desenvolvedor: rede, porta serial, criptografia, regex, Modbus e MQTT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.XCoder?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.XCoder) |
| [GitCandy](https://github.com/NewLifeX/GitCandy) | Sistema auto-hospedado de gerenciamento de código-fonte Git. | — |
| [LuckyClover](https://github.com/NewLifeX/LuckyClover) | Instala automaticamente o runtime .NET correspondente quando iniciado em um host mais antigo. | — |

> Os badges mostram o pacote principal de cada projeto; pacotes de tema, extensão e satélites estão listados nas respectivas páginas do NuGet. Os mais de 30 pacotes não listados aqui (FTP, LoRa, extensões do Office, drivers de protocolo e outros) estão disponíveis na [organização NewLifeX](https://github.com/NewLifeX) e em [newlifex.com](https://newlifex.com).

## Equipe

<img src="https://newlifex.com/logo.png" width="48" align="right" alt="NewLife" />

Fundada em 2002, a equipe NewLife dedica-se há mais de duas décadas ao ecossistema .NET e de IoT, mantém cerca de 100 projetos de código aberto (34 deles essenciais para este mapa) e oferece soluções integradas de software e hardware para os setores de energia, logística, saúde, controle industrial e internet. Seguimos o lema "não há hierarquia no aprendizado — o domínio conquista o respeito".

Os componentes de código aberto já foram baixados <!--nuget-total:start-->6.48M<!--nuget-total:end--> vezes no NuGet (todo o ecossistema, até 2026-10).

Website: https://newlifex.com · Organização de código aberto: https://github.com/NewLifeX

<details>
<summary>Conta oficial no WeChat: 智能大石头 (escaneie para seguir)</summary>

![智能大石头](https://newlifex.com/stone.jpg)

</details>

## Licença

Este projeto é distribuído sob a licença [MIT](LICENSE); você pode usá-lo, modificá-lo e redistribuí-lo livremente, desde que o aviso de copyright seja mantido.

> Se este projeto for útil, deixe uma Star, compartilhe com sua equipe ou envie sugestões de melhoria via Issue/PR.
