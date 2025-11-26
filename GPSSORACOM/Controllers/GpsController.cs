using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;
using GPSSORACOM.Services;

namespace GPSSORACOM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GpsController : ControllerBase
    {
        private readonly JsonStorageService _storage;

        public GpsController(JsonStorageService storage)
        {
            _storage = storage;
        }

        [HttpPost]
        public IActionResult ReceiveGps([FromBody] object body)
        {
            string imsi = Request.Headers["x-soracom-imsi"];
            string imei = Request.Headers["x-soracom-imei"];
            string msisdn = Request.Headers["x-soracom-msisdn"];
            string simId = Request.Headers["x-soracom-sim-id"];

            var record = new
            {
                IMSI = imsi,
                IMEI = imei,
                MSISDN = msisdn,
                SIMID = simId,
                Timestamp = DateTime.UtcNow,
                Data = body
            };

            _storage.Add(record);

            return Ok(new { status = "ok", saved = true });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_storage.Read());
        }
    }
}
