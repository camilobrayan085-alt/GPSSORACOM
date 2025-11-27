using System.Text.Json;

namespace GPSSORACOM.Services
{
    public class JsonStorageService
    {
        private readonly string _filePath;

        public JsonStorageService(IConfiguration config)
        {
            var storageFolder = "/tmp";
            _filePath = Path.Combine(storageFolder, "gps.json");

            if (!Directory.Exists(storageFolder))
                Directory.CreateDirectory(storageFolder);

            if (!File.Exists(_filePath))
                File.WriteAllText(_filePath, "[]");
        }

        public List<dynamic> Load()
        {
            try
            {
                if (!File.Exists(_filePath))
                    return new List<dynamic>();

                var content = File.ReadAllText(_filePath);
                if (string.IsNullOrWhiteSpace(content))
                    return new List<dynamic>();

                return JsonSerializer.Deserialize<List<dynamic>>(content) ?? new List<dynamic>();
            }
            catch
            {
                return new List<dynamic>();
            }
        }

        public void SaveOrUpdateSim(string simId, double lat, double lng)
        {
            var list = Load();

            var existing = list.FirstOrDefault(x => x.simId == simId);
            if (existing != null)
                list.Remove(existing);

            list.Add(new
            {
                simId = simId,
                Latitud = lat,
                Longitud = lng,
                UltimaActualizacion = DateTime.UtcNow
            });

            File.WriteAllText(_filePath, JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true }));
        }

        public dynamic GetSim(string simId)
        {
            return Load().FirstOrDefault(x => x.simId == simId);
        }
    }
}
