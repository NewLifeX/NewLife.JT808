# NewLife.JT808 - Protocole de localisation de véhicules JT/T 808

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
🌐 [English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [Español](README.es.md) | [Deutsch](README.de.md) | [日本語](README.ja.md) | [Português](README.pt.md) | [Русский](README.ru.md) | [한국어](README.ko.md) | [العربية](README.ar.md)
<!-- lang:end -->

> Cette traduction peut être obsolète ; reportez-vous au [English README](README.md).

Implémentation complète du protocole de localisation de véhicules JT/T 808 (versions 2011/2013/2019) : plus de 70 corps de messages standard, extensions de sécurité active SuBiao/YueBiao, messages audio/vidéo JT/T 1078, protocole de chronotachygraphe JT/T 19056 et intégration aux files de messages.

## Fonctionnalités clés

- **Protocole complet** : délimiteurs 0x7E, échappement 0x7D, somme de contrôle XOR et réassemblage des sous-paquets
- **Messages riches** : plus de 70 corps standard en liaison montante et descendante
- **Trois versions** : adaptation automatique via `Version` et `[JT2011]`/`[JT2019]`
- **Double sérialisation** : pilotée par attributs et `IAccessor` pour les corps complexes
- **TLV/KLV** : codecs génériques avec dictionnaires fortement typés
- **Couche réseau** : codec `JTCodec` et client `JT808Client` avec routage automatique
- **Compatible AOT** : projet de test NativeAOT sur les chemins principaux
- **Localisation** : 11 langues au moment de l'exécution, repli sur l'anglais

## Versions du protocole

| Version | Indicateur | Description |
|---------|------------|-------------|
| JT/T 808-2011 | `Version=0` | Version initiale avec quelques structures différentes |
| JT/T 808-2013 | `Version=0` | Version la plus déployée |
| JT/T 808-2019 | `Version=1` | Dernière version avec de nouveaux champs et messages |

## Installation

```bash
dotnet add package NewLife.JT808
```

## Démarrage rapide

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

## Liens

- Code source: https://github.com/NewLifeX/NewLife.JT808
- NuGet: https://www.nuget.org/packages/NewLife.JT808
- Documentation: [English](README.md) | [简体中文](README.zh-CN.md)

> NewLife Team · https://newlifex.com
