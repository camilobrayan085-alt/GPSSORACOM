using Microsoft.AspNetCore.Mvc;
using GPSSORACOM.Models;
using GPSSORACOM.Services;

namespace GPSSORACOM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimController : ControllerBase
    {
        private readonly JsonStorageService _storage;

        public SimController(JsonStorageService storage)
        {
            _storage = storage;
        }

        /// <summary>
        /// Obtiene la información de una SIM por su ID.
        /// </summary>
        [HttpGet("{simId}")]
        public IActionResult GetSim(string simId)
        {
            var sim = _storage.Get(simId);

            if (sim == null)
                return NotFound(new { message = $"No se encontró información para la SIM {simId}" });

            return Ok(sim);
        }

        /// <summary>
        /// Lista TODAS las SIM almacenadas en gps.json
        /// </summary>
        [HttpGet]
        public IActionResult GetAll()
        {
            var list = _storage.GetAll();

            return Ok(list);
        }

        /// <summary>
        /// Registrar una SIM manualmente (opcional)
        /// </summary>
        [HttpPost]
        public IActionResult RegisterSim([FromBody] SimInfo model)
        {
            if (string.IsNullOrEmpty(model.SimId))
                return BadRequest(new { message = "SimId es obligatorio" });

            model.LastUpdate = DateTime.UtcNow;

            _storage.Save(model);

            return Ok(new { message = "SIM registrada correctamente", sim = model.SimId });
        }

        /// <summary>
        /// Elimina una SIM del archivo gps.json
        /// </summary>
        [HttpDelete("{simId}")]
        public IActionResult DeleteSim(string simId)
        {
            bool removed = _storage.Delete(simId);

            if (!removed)
                return NotFound(new { message = $"La SIM {simId} no existe en el archivo" });

            return Ok(new { message = $"SIM {simId} eliminada correctamente" });
        }
    }
}
