using GPSSORACOM.Models;
using Newtonsoft.Json;

namespace GPSSORACOM.Services
{
    public class JsonStorageService
    {
        private readonly string _filePath;
        private readonly object _lock = new object();

        public JsonStorageService(IConfiguration config)
        {
            // Tomar ruta desde appsettings o usar /tmp/gps.json en Render
            var pathFromConfig = config["JsonStoragePath"];
            _filePath = string.IsNullOrWhiteSpace(pathFromConfig) ? "/tmp/gps.json" : Path.GetFullPath(pathFromConfig);

            var folder = Path.GetDirectoryName(_filePath)!;

            // Crear carpeta si no es /tmp
            if (!folder.Equals("/tmp") && !Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
                Console.WriteLine($"[JsonStorageService] Carpeta creada: {folder}");
            }

            // Crear archivo si no existe
            if (!File.Exists(_filePath))
            {
                try
                {
                    File.WriteAllText(_filePath, "[]");
                    Console.WriteLine($"[JsonStorageService] Archivo JSON creado: {_filePath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[JsonStorageService] Error creando archivo JSON: {ex.Message}");
                }
            }

            Console.WriteLine($"[JsonStorageService] Guardando GPS en: {_filePath}");
        }

        // Guardar un GPS
        public void SaveGps(GpsModel gps)
        {
            lock (_lock)
            {
                try
                {
                    var list = GetAllGps() ?? new List<GpsModel>();
                    list.Add(gps);
                    File.WriteAllText(_filePath, JsonConvert.SerializeObject(list, Formatting.Indented));
                    Console.WriteLine($"[JsonStorageService] GPS guardado: {gps.Imei} → {gps.Lat},{gps.Lng}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[JsonStorageService] Error guardando GPS: {ex.Message}");
                }
            }
        }

        // Obtener todos los GPS
        public List<GpsModel> GetAllGps()
        {
            lock (_lock)
            {
                try
                {
                    var json = File.ReadAllText(_filePath);
                    return JsonConvert.DeserializeObject<List<GpsModel>>(json) ?? new List<GpsModel>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[JsonStorageService] Error leyendo {_filePath}: {ex.Message}");
                    return new List<GpsModel>();
                }
            }
        }

        // Obtener GPS por IMEI específico
        public List<GpsModel> GetGpsByImei(string imei)
        {
            var all = GetAllGps();
            return all.Where(g => g.Imei == imei).ToList();
        }
    }
}
