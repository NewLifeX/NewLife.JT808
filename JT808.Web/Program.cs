using System.Text.Encodings.Web;
using System.Text.Unicode;
using NewLife;
using NewLife.Caching;
using NewLife.Caching.Services;
using NewLife.Cube;
using NewLife.JT808.Protocols;
using NewLife.Log;
using NewLife.Serialization;
using XCode;
using XCode.DataAccessLayer;

XTrace.UseConsole();

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

// Cube 魔方数据库
if (!DAL.ConnStrs.ContainsKey("Cube")) DAL.AddConnStr("Cube", "MapTo=Membership", null, "MySql");

var dal = DAL.Create("GPS");
if (!DAL.ConnStrs.ContainsKey("Membership")) DAL.AddConnStr("Membership", dal.ConnStr, null, dal.DbType + "");

EntityFactory.InitConnection("GPS");
EntityFactory.InitConnection("Safety");
EntityFactory.InitConnection("Platform");

// ===== 服务注册 =====
services.AddSingleton<JT808Server>();
services.AddSingleton<JT808Auth>();
services.AddSingleton<JT808.Web.Services.CommandClient>();

services.AddControllersWithViews();
services.AddCube();

// ===== JSON 配置 =====
services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
{
#if NET7_0_OR_GREATER
    options.JsonSerializerOptions.TypeInfoResolver = DataMemberResolver.Default;
#endif
    options.JsonSerializerOptions.Converters.Add(new TypeConverter());
    options.JsonSerializerOptions.Converters.Add(new LocalTimeConverter());
    options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
});

var app = builder.Build();

// ===== 中间件 =====
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseCube(app.Environment);
app.UseCubeHome();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=CubeHome}/{action=Index}/{id?}");

// ===== 星尘注册 =====
var env = app.Environment;
app.RegisterService("JT808Web", null, env.EnvironmentName);

// ===== 优雅退出 =====
NewLife.Model.Host.RegisterExit((s, e) => app.StopAsync().Wait());

app.Run();
