using Microsoft.AspNetCore.Mvc;
using GPSSORACOM.Models;
using GPSSORACOM.Services;

namespace GPSSORACOM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GpsController : ControllerBase
    {
        private readonly JsonStorageService _storage;

        public GpsController(JsonStorageService storage)
        {
            _storage = storage;
        }

        [HttpPost]
        public IActionResult ReceiveGps([FromBody] SimInfo model)
        {
            try
            {
                // Headers enviados por Soracom Beam
                model.SimId = Request.Headers["X-Soracom-Sim-Id"].ToString();
                model.IMSI = Request.Headers["X-Soracom-IMSI"].ToString();
                model.IMEI = Request.Headers["X-Soracom-IMEI"].ToString();
                model.MSISDN = Request.Headers["X-Soracom-MSISDN"].ToString();

                if (string.IsNullOrEmpty(model.SimId))
                    return BadRequest(new { message = "No se recibió SIM ID desde Soracom" });

                model.LastUpdate = DateTime.UtcNow;

                _storage.Save(model);

                return Ok(new { message = "Datos GPS guardados correctamente", sim = model.SimId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // Leer datos del GPS desde gps.json
        [HttpGet("{simId}")]
        public IActionResult GetGps(string simId)
        {
            var data = _storage.Get(simId);

            if (data == null)
                return NotFound(new { message = "No se encontraron datos para esta SIM" });

            return Ok(data);
        }
    }
}
