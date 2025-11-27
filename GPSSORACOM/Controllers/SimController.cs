using Microsoft.AspNetCore.Mvc;
using GPSSORACOM.Models;
using GPSSORACOM.Services;

namespace GPSSORACOM.Controllers
{
    [Route("api/sim")]
    [ApiController]
    public class SimController : ControllerBase
    {
        private readonly JsonStorageService _json;

        public SimController(JsonStorageService json)
        {
            _json = json;
        }

        // Crear o actualizar SIM manual
        [HttpPost("save")]
        public IActionResult Save([FromBody] SimInfo sim)
        {
            if (sim == null || string.IsNullOrEmpty(sim.SimId))
                return BadRequest("SimId requerido");

            _json.SaveOrUpdateSim(sim);

            return Ok(new { message = "SIM guardada", sim });
        }

        // Eliminar SIM
        [HttpDelete("delete/{simId}")]
        public IActionResult Delete(string simId)
        {
            _json.Delete(simId);
            return Ok(new { message = "SIM eliminada" });
        }

        // Obtener SIM
        [HttpGet("{simId}")]
        public IActionResult Get(string simId)
        {
            var sim = _json.Get(simId);
            if (sim == null) return NotFound("SIM no encontrada");

            return Ok(sim);
        }
    }
}
