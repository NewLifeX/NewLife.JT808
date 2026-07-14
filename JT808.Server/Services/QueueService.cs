using NewLife;
using NewLife.Caching;
using NewLife.Log;

namespace JT808.Server.Services;

/// <summary>Redis Stream 消息队列生产者服务</summary>
/// <remarks>
/// 参考 GPS808 的 QueueService 模式，管理多个 Redis Stream 主题的生产者。
/// </remarks>
public class QueueService
{
    private readonly ICacheProvider _cacheProvider;

    public QueueService(ICacheProvider cacheProvider)
    {
        _cacheProvider = cacheProvider;
    }

    private IProducerConsumer<Object>? _position;
    /// <summary>推送位置数据到 Redis Stream</summary>
    public void AddPosition(Object data)
    {
        _position ??= _cacheProvider.GetQueue<Object>("PositionData", "Group");
        if (_position == null) return;
        _position.Add(data);
    }

    private IProducerConsumer<Object>? _sensor;
    /// <summary>推送传感器数据到 Redis Stream</summary>
    public void AddSensor(Object data)
    {
        _sensor ??= _cacheProvider.GetQueue<Object>("SensorData", "Group");
        if (_sensor == null) return;
        _sensor.Add(data);
    }

    private IProducerConsumer<Object>? _event;
    /// <summary>推送事件数据到 Redis Stream</summary>
    public void AddEvent(Object data)
    {
        _event ??= _cacheProvider.GetQueue<Object>("EventData", "Group");
        if (_event == null) return;
        _event.Add(data);
    }

    private IProducerConsumer<Object>? _adas;
    /// <summary>推送 ADAS 报警到 Redis Stream</summary>
    public void AddAdasAlarm(Object data)
    {
        _adas ??= _cacheProvider.GetQueue<Object>("ADASAlarm", "Group");
        if (_adas == null) return;
        _adas.Add(data);
    }

    private IProducerConsumer<Object>? _dsm;
    /// <summary>推送 DSM 报警到 Redis Stream</summary>
    public void AddDsmAlarm(Object data)
    {
        _dsm ??= _cacheProvider.GetQueue<Object>("DSMAlarm", "Group");
        if (_dsm == null) return;
        _dsm.Add(data);
    }

    private IProducerConsumer<Object>? _bsd;
    /// <summary>推送 BSD 报警到 Redis Stream</summary>
    public void AddBsdAlarm(Object data)
    {
        _bsd ??= _cacheProvider.GetQueue<Object>("BSDAlarm", "Group");
        if (_bsd == null) return;
        _bsd.Add(data);
    }

    private IProducerConsumer<Object>? _rawData;
    /// <summary>推送原始报文到 Redis Stream</summary>
    public void AddRawData(Object data)
    {
        _rawData ??= _cacheProvider.GetQueue<Object>("RawData", "Group");
        if (_rawData == null) return;
        _rawData.Add(data);
    }

    private IProducerConsumer<Object>? _command;
    /// <summary>推送指令到 Redis 队列（Web→Server 通道）</summary>
    public void AddCommand(Object data)
    {
        _command ??= _cacheProvider.GetQueue<Object>("Command", "Group");
        if (_command == null) return;
        _command.Add(data);
    }
}
