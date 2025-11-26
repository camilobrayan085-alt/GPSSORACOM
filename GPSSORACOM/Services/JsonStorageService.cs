using Newtonsoft.Json;

namespace GPSSORACOM.Services
{
    public class JsonStorageService
    {
        private readonly string _filePath;

        public JsonStorageService(IConfiguration config)
        {
            _filePath = config["JsonStoragePath"] ?? "data/gps.json";

            var folder = Path.GetDirectoryName(_filePath)!;

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            if (!File.Exists(_filePath))
                File.WriteAllText(_filePath, "[]");
        }

        public List<object> Read()
        {
            var json = File.ReadAllText(_filePath);

            return JsonConvert.DeserializeObject<List<object>>(json)
                   ?? new List<object>();
        }

        public void Add(object item)
        {
            var list = Read();
            list.Add(item);

            var json = JsonConvert.SerializeObject(list, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }
    }
}
