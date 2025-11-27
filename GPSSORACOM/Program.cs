using GPSSORACOM.Services;

var builder = WebApplication.CreateBuilder(args);

// Render asigna puerto dinámico
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

builder.Services.AddControllers();
builder.Services.AddSingleton<JsonStorageService>();

var app = builder.Build();

// Servir wwwroot
app.UseDefaultFiles();   // Busca index.html automáticamente
app.UseStaticFiles();    // Habilita archivos estáticos (css, js, html)

// Ruta opcional para probar la API
app.MapGet("/status", () => "API GPSSORACOM funcionando correctamente");

// Mapear controladores
app.MapControllers();

// Ejecutar la app
app.Run();
