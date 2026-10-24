# NewLife.JT808 - Протокол спутникового мониторинга транспорта JT/T 808

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
🌐 [English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [Español](README.es.md) | [Deutsch](README.de.md) | [日本語](README.ja.md) | [Français](README.fr.md) | [Português](README.pt.md) | [한국어](README.ko.md) | [العربية](README.ar.md)
<!-- lang:end -->

> Этот перевод может отставать; см. [English README](README.md).

Полная реализация протокола мониторинга транспорта JT/T 808: версии 2011/2013/2019 с более чем 70 стандартными телами сообщений, расширения активной безопасности SuBiao (Цзянсу) и YueBiao (Гуандун), сообщения аудио/видео JT/T 1078, протокол тахографа JT/T 19056 и интеграция с очередями сообщений.

Источник: https://github.com/NewLifeX/NewLife.JT808  
NuGet: [NewLife.JT808](https://www.nuget.org/packages/NewLife.JT808)  
Документация: [Требования](Doc/需求文档.md) / [Архитектура](Doc/架构设计.md) / [Список функций](Doc/功能清单.md) / [Руководство пользователя](Doc/使用文档.md) / [Локализация](Doc/多语言本地化.md)

## Основные возможности

- **Полный протокол**: разбор кадров JT/T 808-2011/2013/2019 с разделителями 0x7E, экранированием 0x7D, XOR-контрольной суммой и сборкой подпакетов
- **Богатый набор тел сообщений**: более 70 стандартных тел сообщений (0x0001~0x8A00) как для восходящего потока от терминалов, так и для нисходящего от платформы
- **Совместимость трёх версий**: автоматическая адаптация полей через поле `Version` и атрибуты `[JT2011]`/`[JT2019]`
- **Два режима сериализации**: рефлексия на основе атрибутов для простых тел, `IAccessor` для сложных тел, требующих ручного управления
- **Разбор TLV/KLV**: встроенные кодеки Type-Length-Value и Kind-Length-Value с преобразованием в строго типизированные словари
- **Сетевой уровень**: кодек TCP с обработкой склеенных пакетов `JTCodec` и `JT808Client` с автоматической маршрутизацией обработчиков
- **Расширение SuBiao**: тревоги активной безопасности ADAS/DSM/TPMS/BSD, статус периферии и протокол вложений
- **Расширение YueBiao**: стандарт активной безопасности Гуандуна с выделенной отправкой тревог (0x1FC4), USB pass-through, расширения позиции и параметры производителя
- **Основы JT/T 1078**: модели тел сообщений и перечисления аудио/видео
- **Тахограф**: кодек восходящих и нисходящих пакетов JT/T 19056
- **Кодировка BCD**: пользовательские атрибуты сериализации для BCD-строк и меток времени
- **Кодировка GBK**: встроенный кодек строк GBK с автоматической регистрацией `CodePagesEncodingProvider`
- **Управление через Web API**: единая отправка команд, управление онлайн-сессиями, чёрный список и аутентификация по токену
- **Абстракция очереди сообщений**: `IMessageBus` создаёт производителей/потребителей, события сессий и обработчики нисходящего потока; `RedisMessageBus` — встроенный внутренний транспорт, а адаптеры RocketMQ/Kafka в `JT808.Server` подключаются через переключатель конфигурации `JT808:Messaging`
- **Доставка команд в кластере**: `ClusterState` разделяет онлайн-сессии, heartbeat узлов и чёрный список через Redis, маршрутизирует команды на узел, владеющий терминалом, и оставляет `ICommandBus` точкой расширения для собственных транспортов
- **Устойчивость клиента**: автоматическое переподключение, настраиваемый heartbeat и счётчики отправленных/принятых пакетов
- **Поддержка нескольких платформ**: `net45 / net461 / netstandard2.0 / netstandard2.1 / net8.0`
- **Дружественность к AOT**: проект NativeAOT-проверки, охватывающий кодеки, фабрику и пути сериализации
- **Локализация**: 11 языков во время выполнения «из коробки», английский исходный текст как резерв, AOT-безопасные таблицы
- **Интеграция с RocketMQ**: адаптер производителя/потребителя в проекте `JT808.Server` (не входит в пакет NuGet), поддержка Alibaba Cloud, Huawei Cloud, Tencent Cloud и аутентификации ACL
- **Интеграция с Kafka**: адаптер производителя/потребителя в проекте `JT808.Server` (не входит в пакет NuGet), на основе `Confluent.Kafka` с группами потребителей и поддержкой ACK

## Поддерживаемые версии протокола

| Версия | Признак | Описание |
|---------|------|-------------|
| JT/T 808-2011 | `Version=0` | Ранняя версия с некоторыми отличиями в структуре сообщений |
| JT/T 808-2013 | `Version=0` | Наиболее широко развёрнутая версия |
| JT/T 808-2019 | `Version=1` | Новейшая версия с новыми полями и типами сообщений |

## Матрица сообщений

| № | ID сообщения | Название | Направление | Готово |
|:---:|:------:|---------|:----:|:----:|
| 1 | 0x0001 | Общий ответ терминала | Восходящий | ✅ |
| 2 | 0x8001 | Общий ответ платформы | Нисходящий | ✅ |
| 3 | 0x0002 | Heartbeat терминала | Восходящий | ✅ |
| 4 | 0x8003 | Запрос повторной передачи подпакета | Нисходящий | ✅ |
| 5 | 0x0004 | Запрос времени сервера | Восходящий | ✅ |
| 6 | 0x0100 | Регистрация терминала | Восходящий | ✅ |
| 7 | 0x8100 | Ответ на регистрацию терминала | Нисходящий | ✅ |
| 8 | 0x0003 | Дерегистрация терминала | Восходящий | ✅ |
| 9 | 0x0102 | Аутентификация терминала | Восходящий | ✅ |
| 10 | 0x8103 | Установка параметров терминала | Нисходящий | ✅ |
| 11 | 0x8104 | Запрос параметров терминала | Нисходящий | ✅ |
| 12 | 0x0104 | Ответ на запрос параметров терминала | Восходящий | ✅ |
| 13 | 0x8105 | Управление терминалом | Нисходящий | ✅ |
| 14 | 0x8106 | Запрос заданных параметров терминала | Нисходящий | ✅ |
| 15 | 0x8107 | Запрос атрибутов терминала | Нисходящий | ✅ |
| 16 | 0x0107 | Ответ на запрос атрибутов терминала | Восходящий | ✅ |
| 17 | 0x8108 | Доставка пакета обновления терминала | Нисходящий | ✅ |
| 18 | 0x0108 | Уведомление о результате обновления терминала | Восходящий | ✅ |
| 19 | 0x0200 | Донесение о позиции | Восходящий | ✅ |
| 20 | 0x8201 | Запрос позиции | Нисходящий | ✅ |
| 21 | 0x0201 | Ответ на запрос позиции | Восходящий | ✅ |
| 22 | 0x8202 | Управление временным отслеживанием позиции | Нисходящий | ✅ |
| 23 | 0x8203 | Ручное подтверждение тревоги | Нисходящий | ✅ |
| 24 | 0x8300 | Доставка текстового сообщения | Нисходящий | ✅ |
| 25 | 0x8301 | Настройка событий | Нисходящий | ✅ |
| 26 | 0x0301 | Донесение о событии | Восходящий | ✅ |
| 27 | 0x8302 | Доставка вопроса | Нисходящий | ✅ |
| 28 | 0x0302 | Ответ на вопрос | Восходящий | ✅ |
| 29 | 0x8303 | Настройка меню информационного запроса | Нисходящий | ✅ |
| 30 | 0x0303 | Информационный запрос / отмена | Восходящий | ✅ |
| 31 | 0x8304 | Информационная услуга | Нисходящий | ✅ |
| 32 | 0x8400 | Телефонный звонок | Нисходящий | ✅ |
| 33 | 0x8401 | Настройка телефонной книги | Нисходящий | ✅ |
| 34 | 0x8500 | Управление автомобилем | Нисходящий | ✅ |
| 35 | 0x0500 | Ответ на управление автомобилем | Восходящий | ✅ |
| 36 | 0x8600 | Задание круговой области | Нисходящий | ✅ |
| 37 | 0x8601 | Удаление круговой области | Нисходящий | ✅ |
| 38 | 0x8602 | Задание прямоугольной области | Нисходящий | ✅ |
| 39 | 0x8603 | Удаление прямоугольной области | Нисходящий | ✅ |
| 40 | 0x8604 | Задание многоугольной области | Нисходящий | ✅ |
| 41 | 0x8605 | Удаление многоугольной области | Нисходящий | ✅ |
| 42 | 0x8606 | Задание маршрута | Нисходящий | ✅ |
| 43 | 0x8607 | Удаление маршрута | Нисходящий | ✅ |
| 44 | 0x8608 | Запрос данных области или маршрута | Нисходящий | ✅ |
| 45 | 0x0608 | Ответ на запрос данных области или маршрута | Восходящий | ✅ |
| 46 | 0x8700 | Команда сбора данных тахографа | Нисходящий | ✅ |
| 47 | 0x0700 | Выгрузка данных тахографа | Восходящий | ✅ |
| 48 | 0x8701 | Команда доставки параметров тахографа | Нисходящий | ✅ |
| 49 | 0x0701 | Донесение электронной путевой ведомости | Восходящий | ✅ |
| 50 | 0x8702 | Запрос отчёта идентификации водителя | Нисходящий | ✅ |
| 51 | 0x0702 | Донесение информации идентификации водителя | Восходящий | ✅ |
| 52 | 0x0704 | Пакетная выгрузка данных позиционирования | Восходящий | ✅ |
| 53 | 0x0705 | Выгрузка данных шины CAN | Восходящий | ✅ |
| 54 | 0x0800 | Донесение о мультимедийном событии | Восходящий | ✅ |
| 55 | 0x0801 | Выгрузка мультимедийных данных | Восходящий | ✅ |
| 56 | 0x8800 | Ответ на выгрузку мультимедийных данных | Нисходящий | ✅ |
| 57 | 0x8801 | Команда немедленной съёмки камерой | Нисходящий | ✅ |
| 58 | 0x0805 | Ответ на немедленную съёмку камерой | Восходящий | ✅ |
| 59 | 0x8802 | Поиск сохранённых мультимедийных данных | Нисходящий | ✅ |
| 60 | 0x0802 | Ответ на поиск сохранённых мультимедийных данных | Восходящий | ✅ |
| 61 | 0x8803 | Выгрузка сохранённых мультимедийных данных | Нисходящий | ✅ |
| 62 | 0x8804 | Команда начала записи | Нисходящий | ✅ |
| 63 | 0x8805 | Выгрузка запрошенных сохранённых мультимедийных данных | Нисходящий | ✅ |
| 64 | 0x8900 | Нисходящий pass-through | Нисходящий | ✅ |
| 65 | 0x0900 | Восходящий pass-through | Восходящий | ✅ |
| 66 | 0x0901 | Донесение сжатых данных | Восходящий | ✅ |
| 67 | 0x8A00 | Открытый ключ RSA платформы | Нисходящий | ✅ |
| 68 | 0x0005 | Запрос терминала на повторную передачу подпакета | Восходящий | ✅ |
| 69 | 0x8204 | Проверка канала связи | Нисходящий | ✅ |
| 70 | 0x6006 | Ответ на доставку текстового сообщения | Восходящий | ✅ |
| 71 | 0x1080 | Пользовательское сообщение | — | ✅ |

## Быстрый старт

### Установка пакета NuGet

```bash
dotnet add package NewLife.JT808
```

### Разбор восходящего сообщения

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

### Построение нисходящего сообщения

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

### Ручная сериализация сложного тела (IAccessor)

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

### Локализация

Библиотека поставляется с 11 языками во время выполнения (упрощённый и традиционный китайский, английский, испанский, немецкий, японский, французский, португальский, русский, корейский и арабский). Сообщения ключуются по исходному английскому тексту и используют его как резерв при отсутствии перевода.

```csharp
using System.Globalization;
using NewLife;

// Follow the process UI culture by default; pin it explicitly if needed
Localization.Culture = CultureInfo.GetCultureInfo("ja");

// ASP.NET Core hosts can resolve the request language from Accept-Language
using var scope = RequestCultureScope.Create(Request.Headers.AcceptLanguage);
```

### NativeAOT

`MessageFactory.Init()` обнаруживает тела сообщений сканированием сборки протокола. Приложения NativeAOT или с триммингом должны сохранять эту сборку:

```xml
<ItemGroup>
  <TrimmerRootAssembly Include="NewLife.JT808" />
</ItemGroup>
```

Проект `AotSmoke` в этом репозитории публикуется с NativeAOT и выполняет 16 сценариев в Windows/Linux (x64 и arm64).

## Карта экосистемы

NewLife — это многоуровневая экосистема независимо версионируемых open-source проектов. Карта ниже дословно используется во всех репозиториях NewLife; каждый проект ссылается на свой репозиторий и свой основной пакет NuGet.

| Проект | Что делает | Загрузки |
| --- | --- | --- |
| **Базовый** | | |
| [NewLife.Core](https://github.com/NewLifeX/X) | Логирование, конфигурация, кэширование, сеть, сериализация, планирование и APM-трассировка. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Core?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Core) |
| [NewLife.XCode](https://github.com/NewLifeX/NewLife.XCode) | Промежуточное ПО данных: отдельные таблицы масштаба десятков миллиардов строк, автоматическое секционирование, разделение чтения и записи, более 10 баз данных. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.XCode?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.XCode) |
| [NewLife.Net](https://github.com/NewLifeX/NewLife.Net) | Высокопроизводительная сетевая библиотека: 1,77M pps / 14,5 Гбит/с на машину (TCP 1KB, 2026-10), 4M долгоживущих TCP-соединений. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Net?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Net) |
| [NewLife.Remoting](https://github.com/NewLifeX/NewLife.Remoting) | Протокольная связь и RPC-фреймворк для клиент-серверных приложений и IoT-устройств. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Remoting?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Remoting) |
| [NewLife.Agent](https://github.com/NewLifeX/NewLife.Agent) | Устанавливает .NET-приложения как демоны ОС: службы Windows и systemd в Linux. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Agent?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Agent) |
| [NewLife.Templates](https://github.com/NewLifeX/NewLife.Zero) | Шаблоны проектов `dotnet new` для консоли, web, API, службы, данных и IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Templates?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Templates) |
| **Данные и обмен сообщениями** | | |
| [NewLife.Redis](https://github.com/NewLifeX/NewLife.Redis) | Клиент Redis с микросекундной задержкой, миллионами операций в секунду и встроенными очередями сообщений. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Redis?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Redis) |
| [NewLife.RocketMQ](https://github.com/NewLifeX/NewLife.RocketMQ) | Полностью управляемый клиент RocketMQ для Apache RocketMQ и Alibaba Cloud Message Queue. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.RocketMQ?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.RocketMQ) |
| [NewLife.MQTT](https://github.com/NewLifeX/NewLife.MQTT) | Клиент и сервер MQTT, включая подключение к Alibaba Cloud IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.MQTT?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.MQTT) |
| [NewLife.NovaDb](https://github.com/NewLifeX/NewLife.NovaDb) | Гибридная база данных на C#: встроенная или сетевая, реляционная, временных рядов, очередь сообщений и KV в одном. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.NovaDb?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.NovaDb) |
| [NewLife.MySql](https://github.com/NewLifeX/NewLife.MySql) | Драйвер MySql и вспомогательный слой на основе стека данных NewLife. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.MySql?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.MySql) |
| **Web, AI и распределённые системы** | | |
| [NewLife.Cube](https://github.com/NewLifeX/NewLife.Cube) | Фреймворк быстрой разработки админ-платформ: права, SSO, сервер OAuth и CRUD-скаффолдинг. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Cube?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Cube) |
| [Stardust](https://github.com/NewLifeX/Stardust) | Распределённая сервисная платформа: управление узлами, APM, центр конфигурации, реестр и центр публикации. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Stardust?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Stardust) |
| [AntJob](https://github.com/NewLifeX/AntJob) | Распределённая платформа задач и планирования данных в реальном времени с секционированием, повторами и дозаполнением. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.AntJob?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.AntJob) |
| [NewLife.AI](https://github.com/NewLifeX/NewLife.AI) | Единый клиент для 46 поставщиков LLM плюс ChatAI — готовое веб-приложение для чата. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.AI?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.AI) |
| **Промышленность и IoT** | | |
| [NewLife.IoT](https://github.com/NewLifeX/NewLife.IoT) | Стандартная библиотека IoT, определяющая контракты протоколов и абстракции устройств. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.IoT?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.IoT) |
| [NewLife.Modbus](https://github.com/NewLifeX/NewLife.Modbus) | Реализация Modbus TCP/RTU/ASCII поверх NewLife.IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Modbus?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Modbus) |
| [NewLife.Siemens](https://github.com/NewLifeX/NewLife.Siemens) | Протокол Siemens PLC поверх NewLife.IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Siemens?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Siemens) |
| [NewLife.Omron](https://github.com/NewLifeX/NewLife.Omron) | Протокол Omron PLC поверх NewLife.IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Omron?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Omron) |
| [NewLife.Melsec](https://github.com/NewLifeX/NewLife.Melsec) | Протокол Mitsubishi Melsec PLC поверх NewLife.IoT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Melsec?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Melsec) |
| [NewLife.OPC](https://github.com/NewLifeX/NewLife.OPC) | Клиент OPC для доступа к промышленным данным. | — |
| [NewLife.BACnet](https://github.com/NewLifeX/NewLife.BACnet) | Стек протокола автоматизации зданий BACnet. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.BACnet?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.BACnet) |
| [ZeroIoT](https://github.com/NewLifeX/ZeroIoT) | Лёгкая open-source платформа IoT с управлением шлюзами и устройствами. | — |
| **Телематика транспорта** | | |
| [NewLife.JT808](https://github.com/NewLifeX/NewLife.JT808) | Протокол позиционирования транспорта JT/T 808: более 70 тел сообщений, 2011/2013/2019. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.JT808?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.JT808) |
| [NewLife.JT809](https://github.com/NewLifeX/NewLife.JT809) | Межплатформенный протокол JT/T 809 для органов управления транспортом. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.JT809?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.JT809) |
| [NewLife.JT1078](https://github.com/NewLifeX/NewLife.JT1078) | Автомобильное видео JT/T 1078: разбор RTP, ремультиплексирование в FLV/fMP4 и прямая трансляция. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.JT1078?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.JT1078) |
| **Прикладные компоненты** | | |
| [NewLife.Office](https://github.com/NewLifeX/NewLife.Office) | Один пакет для Excel, Word, PPT, PDF и более 15 форматов документов. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Office?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Office) |
| [NewLife.Audio](https://github.com/NewLifeX/NewLife.Audio) | Кроссплатформенное аудио: кодеки, ввод-вывод устройств, DSP, контейнеры и потоковая передача. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Audio?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Audio) |
| [NewLife.Video](https://github.com/NewLifeX/NewLife.Video) | Видеокодеки, форматы контейнеров, обработка кадров и потоковая передача. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Video?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Video) |
| [NewLife.Map](https://github.com/NewLifeX/NewLife.Map) | Единый интерфейс к Baidu, AMap, Tencent и Tianditu с геокодированием и преобразованием координат. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.Map?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.Map) |
| [NewLife.IP](https://github.com/NewLifeX/NewLife.IP) | Геолокация IPv4 с офлайн-базой, отображаемой в память, и онлайн-API как резервом. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.IP?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.IP) |
| **Инструменты и продукты** | | |
| [CrazyCoder](https://github.com/NewLifeX/XCoder) | Инструменты разработчика: сеть, последовательный порт, шифрование, регулярные выражения, Modbus и MQTT. | [![NuGet](https://img.shields.io/nuget/dt/NewLife.XCoder?style=flat-square&logo=nuget&label=downloads)](https://www.nuget.org/packages/NewLife.XCoder) |
| [GitCandy](https://github.com/NewLifeX/GitCandy) | Самостоятельно размещаемая система управления исходным кодом Git. | — |
| [LuckyClover](https://github.com/NewLifeX/LuckyClover) | Автоматически устанавливает подходящую среду выполнения .NET при запуске на старом хосте. | — |

> Бейджи показывают основной пакет каждого проекта; пакеты тем, расширений и сателлитов перечислены на соответствующих страницах NuGet. Более 30 пакетов, не перечисленных здесь (FTP, LoRa, расширения Office, драйверы протоколов и другие), доступны в [организации NewLifeX](https://github.com/NewLifeX) и на [newlifex.com](https://newlifex.com).

## Команда

<img src="https://newlifex.com/logo.png" width="48" align="right" alt="NewLife" />

Команда NewLife основана в 2002 году и более двух десятилетий работает в экосистеме .NET и IoT, поддерживает около 100 проектов с открытым кодом (34 из них — основные в этой карте) и предлагает интегрированные программно-аппаратные решения для энергетики, логистики, здравоохранения, промышленной автоматизации и интернет-отрасли. Мы следуем девизу «в учении нет старших — мастерство приносит уважение».

Open-source компоненты скачали <!--nuget-total:start-->6.48M<!--nuget-total:end--> раз с NuGet (по всей экосистеме, по состоянию на 2026-10).

Сайт: https://newlifex.com · Организация open source: https://github.com/NewLifeX

<details>
<summary>Официальный аккаунт WeChat: 智能大石头 (отсканируйте, чтобы подписаться)</summary>

![智能大石头](https://newlifex.com/stone.jpg)

</details>

## Лицензия

Проект распространяется по лицензии [MIT](LICENSE); вы можете свободно использовать, изменять и распространять его при сохранении уведомления об авторских правах.

> Если проект оказался полезен, поставьте Star, поделитесь им с командой или предложите улучшения через Issue/PR.
