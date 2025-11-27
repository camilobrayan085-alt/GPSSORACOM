using System;

namespace GPSSORACOM.Models
{
    public class SimInfo
    {
        // Identificadores de la SIM
        public string SimId { get; set; }
        public string IMSI { get; set; }
        public string IMEI { get; set; }
        public string MSISDN { get; set; }

        // Coordenadas de ubicación
        public double Latitud { get; set; }
        public double Longitud { get; set; }

        // Fecha y hora de la última actualización
        public string UltimaActualizacion { get; set; }
    }
}
