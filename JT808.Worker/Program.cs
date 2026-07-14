using JT808.Data;
using JT808.Worker.Workers;
using NewLife;
using NewLife.Caching;
using NewLife.Caching.Services;
using NewLife.Log;
using NewLife.Model;
using Stardust;
using Stardust.Extensions;
using XCode;
using XCode.DataAccessLayer;

XTrace.UseConsole();

var services = ObjectContainer.Current;

// 星尘注册
var star = services.AddStardust();

// Redis 分布式缓存
services.AddSingleton<ICacheProvider, RedisCacheProvider>();

// 数据库连接
var set = NewLife.Setting.Current;
if (set.IsNew)
{
    set.DataPath = "../Data";
    set.Save();
}

if (!DAL.ConnStrs.ContainsKey("GPS"))
    DAL.AddConnStr("GPS", "Server=127.0.0.1;Port=3306;Database=JT808;User=root;Password=root;", null, "MySql");
if (!DAL.ConnStrs.ContainsKey("Safety")) DAL.AddConnStr("Safety", "MapTo=GPS", null, "MySql");
if (!DAL.ConnStrs.ContainsKey("Platform")) DAL.AddConnStr("Platform", "MapTo=GPS", null, "MySql");

EntityFactory.InitConnection("GPS");
EntityFactory.InitConnection("Safety");
EntityFactory.InitConnection("Platform");

services.AddSingleton<QueueService>();

var host = services.BuildHost();

// 注册后台 Worker
host.Add<PositionWorker>();

await host.RunAsync();

/// <summary>Redis Stream 消费者服务</summary>
public class QueueService
{
    private readonly ICacheProvider _cacheProvider;

    public QueueService(ICacheProvider cacheProvider)
    {
        _cacheProvider = cacheProvider;
    }

    public IProducerConsumer<T>? Create<T>(String topic, String? group = null)
    {
        var queue = _cacheProvider.GetQueue<T>(topic, group ?? "Group");
        return queue as IProducerConsumer<T>;
    }
}
