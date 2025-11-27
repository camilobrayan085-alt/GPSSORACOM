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
            // Tomar los valores directamente de los headers de Soracom
            string simId = Request.Headers["x-soracom-sim-id"];
            string imsi = Request.Headers["x-soracom-imsi"];
            string imei = Request.Headers["x-soracom-imei"];
            string msisdn = Request.Headers["x-soracom-msisdn"];

            if (string.IsNullOrEmpty(simId))
                return BadRequest("No se recibió SIM ID");

            // Si body viene vacío, crear nuevo objeto
            if (data == null) data = new SimInfo();

            data.SimId = simId;
            data.IMSI = imsi;
            data.IMEI = imei;
            data.MSISDN = msisdn;
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
                return NotFound(new { message = "SIM no encontrada" });

            return Ok(sim);
        }

        // GET: Obtiene todas las SIMs almacenadas
        [HttpGet]
        public IActionResult GetAll()
        {
            var allSims = _storage.Read();
            return Ok(allSims);
        }
    }
}
