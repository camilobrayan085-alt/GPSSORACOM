using Microsoft.AspNetCore.Mvc;
using GPSSORACOM.Models;
using GPSSORACOM.Services;

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

        [HttpPost("update")]
        public IActionResult UpdateSim([FromBody] SimInfo sim)
        {
            sim.LastUpdate = DateTime.Now;
            _storage.SaveSim(sim);
            return Ok(new { message = "SIM actualizado correctamente." });
        }

        [HttpGet("all")]
        public IActionResult GetAllSims()
        {
            var list = _storage.GetAllSims();
            return Ok(list);
        }
    }
}
