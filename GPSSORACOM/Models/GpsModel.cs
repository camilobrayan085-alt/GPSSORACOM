namespace GPSSORACOM.Models
{
    public class GpsModel
    {
        public string Imei { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public double? Accuracy { get; set; }
        public double? Speed { get; set; }
        public double? Heading { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
