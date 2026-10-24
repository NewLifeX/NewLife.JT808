# NewLife.JT808 - Protocole de localisation de véhicules JT/T 808

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
🌐 [English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [Español](README.es.md) | [Deutsch](README.de.md) | [日本語](README.ja.md) | [Português](README.pt.md) | [Русский](README.ru.md) | [한국어](README.ko.md) | [العربية](README.ar.md)
<!-- lang:end -->

> Cette traduction peut être obsolète ; reportez-vous au [English README](README.md).

Implémentation complète du protocole de localisation de véhicules JT/T 808, couvrant les versions 2011/2013/2019 avec plus de 70 corps de messages standard, les extensions de sécurité active SuBiao (Jiangsu) et YueBiao (Guangdong), les messages audio/vidéo JT/T 1078, le protocole de chronotachygraphe JT/T 19056 et l'intégration aux files de messages.

Source : https://github.com/NewLifeX/NewLife.JT808  
NuGet : [NewLife.JT808](https://www.nuget.org/packages/NewLife.JT808)  
Docs : [Exigences](Doc/需求文档.md) / [Architecture](Doc/架构设计.md) / [Liste des fonctionnalités](Doc/功能清单.md) / [Guide d'utilisation](Doc/使用文档.md) / [Localisation](Doc/多语言本地化.md)

## Fonctionnalités clés

- **Protocole complet** : analyse des trames JT/T 808-2011/2013/2019 avec délimiteurs 0x7E, échappement 0x7D, somme de contrôle XOR et réassemblage des sous-paquets
- **Corps de messages riches** : plus de 70 corps de messages standard (0x0001~0x8A00) couvrant la liaison montante du terminal et la liaison descendante de la plateforme
- **Compatibilité trois versions** : adaptation automatique des champs via le champ `Version` et les attributs `[JT2011]`/`[JT2019]`
- **Double mode de sérialisation** : réflexion pilotée par attributs pour les corps simples, `IAccessor` pour les corps complexes nécessitant un contrôle manuel
- **Analyse TLV/KLV** : codecs Type-Length-Value et Kind-Length-Value intégrés avec conversion en dictionnaires fortement typés
- **Couche réseau** : codec TCP `JTCodec` (paquets collés) et client `JT808Client` avec routage automatique des gestionnaires
- **Extension SuBiao** : alarmes de sécurité active ADAS/DSM/TPMS/BSD, état des périphériques et protocole de pièces jointes
- **Extension YueBiao** : norme de sécurité active du Guangdong avec rapport d'alarme dédié (0x1FC4), passthrough USB, extensions de position et paramètres constructeur
- **Bases JT/T 1078** : modèles de corps de messages audio/vidéo et énumérations
- **Chronotachygraphe** : codec de paquets montants et descendants JT/T 19056
- **Encodage BCD** : attributs de sérialisation personnalisés pour les chaînes BCD et les horodatages
- **Encodage GBK** : codec de chaînes GBK intégré avec enregistrement automatique de `CodePagesEncodingProvider`
- **Gestion par Web API** : envoi de commandes unifié, gestion des sessions en ligne, liste noire et authentification par jeton
- **Abstraction de file de messages** : `IMessageBus` crée les producteurs/consommateurs, les événements de session et les gestionnaires de liaison descendante ; `RedisMessageBus` est le transport interne intégré et les adaptateurs RocketMQ/Kafka de `JT808.Server` s'y branchent via le commutateur de configuration `JT808:Messaging`
- **Livraison de commandes en cluster** : `ClusterState` partage les sessions en ligne, les battements de cœur des nœuds et la liste noire via Redis, achemine les commandes vers le nœud propriétaire du terminal et conserve `ICommandBus` comme point d'extension pour des transports personnalisés
- **Renforcement du client** : reconnexion automatique, battement de cœur configurable et compteurs d'envoi/réception
- **Prise en charge multi-frameworks** : `net45 / net461 / netstandard2.0 / netstandard2.1 / net8.0`
- **Compatible AOT** : projet de test NativeAOT couvrant les chemins de codec, de fabrique et de sérialisation
- **Localisation** : 11 langues à l'exécution d'emblée, texte source anglais en repli, tables compatibles AOT
- **Intégration RocketMQ** : adaptateur producteur/consommateur dans le projet `JT808.Server` (non fourni dans le paquet NuGet), compatible Alibaba Cloud, Huawei Cloud, Tencent Cloud et authentification ACL
- **Intégration Kafka** : adaptateur producteur/consommateur dans le projet `JT808.Server` (non fourni dans le paquet NuGet), basé sur `Confluent.Kafka` avec groupes de consommateurs et prise en charge des ACK

## Versions de protocole prises en charge

| Version | Indicateur | Description |
|---------|------------|-------------|
| JT/T 808-2011 | `Version=0` | Version initiale avec quelques structures de messages différentes |
| JT/T 808-2013 | `Version=0` | Version la plus déployée |
| JT/T 808-2019 | `Version=1` | Dernière version avec de nouveaux champs et types de messages |

## Matrice des messages

| # | ID du message | Nom | Sens | Fait |
|:---:|:------:|---------|:----:|:----:|
| 1 | 0x0001 | Réponse générale du terminal | Montante | ✅ |
| 2 | 0x8001 | Réponse générale de la plateforme | Descendante | ✅ |
| 3 | 0x0002 | Battement de cœur du terminal | Montante | ✅ |
| 4 | 0x8003 | Demande de retransmission de sous-paquet | Descendante | ✅ |
| 5 | 0x0004 | Demande d'interrogation de l'heure du serveur | Montante | ✅ |
| 6 | 0x0100 | Enregistrement du terminal | Montante | ✅ |
| 7 | 0x8100 | Réponse d'enregistrement du terminal | Descendante | ✅ |
| 8 | 0x0003 | Désenregistrement du terminal | Montante | ✅ |
| 9 | 0x0102 | Authentification du terminal | Montante | ✅ |
| 10 | 0x8103 | Définition des paramètres du terminal | Descendante | ✅ |
| 11 | 0x8104 | Interrogation des paramètres du terminal | Descendante | ✅ |
| 12 | 0x0104 | Réponse d'interrogation des paramètres du terminal | Montante | ✅ |
| 13 | 0x8105 | Contrôle du terminal | Descendante | ✅ |
| 14 | 0x8106 | Interrogation de paramètres spécifiés du terminal | Descendante | ✅ |
| 15 | 0x8107 | Interrogation des attributs du terminal | Descendante | ✅ |
| 16 | 0x0107 | Réponse d'interrogation des attributs du terminal | Montante | ✅ |
| 17 | 0x8108 | Livraison du paquet de mise à jour du terminal | Descendante | ✅ |
| 18 | 0x0108 | Notification du résultat de mise à jour du terminal | Montante | ✅ |
| 19 | 0x0200 | Rapport de position | Montante | ✅ |
| 20 | 0x8201 | Interrogation de position | Descendante | ✅ |
| 21 | 0x0201 | Réponse d'interrogation de position | Montante | ✅ |
| 22 | 0x8202 | Contrôle de suivi temporaire de position | Descendante | ✅ |
| 23 | 0x8203 | Confirmation manuelle d'alarme | Descendante | ✅ |
| 24 | 0x8300 | Livraison d'un message texte | Descendante | ✅ |
| 25 | 0x8301 | Configuration d'événement | Descendante | ✅ |
| 26 | 0x0301 | Rapport d'événement | Montante | ✅ |
| 27 | 0x8302 | Livraison d'une question | Descendante | ✅ |
| 28 | 0x0302 | Réponse à la question | Montante | ✅ |
| 29 | 0x8303 | Configuration du menu de services d'information à la demande | Descendante | ✅ |
| 30 | 0x0303 | Information à la demande / annulation | Montante | ✅ |
| 31 | 0x8304 | Service d'information | Descendante | ✅ |
| 32 | 0x8400 | Rappel téléphonique | Descendante | ✅ |
| 33 | 0x8401 | Définition du répertoire téléphonique | Descendante | ✅ |
| 34 | 0x8500 | Contrôle du véhicule | Descendante | ✅ |
| 35 | 0x0500 | Réponse de contrôle du véhicule | Montante | ✅ |
| 36 | 0x8600 | Définition d'une zone circulaire | Descendante | ✅ |
| 37 | 0x8601 | Suppression d'une zone circulaire | Descendante | ✅ |
| 38 | 0x8602 | Définition d'une zone rectangulaire | Descendante | ✅ |
| 39 | 0x8603 | Suppression d'une zone rectangulaire | Descendante | ✅ |
| 40 | 0x8604 | Définition d'une zone polygonale | Descendante | ✅ |
| 41 | 0x8605 | Suppression d'une zone polygonale | Descendante | ✅ |
| 42 | 0x8606 | Définition d'un itinéraire | Descendante | ✅ |
| 43 | 0x8607 | Suppression d'un itinéraire | Descendante | ✅ |
| 44 | 0x8608 | Interrogation des données de zone ou d'itinéraire | Descendante | ✅ |
| 45 | 0x0608 | Réponse d'interrogation des données de zone ou d'itinéraire | Montante | ✅ |
| 46 | 0x8700 | Commande de collecte des données du chronotachygraphe | Descendante | ✅ |
| 47 | 0x0700 | Téléversement des données du chronotachygraphe | Montante | ✅ |
| 48 | 0x8701 | Commande de livraison des paramètres du chronotachygraphe | Descendante | ✅ |
| 49 | 0x0701 | Rapport de bordereau électronique | Montante | ✅ |
| 50 | 0x8702 | Demande de rapport d'identité du conducteur | Descendante | ✅ |
| 51 | 0x0702 | Rapport des informations d'identité du conducteur | Montante | ✅ |
| 52 | 0x0704 | Téléversement de données de position par lots | Montante | ✅ |
| 53 | 0x0705 | Téléversement de données du bus CAN | Montante | ✅ |
| 54 | 0x0800 | Rapport d'événement multimédia | Montante | ✅ |
| 55 | 0x0801 | Téléversement de données multimédias | Montante | ✅ |
| 56 | 0x8800 | Réponse au téléversement de données multimédias | Descendante | ✅ |
| 57 | 0x8801 | Commande de prise de vue immédiate par caméra | Descendante | ✅ |
| 58 | 0x0805 | Réponse de prise de vue immédiate par caméra | Montante | ✅ |
| 59 | 0x8802 | Recherche de données multimédias stockées | Descendante | ✅ |
| 60 | 0x0802 | Réponse de recherche de données multimédias stockées | Montante | ✅ |
| 61 | 0x8803 | Téléversement de données multimédias stockées | Descendante | ✅ |
| 62 | 0x8804 | Commande de démarrage d'enregistrement | Descendante | ✅ |
| 63 | 0x8805 | Téléversement par extraction d'une donnée multimédia stockée | Descendante | ✅ |
| 64 | 0x8900 | Passthrough descendant | Descendante | ✅ |
| 65 | 0x0900 | Passthrough montant | Montante | ✅ |
| 66 | 0x0901 | Rapport de données compressées | Montante | ✅ |
| 67 | 0x8A00 | Clé publique RSA de la plateforme | Descendante | ✅ |
| 68 | 0x0005 | Demande de retransmission de sous-paquet par le terminal | Montante | ✅ |
| 69 | 0x8204 | Détection de liaison | Descendante | ✅ |
| 70 | 0x6006 | Réponse de livraison de message texte | Montante | ✅ |
| 71 | 0x1080 | Message personnalisé | — | ✅ |

## Démarrage rapide

### Installer le paquet NuGet

```bash
dotnet add package NewLife.JT808
```

### Analyser un message montant

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

### Construire un message descendant

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

### Sérialiser manuellement un corps complexe (IAccessor)

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

### Localisation

La bibliothèque fournit 11 langues d'exécution (chinois simplifié/traditionnel, anglais, espagnol, allemand, japonais, français, portugais, russe, coréen et arabe). Les messages sont indexés par le texte source anglais et y reviennent lorsqu'une traduction est manquante.

```csharp
using System.Globalization;
using NewLife;

// Follow the process UI culture by default; pin it explicitly if needed
Localization.Culture = CultureInfo.GetCultureInfo("ja");

// ASP.NET Core hosts can resolve the request language from Accept-Language
using var scope = RequestCultureScope.Create(Request.Headers.AcceptLanguage);
```

### NativeAOT

`MessageFactory.Init()` découvre les corps de messages en analysant l'assembly de protocole. Les applications NativeAOT ou trimmées doivent conserver cet assembly :

```xml
<ItemGroup>
  <TrimmerRootAssembly Include="NewLife.JT808" />
</ItemGroup>
```

Le projet `AotSmoke` de ce dépôt se publie avec NativeAOT et exécute 16 scénarios sous Windows/Linux (x64 et arm64).

## Carte de l'écosystème

NewLife est un écosystème en couches de projets open source versionnés indépendamment. La carte ci-dessous est reprise à l'identique par tous les dépôts NewLife ; chaque projet renvoie à son dépôt et à son paquet NuGet principal.

| Projet | Rôle | Téléchargements |
| --- | --- | --- |
| **Fondation** | | |
| [NewLife.Core](https://github.com/NewLifeX/X) | Journalisation, configuration, cache, réseau, sérialisation, ordonnancement et traçage APM. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Core?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Core) |
| [NewLife.XCode](https://github.com/NewLifeX/NewLife.XCode) | Intergiciel de données : tables uniques à l'échelle de dizaines de milliards, partitionnement automatique, séparation lecture/écriture et plus de 10 bases de données. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.XCode?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.XCode) |
| [NewLife.Net](https://github.com/NewLifeX/NewLife.Net) | Bibliothèque réseau hautes performances : 1,77 M pps / 14,5 Gbps sur une machine (TCP 1KB, 2026-10), 4 M connexions TCP persistantes. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Net?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Net) |
| [NewLife.Remoting](https://github.com/NewLifeX/NewLife.Remoting) | Communication protocolaire et framework RPC pour applications client/serveur et appareils IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Remoting?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Remoting) |
| [NewLife.Agent](https://github.com/NewLifeX/NewLife.Agent) | Installe des applications .NET comme démons système : services Windows et systemd Linux. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Agent?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Agent) |
| [NewLife.Templates](https://github.com/NewLifeX/NewLife.Zero) | Modèles de projet `dotnet new` pour console, web, API, service, données et IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Templates?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Templates) |
| **Données et messagerie** | | |
| [NewLife.Redis](https://github.com/NewLifeX/NewLife.Redis) | Client Redis avec une latence en microsecondes, des millions d'opérations/s et des files de messages intégrées. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Redis?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Redis) |
| [NewLife.RocketMQ](https://github.com/NewLifeX/NewLife.RocketMQ) | Client RocketMQ entièrement managé pour Apache RocketMQ et Alibaba Cloud Message Queue. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.RocketMQ?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.RocketMQ) |
| [NewLife.MQTT](https://github.com/NewLifeX/NewLife.MQTT) | Client et serveur MQTT, y compris la connectivité Alibaba Cloud IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.MQTT?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.MQTT) |
| [NewLife.NovaDb](https://github.com/NewLifeX/NewLife.NovaDb) | Base de données hybride en C# : embarquée ou en réseau, relationnelle, séries temporelles, file de messages et KV réunis. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.NovaDb?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.NovaDb) |
| [NewLife.MySql](https://github.com/NewLifeX/NewLife.MySql) | Pilote et couche d'assistance MySql construits sur la pile de données NewLife. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.MySql?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.MySql) |
| **Web, IA et distribué** | | |
| [NewLife.Cube](https://github.com/NewLifeX/NewLife.Cube) | Framework rapide de plateforme d'administration : permissions, SSO, serveur OAuth et échafaudage CRUD. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Cube?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Cube) |
| [Stardust](https://github.com/NewLifeX/Stardust) | Plateforme de services distribués : gestion des nœuds, APM, centre de configuration, registre et centre de publication. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Stardust?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Stardust) |
| [AntJob](https://github.com/NewLifeX/AntJob) | Plateforme de tâches distribuées et d'ordonnancement de données en temps réel avec partitionnement, reprises et backfill. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.AntJob?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.AntJob) |
| [NewLife.AI](https://github.com/NewLifeX/NewLife.AI) | Client unifié pour 46 fournisseurs de LLM, plus ChatAI, une application web de chat prête à l'emploi. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.AI?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.AI) |
| **Industriel et IoT** | | |
| [NewLife.IoT](https://github.com/NewLifeX/NewLife.IoT) | Bibliothèque standard IoT définissant les contrats de protocole et les abstractions d'appareils. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.IoT?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.IoT) |
| [NewLife.Modbus](https://github.com/NewLifeX/NewLife.Modbus) | Implémentation Modbus TCP/RTU/ASCII au-dessus de NewLife.IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Modbus?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Modbus) |
| [NewLife.Siemens](https://github.com/NewLifeX/NewLife.Siemens) | Protocole pour automate Siemens au-dessus de NewLife.IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Siemens?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Siemens) |
| [NewLife.Omron](https://github.com/NewLifeX/NewLife.Omron) | Protocole pour automate Omron au-dessus de NewLife.IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Omron?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Omron) |
| [NewLife.Melsec](https://github.com/NewLifeX/NewLife.Melsec) | Protocole pour automate Mitsubishi Melsec au-dessus de NewLife.IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Melsec?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Melsec) |
| [NewLife.OPC](https://github.com/NewLifeX/NewLife.OPC) | Client OPC pour l'accès aux données industrielles. | — |
| [NewLife.BACnet](https://github.com/NewLifeX/NewLife.BACnet) | Pile de protocoles BACnet pour l'automatisation des bâtiments. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.BACnet?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.BACnet) |
| [ZeroIoT](https://github.com/NewLifeX/ZeroIoT) | Plateforme IoT open source légère avec gestion des passerelles et des appareils. | — |
| **Télématique embarquée** | | |
| [NewLife.JT808](https://github.com/NewLifeX/NewLife.JT808) | Protocole de positionnement de véhicules JT/T 808 : plus de 70 corps de message, 2011/2013/2019. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.JT808?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.JT808) |
| [NewLife.JT809](https://github.com/NewLifeX/NewLife.JT809) | Protocole plateforme à plateforme JT/T 809 pour les autorités de transport. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.JT809?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.JT809) |
| [NewLife.JT1078](https://github.com/NewLifeX/NewLife.JT1078) | Vidéo embarquée JT/T 1078 : analyse RTP, remuxage FLV/fMP4 et diffusion en direct. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.JT1078?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.JT1078) |
| **Composants métier** | | |
| [NewLife.Office](https://github.com/NewLifeX/NewLife.Office) | Un seul paquet pour Excel, Word, PPT, PDF et plus de 15 formats de documents. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Office?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Office) |
| [NewLife.Audio](https://github.com/NewLifeX/NewLife.Audio) | Audio multiplateforme : codecs, E/S d'appareils, DSP, conteneurs et streaming. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Audio?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Audio) |
| [NewLife.Video](https://github.com/NewLifeX/NewLife.Video) | Codecs vidéo, formats de conteneurs, traitement des images et streaming. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Video?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Video) |
| [NewLife.Map](https://github.com/NewLifeX/NewLife.Map) | Interface unifiée pour Baidu, AMap, Tencent et Tianditu avec géocodage et conversion de coordonnées. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Map?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Map) |
| [NewLife.IP](https://github.com/NewLifeX/NewLife.IP) | Géolocalisation IPv4 avec une base de données hors ligne mappée en mémoire et une API en ligne en secours. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.IP?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.IP) |
| **Outils et produits** | | |
| [CrazyCoder](https://github.com/NewLifeX/XCoder) | Boîte à outils du développeur : réseau, port série, cryptographie, regex, Modbus et MQTT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.XCoder?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.XCoder) |
| [GitCandy](https://github.com/NewLifeX/GitCandy) | Système de gestion de code source Git auto-hébergé. | — |
| [LuckyClover](https://github.com/NewLifeX/LuckyClover) | Installe automatiquement le runtime .NET correspondant lorsqu'il est lancé depuis un hôte plus ancien. | — |

> Les badges indiquent le paquet principal de chaque projet ; les paquets de thèmes, d'extensions et satellites sont répertoriés sur leurs pages NuGet. Les plus de 30 paquets non listés ici (FTP, LoRa, extensions Office, pilotes de protocoles, etc.) sont disponibles dans l'[organisation NewLifeX](https://github.com/NewLifeX) et sur [newlifex.com](https://newlifex.com).

## Équipe

<img src="https://newlifex.com/logo.png" width="48" align="right" alt="NewLife" />

Fondée en 2002, l'équipe NewLife œuvre depuis plus de deux décennies dans l'écosystème .NET et IoT, maintient près de 100 projets open source (dont 34 essentiels à cette carte) et fournit des solutions matérielles et logicielles intégrées aux secteurs de l'énergie, de la logistique, de la santé, du contrôle industriel et d'Internet. Notre devise : « l'apprentissage ne connaît pas l'ancienneté — celui qui maîtrise devient maître ».

Les composants open source ont été téléchargés <!--nuget-total:start-->6.48M<!--nuget-total:end--> fois depuis NuGet (à l'échelle de l'écosystème, en date de 2026-10).

Site web : https://newlifex.com · Organisation open source : https://github.com/NewLifeX

<details>
<summary>Compte officiel WeChat : 智能大石头 (scannez pour suivre)</summary>

![智能大石头](https://newlifex.com/stone.jpg)

</details>

## Licence

Ce projet est distribué sous licence [MIT](LICENSE) ; vous pouvez l'utiliser, le modifier et le redistribuer librement, à condition de conserver la mention de copyright.

> Si ce projet vous est utile, n'hésitez pas à lui donner une Star, à le partager avec votre équipe ou à proposer des améliorations via Issue/PR.
