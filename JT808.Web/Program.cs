using NewLife.Cube;
using NewLife.JT808.Protocols;
using NewLife.Log;

XTrace.UseConsole();

var builder = WebApplication.CreateBuilder(args);

// 注册服务
builder.Services.AddCube();
builder.Services.AddSingleton<JT808Server>();
builder.Services.AddSingleton<JT808Auth>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseCube(app.Environment);
app.UseCubeHome();
app.MapControllers();

app.Run();
