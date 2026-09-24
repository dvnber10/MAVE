using MAVE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace MAVE.Controllers
{
    /// <summary>
    /// Diagnóstico público para verificar el despliegue (lo consume /Estado).
    /// No expone secretos ni datos de usuarios.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : Controller
    {
        private readonly DbAa60a4MavetestContext _db;
        private readonly IConfiguration _config;

        public HealthController(DbAa60a4MavetestContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var sw = Stopwatch.StartNew();
            bool dbOk = false;
            try { dbOk = await _db.Database.CanConnectAsync(); }
            catch { dbOk = false; }
            sw.Stop();
            var key = _config["Freesound:ApiKey"] ?? string.Empty;
            bool fsOk = !string.IsNullOrWhiteSpace(key) && !key.StartsWith("PEGA");
            return Ok(new
            {
                status = dbOk ? "ok" : "degraded",
                time = DateTime.UtcNow,
                database = new { ok = dbOk, latencyMs = sw.ElapsedMilliseconds },
                freesound = new { configured = fsOk },
                version = "1.0"
            });
        }
    }
}
