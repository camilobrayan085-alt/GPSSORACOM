using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using GPSSORACOM.Models;

namespace MiApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SimController : ControllerBase
    {
        private readonly string filePath = "App_Data/sims.json";

        // ============
        // GET: api/sim/{simId}
        // ============
        [HttpGet("{simId}")]
        public IActionResult GetSimLocation(string simId)
        {
            var sims = LeerJson();

            var sim = sims.FirstOrDefault(s => s.SimId == simId);

            if (sim == null)
                return NotFound(new { message = "SIM no encontrada" });

            return Ok(sim);
        }

        // ============
        // PUT: api/sim/update
        // ============
        [HttpPut("update")]
        public IActionResult UpdateSim([FromBody] SimInfo update)
        {
            var sims = LeerJson();

            var sim = sims.FirstOrDefault(s => s.SimId == update.SimId);

            if (sim == null)
            {
                sims.Add(update);
            }
            else
            {
                sim.Latitud = update.Latitud;
                sim.Longitud = update.Longitud;
                sim.UltimaActualizacion = DateTime.Now;
            }

            System.IO.File.WriteAllText(filePath, JsonConvert.SerializeObject(sims, Formatting.Indented));

            return Ok(new { message = "SIM actualizada correctamente" });
        }

        // ============
        // Función para leer JSON
        // ============
        private List<SimInfo> LeerJson()
        {
            if (!System.IO.File.Exists(filePath))
                return new List<SimInfo>();

            var json = System.IO.File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<SimInfo>>(json) ?? new List<SimInfo>();
        }
    }
}
