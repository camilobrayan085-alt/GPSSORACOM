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
            // Si viene vacío o NULL, se usa /tmp/gps.json (Render)
            var pathFromConfig = config["JsonStoragePath"];

            if (string.IsNullOrWhiteSpace(pathFromConfig))
            {
                _filePath = "/tmp/gps.json";   // Render
            }
            else
            {
                // Normaliza ruta local (appsettings)
                _filePath = Path.GetFullPath(pathFromConfig);
            }

            var folder = Path.GetDirectoryName(_filePath)!;

            // Crear carpeta solo si no es /tmp
            if (!folder.Equals("/tmp") && !Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            // Crear archivo si no existe
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
            }

            Console.WriteLine($"[JsonStorageService] Guardando GPS en: {_filePath}");
        }

        // Leer todas las SIM
        public List<SimInfo> Read()
        {
            lock (_lock)
            {
                var json = File.ReadAllText(_filePath);
                return JsonConvert.DeserializeObject<List<SimInfo>>(json) ?? new List<SimInfo>();
            }
        }

        // Guardar o actualizar una SIM
        public void SaveOrUpdateSim(SimInfo sim)
        {
            lock (_lock)
            {
                var list = Read();
                var existing = list.FirstOrDefault(x => x.SimId == sim.SimId);

                if (existing != null)
                {
                    list.Remove(existing);
                }

                list.Add(sim);

                File.WriteAllText(_filePath, JsonConvert.SerializeObject(list, Formatting.Indented));
                Console.WriteLine($"[JsonStorageService] SIM {sim.SimId} guardada/actualizada correctamente.");
            }
        }

        // Obtener una SIM por ID
        public SimInfo? GetSim(string simId)
        {
            lock (_lock)
            {
                var list = Read();
                return list.FirstOrDefault(x => x.SimId == simId);
            }
        }
    }
}
