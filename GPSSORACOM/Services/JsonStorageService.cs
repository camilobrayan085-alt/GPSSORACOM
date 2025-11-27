using System.Text.Json;
using GPSSORACOM.Models;

namespace GPSSORACOM.Services
{
    public class JsonStorageService
    {
        private readonly string _filePath;

        public JsonStorageService(IConfiguration config)
        {
<<<<<<< HEAD
            // Obtiene el archivo desde appsettings.json
            _filePath = config["Storage:GpsFilePath"] ?? "/tmp/gps.json";
        }

        /// <summary>
        /// Guarda o actualiza un registro SIM en gps.json
        /// </summary>
        public void Save(SimInfo info)
        {
            Dictionary<string, SimInfo> data;

            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                data = JsonSerializer.Deserialize<Dictionary<string, SimInfo>>(json)
                       ?? new Dictionary<string, SimInfo>();
=======
            // Ruta de almacenamiento en Render (escribible)
            _filePath = config["JsonStoragePath"] ?? "/tmp/gps.json";

            var folder = Path.GetDirectoryName(_filePath)!;
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            if (!File.Exists(_filePath))
                File.WriteAllText(_filePath, "[]");

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
>>>>>>> 8706f75 (Actualizado JsonStorageService con métodos SaveOrUpdateSim y GetSim)
            }
            else
            {
                data = new Dictionary<string, SimInfo>();
            }

            // Guardar/actualizar por SimId
            data[info.SimId] = info;

            var output = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(_filePath, output);
        }

<<<<<<< HEAD
        /// <summary>
        /// Obtiene una SIM espec�fica por su ID
        /// </summary>
        public SimInfo? Get(string simId)
        {
            if (!File.Exists(_filePath))
                return null;

            var json = File.ReadAllText(_filePath);
            var data = JsonSerializer.Deserialize<Dictionary<string, SimInfo>>(json);

            if (data != null && data.ContainsKey(simId))
                return data[simId];

            return null;
        }

        /// <summary>
        /// Obtiene TODAS las SIM almacenadas
        /// </summary>
        public Dictionary<string, SimInfo> GetAll()
        {
            if (!File.Exists(_filePath))
                return new Dictionary<string, SimInfo>();

            var json = File.ReadAllText(_filePath);

            var data = JsonSerializer.Deserialize<Dictionary<string, SimInfo>>(json);

            if (data == null)
                return new Dictionary<string, SimInfo>();

            return data;
        }

        /// <summary>
        /// Elimina una SIM del archivo
        /// </summary>
        public bool Delete(string simId)
        {
            if (!File.Exists(_filePath))
                return false;

            var json = File.ReadAllText(_filePath);
            var data = JsonSerializer.Deserialize<Dictionary<string, SimInfo>>(json);

            if (data == null || !data.ContainsKey(simId))
                return false;

            data.Remove(simId);

            File.WriteAllText(_filePath, JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true
            }));

            return true;
=======
        // Obtener una SIM por ID
        public SimInfo? GetSim(string simId)
        {
            lock (_lock)
            {
                var list = Read();
                return list.FirstOrDefault(x => x.SimId == simId);
            }
>>>>>>> 8706f75 (Actualizado JsonStorageService con métodos SaveOrUpdateSim y GetSim)
        }
    }
}
