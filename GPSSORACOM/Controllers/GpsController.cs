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

        [HttpPost("save")]
        public IActionResult SaveGps([FromBody] GpsModel gps)
        {
            gps.Timestamp = DateTime.Now;
            _storage.SaveGps(gps);
            return Ok(new { message = "GPS guardado correctamente." });
        }
    }
}
