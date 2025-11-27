using GPSSORACOM.Models;
using System.Text.Json;

namespace GPSSORACOM.Services
{
    public class JsonStorageService
    {
        private readonly string gpsFile = "gps_data.json";
        private readonly string simFile = "sim_data.json";
        private readonly object fileLock = new object();

        // --- GPS ---
        public void SaveGps(GpsModel gps)
        {
            lock (fileLock)
            {
                List<GpsModel> list = File.Exists(gpsFile)
                    ? JsonSerializer.Deserialize<List<GpsModel>>(File.ReadAllText(gpsFile)) ?? new List<GpsModel>()
                    : new List<GpsModel>();

                list.Add(gps);
                File.WriteAllText(gpsFile, JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true }));
            }
        }

        public List<GpsModel> GetAllGps()
        {
            if (!File.Exists(gpsFile)) return new List<GpsModel>();
            return JsonSerializer.Deserialize<List<GpsModel>>(File.ReadAllText(gpsFile)) ?? new List<GpsModel>();
        }

        // --- SIM ---
        public void SaveSim(SimInfo sim)
        {
            lock (fileLock)
            {
                List<SimInfo> list = File.Exists(simFile)
                    ? JsonSerializer.Deserialize<List<SimInfo>>(File.ReadAllText(simFile)) ?? new List<SimInfo>()
                    : new List<SimInfo>();

                var existing = list.FirstOrDefault(s => s.Imei == sim.Imei);
                if (existing != null) list.Remove(existing);

                list.Add(sim);
                File.WriteAllText(simFile, JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true }));
            }
        }

        public List<SimInfo> GetAllSims()
        {
            if (!File.Exists(simFile)) return new List<SimInfo>();
            return JsonSerializer.Deserialize<List<SimInfo>>(File.ReadAllText(simFile)) ?? new List<SimInfo>();
        }
    }
}
