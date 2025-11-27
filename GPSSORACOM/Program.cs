using GPSSORACOM.Services;

var builder = WebApplication.CreateBuilder(args);

// Habilitar CORS (importante en Render)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

// Registrar el servicio JSON
builder.Services.AddSingleton<JsonStorageService>();

var app = builder.Build();

// Activar CORS
app.UseCors("AllowAll");

app.MapControllers();

app.Run();
