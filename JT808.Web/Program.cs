using NewLife.Cube;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCube();

var app = builder.Build();

app.UseCube(app.Environment);

app.Run();
