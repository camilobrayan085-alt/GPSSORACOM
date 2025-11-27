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

        // Leer todas las SIMs
        public List<SimInfo> Read()
        {
            lock (_lock)
            {
                try
                {
                    var json = File.ReadAllText(_filePath);
                    return JsonConvert.DeserializeObject<List<SimInfo>>(json) ?? new List<SimInfo>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[JsonStorageService] Error leyendo {_filePath}: {ex.Message}");
                    return new List<SimInfo>();
                }
            }
        }

        // Guardar o actualizar una SIM
        public void SaveOrUpdateSim(SimInfo sim)
        {
            lock (_lock)
            {
                try
                {
                    var list = Read();

                    // Eliminar si ya existe
                    var existing = list.FirstOrDefault(x => x.SimId == sim.SimId);
                    if (existing != null)
                        list.Remove(existing);

                    // Asegurar valores por defecto
                    if (sim.Latitud == null) sim.Latitud = 0;
                    if (sim.Longitud == null) sim.Longitud = 0;
                    if (string.IsNullOrEmpty(sim.UltimaActualizacion))
                        sim.UltimaActualizacion = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

                    list.Add(sim);

                    File.WriteAllText(_filePath, JsonConvert.SerializeObject(list, Formatting.Indented));
                    Console.WriteLine($"[JsonStorageService] SIM {sim.SimId} guardada/actualizada correctamente.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[JsonStorageService] Error guardando SIM {sim.SimId}: {ex.Message}");
                }
            }
        }

        // Obtener una SIM por ID
        public SimInfo? GetSim(string simId)
        {
            lock (_lock)
            {
                try
                {
                    var list = Read();
                    return list.FirstOrDefault(x => x.SimId == simId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[JsonStorageService] Error obteniendo SIM {simId}: {ex.Message}");
                    return null;
                }
            }
        }

        // Guardar múltiples SIMs (opcional)
        public void SaveOrUpdateSims(List<SimInfo> sims)
        {
            lock (_lock)
            {
                try
                {
                    var list = Read();

                    foreach (var sim in sims)
                    {
                        var existing = list.FirstOrDefault(x => x.SimId == sim.SimId);
                        if (existing != null) list.Remove(existing);

                        if (sim.Latitud == null) sim.Latitud = 0;
                        if (sim.Longitud == null) sim.Longitud = 0;
                        if (string.IsNullOrEmpty(sim.UltimaActualizacion))
                            sim.UltimaActualizacion = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

                        list.Add(sim);
                    }

                    File.WriteAllText(_filePath, JsonConvert.SerializeObject(list, Formatting.Indented));
                    Console.WriteLine($"[JsonStorageService] {sims.Count} SIMs guardadas/actualizadas correctamente.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[JsonStorageService] Error guardando múltiples SIMs: {ex.Message}");
                }
            }
        }
    }
}
