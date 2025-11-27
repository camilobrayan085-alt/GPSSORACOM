using System.Text.Json;
using GPSSORACOM.Models;

namespace GPSSORACOM.Services
{
    public class JsonStorageService
    {
        private readonly string simPath = "sim_data.json";
        private readonly string gpsPath = "gps_data.json";

        public void SaveSim(SimInfo sim)
        {
            List<SimInfo> sims = Load<SimInfo>(simPath);
            sims.RemoveAll(x => x.SimId == sim.SimId);
            sims.Add(sim);
            File.WriteAllText(simPath, JsonSerializer.Serialize(sims));
        }

        public void SaveGps(GpsModel gps)
        {
            List<GpsModel> data = Load<GpsModel>(gpsPath);
            data.Add(gps);
            File.WriteAllText(gpsPath, JsonSerializer.Serialize(data));
        }

        public List<T> Load<T>(string path)
        {
            if (!File.Exists(path)) return new List<T>();
            return JsonSerializer.Deserialize<List<T>>(File.ReadAllText(path)) ?? new List<T>();
        }
    }
}
