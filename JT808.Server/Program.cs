using NewLife;
using NewLife.Caching;
using NewLife.Caching.Services;
using NewLife.JT808.Protocols;
using NewLife.Log;
using Stardust.Extensions;
using XCode;
using XCode.DataAccessLayer;

XTrace.UseConsole();

ThreadPool.SetMinThreads(1024, 1000);

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// ===== 星尘服务治理 =====
var star = services.AddStardust(null);

// ===== 分布式缓存（Redis） =====
services.AddSingleton<ICacheProvider, RedisCacheProvider>();

// ===== 数据库连接初始化 =====
var set = NewLife.Setting.Current;
if (set.IsNew)
{
    set.DataPath = "../Data";
    set.Save();
}

// 核心库 GPS 连接
if (!DAL.ConnStrs.ContainsKey("GPS"))
    DAL.AddConnStr("GPS", "Server=127.0.0.1;Port=3306;Database=JT808;User=root;Password=root;", null, "MySql");

// 安全域和平台域默认 MapTo 到 GPS 库（可独立配置）
if (!DAL.ConnStrs.ContainsKey("Safety")) DAL.AddConnStr("Safety", "MapTo=GPS", null, "MySql");
if (!DAL.ConnStrs.ContainsKey("Platform")) DAL.AddConnStr("Platform", "MapTo=GPS", null, "MySql");

EntityFactory.InitConnection("GPS");
EntityFactory.InitConnection("Safety");
EntityFactory.InitConnection("Platform");

// ===== 服务注册 =====
// 指令处理器
services.AddSingleton<JT808.Server.Handlers.NormalController>();
services.AddSingleton<JT808.Server.Handlers.InfoController>();
services.AddSingleton<JT808.Server.Handlers.MediaController>();
services.AddSingleton<JT808.Server.Handlers.PositionController>();
services.AddSingleton<JT808.Server.Handlers.FileHandler>();

// 业务服务
services.AddSingleton<JT808.Server.Services.DeviceService>();
services.AddSingleton<JT808.Server.Services.QueueService>();
services.AddSingleton<JT808.Server.Services.AlarmEventService>();
services.AddSingleton<JT808.Server.Services.MediaService>();

// 网关服务与集群共享状态
var node = builder.Configuration["JT808:Node"];
if (node.IsNullOrEmpty()) node = Environment.MachineName;
var port = builder.Configuration["JT808:Port"].ToInt();
if (port <= 0) port = 808;

services.AddSingleton(sp =>
{
    var server = new JT808Server
    {
        Port = port,
        Log = XTrace.Log,
        SessionLog = XTrace.Log,
        Tracer = DefaultTracer.Instance,
    };

    server.AddHandler(sp.GetRequiredService<JT808.Server.Handlers.NormalController>());
    server.AddHandler(sp.GetRequiredService<JT808.Server.Handlers.PositionController>());
    server.AddHandler(sp.GetRequiredService<JT808.Server.Handlers.InfoController>());
    server.AddHandler(sp.GetRequiredService<JT808.Server.Handlers.MediaController>());
    server.AddHandler(sp.GetRequiredService<JT808.Server.Handlers.FileHandler>());

    return server;
});

services.AddSingleton(sp => new ClusterState(sp.GetRequiredService<ICacheProvider>().Cache) { Node = node });

// 后台服务（注意启动顺序）
services.AddHostedService<JT808.Server.HostedServices.DeviceOnlineHostedService>();
services.AddHostedService<JT808.Server.HostedServices.DataRetentionService>();
services.AddHostedService<JT808.Server.HostedServices.PreheatHostedService>();
services.AddHostedService<JT808.Server.HostedServices.GpsHostedService>();
services.AddHostedService<JT808.Server.HostedServices.ClusterStateHostedService>();
services.AddHostedService<JT808.Server.HostedServices.ClearHostedService>();

// ===== 健康检查 =====
services.AddHealthChecks();

var app = builder.Build();

// ===== 中间件 =====
app.UseStardust();
app.UseHealthChecks("/health");

// ===== 注册到星尘 =====
var env = app.Environment;
app.RegisterService("JT808Server", null, env.EnvironmentName);

// ===== 优雅退出 =====
NewLife.Model.Host.RegisterExit((s, e) => app.StopAsync().Wait());

XTrace.WriteLine("JT808 网关服务启动完成，端口：808");
XTrace.WriteLine("健康检查：http://localhost:5000/health");

app.Run();
