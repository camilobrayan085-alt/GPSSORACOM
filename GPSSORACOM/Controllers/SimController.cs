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

        // GET: api/sim/{simId}
        [HttpGet("{simId}")]
        public IActionResult GetSimLocation(string simId)
        {
            var sim = _storage.GetSim(simId);
            if (sim == null)
                return NotFound(new { message = "SIM no encontrada" });

            return Ok(sim);
        }

        // PUT: api/sim/update
        [HttpPut("update")]
        public IActionResult UpdateSim([FromBody] SimInfo update)
        {
            if (update == null || string.IsNullOrEmpty(update.SimId))
                return BadRequest(new { message = "Datos inválidos" });

            update.UltimaActualizacion = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

            _storage.SaveOrUpdateSim(update);

            return Ok(new { message = "SIM actualizada correctamente" });
        }

        // GET: api/sim
        [HttpGet]
        public IActionResult GetAll()
        {
            var allSims = _storage.Read();
            return Ok(allSims);
        }
    }
}
