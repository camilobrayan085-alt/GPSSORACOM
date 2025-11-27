using GPSSORACOM.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<JsonStorageService>();

var app = builder.Build();

app.MapControllers();

app.Run();
