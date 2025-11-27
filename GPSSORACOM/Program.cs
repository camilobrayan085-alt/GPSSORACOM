using GPSSORACOM.Services;

var builder = WebApplication.CreateBuilder(args);

// Render asigna puerto dinámico
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

builder.Services.AddControllers();
builder.Services.AddSingleton<JsonStorageService>();

var app = builder.Build();

app.MapControllers();

app.Run();
