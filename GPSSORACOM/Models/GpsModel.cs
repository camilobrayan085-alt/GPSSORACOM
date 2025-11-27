namespace GPSSORACOM.Models
{
    public class GpsModel
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Timestamp { get; set; }
        public string? DeviceId { get; set; }
    }
}
