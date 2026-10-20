using JT808.Data;
using JT808.Worker.Workers;
using NewLife;
using NewLife.Caching;
using NewLife.Caching.Services;
using NewLife.JT808.Protocols;
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

// 消息总线：消费端使用 Redis Stream，与网关生产端保持同一主题与消费组
services.AddSingleton<IMessageBus, RedisMessageBus>();

var host = services.BuildHost();

// 注册后台 Worker（每个 Worker 对应一个 Redis Stream 消费组）
host.Add<PositionWorker>();
host.Add<SensorWorker>();
host.Add<EventWorker>();
host.Add<ADASAlarmWorker>();
host.Add<DSMAlarmWorker>();
host.Add<BSDAlarmWorker>();
host.Add<TirePressureWorker>();

XTrace.WriteLine("JT808.Worker 已启动，共 7 个 Worker");

await host.RunAsync();
