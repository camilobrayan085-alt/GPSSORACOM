using Microsoft.AspNetCore.Mvc;
using GPSSORACOM.Models;
using GPSSORACOM.Services;

namespace GPSSORACOM.Controllers
{
    [Route("api/gps")]
    [ApiController]
    public class GpsController : ControllerBase
    {
        private readonly JsonStorageService _json;

        public GpsController(JsonStorageService json)
        {
            _json = json;
        }

        // POST api/gps/update
        [HttpPost("update")]
        public IActionResult Update([FromBody] SimInfo data)
        {
            if (data == null || string.IsNullOrEmpty(data.SimId))
                return BadRequest("SimId requerido");

            _json.SaveOrUpdateSim(data);

            return Ok(new { message = "GPS actualizado", sim = data });
        }

        // GET api/gps/all
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            return Ok(_json.GetAll());
        }

        // GET api/gps/{simId}
        [HttpGet("{simId}")]
        public IActionResult Get(string simId)
        {
            var sim = _json.Get(simId);
            if (sim == null) return NotFound("SIM no encontrada");

            return Ok(sim);
        }
    }
}
