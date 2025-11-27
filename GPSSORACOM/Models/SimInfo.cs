namespace GPSSORACOM.Models
{
    public class SimInfo
    {
        public string? SimId { get; set; }
        public string? IMSI { get; set; }
        public string? IMEI { get; set; }
        public string? MSISDN { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public DateTime LastUpdate { get; set; }
    }
}
