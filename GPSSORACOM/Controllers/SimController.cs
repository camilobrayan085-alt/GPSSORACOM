using Microsoft.AspNetCore.Mvc;
using GPSSORACOM.Models;
using GPSSORACOM.Services;
using System.Linq;

namespace GPSSORACOM.Controllers
{
    [ApiController]
    [Route("api/sim")]
    public class SimController : ControllerBase
    {
        private readonly JsonStorageService _storage;

        public SimController(JsonStorageService storage)
        {
            _storage = storage;
        }

        // Actualiza un "SIM" → en realidad actualiza la posición GPS del IMEI
        [HttpPost("update")]
        public IActionResult UpdateSim([FromBody] GpsModel gps)
        {
            if (gps == null || string.IsNullOrEmpty(gps.Imei))
                return BadRequest(new { message = "Datos inválidos" });

            gps.Timestamp = DateTime.UtcNow;
            _storage.SaveGps(gps);

            return Ok(new { message = "SIM/GPS actualizado correctamente." });
        }

        // Obtener todos los "SIMs" → devuelve todas las posiciones GPS guardadas
        [HttpGet("all")]
        public IActionResult GetAllSims()
        {
            var list = _storage.GetAllGps();
            return Ok(list);
        }

        // Consultar por IMEI específico
        [HttpGet("{imei}")]
        public IActionResult GetSimByImei(string imei)
        {
            var list = _storage.GetAllGps();
            var result = list.Where(g => g.Imei == imei).ToList();

            if (!result.Any())
                return NotFound(new { message = "IMEI no encontrado" });

            return Ok(result);
        }
    }
}
