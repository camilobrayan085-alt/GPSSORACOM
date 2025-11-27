using Microsoft.AspNetCore.Mvc;
using GPSSORACOM.Models;
using GPSSORACOM.Services;

namespace GPSSORACOM.Controllers
{
    [ApiController]
    [Route("api/gps")]
    public class GpsController : ControllerBase
    {
        private readonly JsonStorageService _storage;

        public GpsController(JsonStorageService storage)
        {
            _storage = storage;
        }

        // POST: Recibe datos de Soracom Beam
        [HttpPost]
        public IActionResult ReceiveGps([FromBody] SimInfo data)
        {
            string simId = Request.Headers["x-soracom-sim-id"];
            string imsi = Request.Headers["x-soracom-imsi"];
            string imei = Request.Headers["x-soracom-imei"];
            string msisdn = Request.Headers["x-soracom-msisdn"];

            if (string.IsNullOrEmpty(simId))
                return BadRequest(new { status = "error", message = "No se recibió SIM ID" });

            if (data == null) data = new SimInfo();

            data.SimId = simId;
            data.IMSI = imsi;
            data.IMEI = imei;
            data.MSISDN = msisdn;
            data.Latitud ??= 0;
            data.Longitud ??= 0;
            data.UltimaActualizacion = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

            _storage.SaveOrUpdateSim(data);

            return Ok(new { status = "ok", saved = true });
        }

        // GET: Obtiene ubicación de una SIM específica
        [HttpGet("{simId}")]
        public IActionResult GetSim(string simId)
        {
            var sim = _storage.GetSim(simId);

            if (sim == null)
                return NotFound(new { status = "error", message = "SIM no encontrada" });

            // Retornar valores aunque Lat/Lon sean 0
            return Ok(new
            {
                sim.SimId,
                sim.IMSI,
                sim.IMEI,
                sim.MSISDN,
                latitud = sim.Latitud,
                longitud = sim.Longitud,
                ultimaActualizacion = sim.UltimaActualizacion
            });
        }

        // GET: Obtiene todas las SIMs almacenadas
        [HttpGet]
        public IActionResult GetAll()
        {
            var allSims = _storage.Read();
            return Ok(allSims.Select(sim => new
            {
                sim.SimId,
                sim.IMSI,
                sim.IMEI,
                sim.MSISDN,
                latitud = sim.Latitud,
                longitud = sim.Longitud,
                ultimaActualizacion = sim.UltimaActualizacion
            }));
        }
    }
}
