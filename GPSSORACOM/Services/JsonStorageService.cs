using System.Text.Json;
using GPSSORACOM.Models;   // <-- Importamos tu SimInfo real

namespace GPSSORACOM.Services
{
    public class JsonStorageService
    {
        private readonly string _jsonPath;

        public JsonStorageService(IConfiguration config)
        {
            // Render SOLO permite escribir en /tmp
            _jsonPath = Path.Combine("/tmp", "gps.json");

            if (!File.Exists(_jsonPath))
            {
                File.WriteAllText(_jsonPath, "[]");
            }
        }

        // ===========================================================
        // ✔ Obtener un registro por SIM
        // ===========================================================
        public SimInfo? Get(string simId)
        {
            var all = GetAll();
            return all.FirstOrDefault(x => x.SimId == simId);
        }

        // ===========================================================
        // ✔ Obtener todos los registros
        // ===========================================================
        public List<SimInfo> GetAll()
        {
            if (!File.Exists(_jsonPath)) return new List<SimInfo>();

            var json = File.ReadAllText(_jsonPath);

            return JsonSerializer.Deserialize<List<SimInfo>>(json) ?? new List<SimInfo>();
        }

        // ===========================================================
        // ✔ Guardar o actualizar registro
        // ===========================================================
        public void SaveOrUpdateSim(SimInfo sim)
        {
            var all = GetAll();

            var existing = all.FirstOrDefault(x => x.SimId == sim.SimId);
            if (existing != null)
            {
                existing.Latitude = sim.Latitude;
                existing.Longitude = sim.Longitude;
                existing.LastUpdate = sim.LastUpdate;
            }
            else
            {
                all.Add(sim);
            }

            SaveAll(all);
        }

        // ===========================================================
        // ✔ Eliminar registro por SIM
        // ===========================================================
        public void Delete(string simId)
        {
            var all = GetAll();
            all = all.Where(x => x.SimId != simId).ToList();
            SaveAll(all);
        }

        // ===========================================================
        // ✔ Guardar la lista completa
        // ===========================================================
        private void SaveAll(List<SimInfo> all)
        {
            var json = JsonSerializer.Serialize(all, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(_jsonPath, json);
        }
    }
}
