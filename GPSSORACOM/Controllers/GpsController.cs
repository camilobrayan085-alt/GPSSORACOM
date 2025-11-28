using Microsoft.AspNetCore.Mvc;
using GPSSORACOM.Models;
using GPSSORACOM.Services;
using System.Linq;

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

        // Guardar GPS (Beam enviará datos aquí)
        [HttpPost("save")]
        public IActionResult SaveGps([FromBody] GpsModel gps)
        {
            gps.Timestamp = DateTime.UtcNow;
            _storage.SaveGps(gps);
            return Ok(new { message = "GPS guardado correctamente." });
        }

        // Listar todos los GPS
        [HttpGet("all")]
        public IActionResult GetAllGps()
        {
            var list = _storage.GetAllGps();
            return Ok(list);
        }

        // Consultar por IMEI específico
        [HttpGet("{imei}")]
        public IActionResult GetGpsByImei(string imei)
        {
            var list = _storage.GetAllGps();
            var result = list.Where(g => g.Imei == imei).ToList();
            if (!result.Any())
                return NotFound(new { message = "IMEI no encontrado" });
            return Ok(result);
        }
    }
}
