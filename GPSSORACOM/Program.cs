using GPSSORACOM.Services;

var builder = WebApplication.CreateBuilder(args);

// Render usa el puerto 10000
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(10000);
});

builder.Services.AddControllers();
builder.Services.AddSingleton<JsonStorageService>();

var app = builder.Build();

app.MapControllers();

app.Run();
