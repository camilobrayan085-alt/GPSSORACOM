using System.Text.Json;
using GPSSORACOM.Models;

namespace GPSSORACOM.Services
{
    public class JsonStorageService
    {
        private readonly string _jsonPath;

        public JsonStorageService(IConfiguration config)
        {
            // Render solo permite escribir en /tmp
            _jsonPath = Path.Combine("/tmp", "gps.json");

            if (!File.Exists(_jsonPath))
            {
                File.WriteAllText(_jsonPath, "[]");
            }
        }

        // Obtener un registro por SIM
        public SimInfo? Get(string simId)
        {
            return GetAll().FirstOrDefault(x => x.SimId == simId);
        }

        // Obtener todos los registros
        public List<SimInfo> GetAll()
        {
            if (!File.Exists(_jsonPath)) return new List<SimInfo>();

            string json = File.ReadAllText(_jsonPath);

            return JsonSerializer.Deserialize<List<SimInfo>>(json) ?? new List<SimInfo>();
        }

        // Guardar o actualizar
        public void SaveOrUpdateSim(SimInfo sim)
        {
            var list = GetAll();

            var existing = list.FirstOrDefault(x => x.SimId == sim.SimId);

            if (existing != null)
            {
                existing.IMSI = sim.IMSI;
                existing.IMEI = sim.IMEI;
                existing.MSISDN = sim.MSISDN;

                existing.Latitude = sim.Latitude;
                existing.Longitude = sim.Longitude;
                existing.LastUpdate = sim.LastUpdate;
            }
            else
            {
                list.Add(sim);
            }

            SaveAll(list);
        }

        // Eliminar por SIM
        public void Delete(string simId)
        {
            var list = GetAll().Where(x => x.SimId != simId).ToList();
            SaveAll(list);
        }

        // Guardar la lista completa
        private void SaveAll(List<SimInfo> all)
        {
            var json = JsonSerializer.Serialize(all, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_jsonPath, json);
        }
    }
}
