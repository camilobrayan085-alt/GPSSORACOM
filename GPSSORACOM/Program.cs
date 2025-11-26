using GPSSORACOM.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<JsonStorageService>();

var app = builder.Build();

app.UseDefaultFiles(); // index.html por defecto
app.UseStaticFiles();

app.MapControllers();

// Puerto dinámico para Render
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Urls.Add($"http://0.0.0.0:{port}");

app.Run();
