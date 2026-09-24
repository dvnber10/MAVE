using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MAVE.DTO;
using MAVE.Services;

namespace MAVE.Controllers
{
    /// <summary>
    /// Sesiones de actividad cronometradas (mindfulness, yoga, respiración, etc.).
    /// Se guardan en AUDITORY sin necesidad de migraciones.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ActivitiesController : Controller
    {
        private static readonly HashSet<string> _allowed = new HashSet<string>(
            new[] { "MEDITATION", "MINDFULNESS", "YOGA", "BREATHING", "BODYSCAN", "WALK" });

        private readonly MeditationService _serv;

        public ActivitiesController(MeditationService serv)
        {
            _serv = serv;
        }

        /// <summary>
        /// Registra segundos de una actividad del catálogo.
        /// POST /api/activities/log
        /// </summary>
        [HttpPost("log")]
        [Authorize]
        public async Task<IActionResult> Log(ActivityLogDTO dto)
        {
            if (dto == null || dto.UserId <= 0 || dto.Seconds <= 0)
                return BadRequest("Datos inválidos");
            var action = (dto.Action ?? string.Empty).Trim().ToUpperInvariant();
            if (!_allowed.Contains(action))
                return BadRequest("Actividad no válida");
            try
            {
                await _serv.LogActivityAsync(dto.UserId, action, dto.Seconds);
                return Ok("Actividad registrada");
            }
            catch (Exception e)
            {
                return BadRequest("Algo salió mal: " + e.Message);
            }
        }
    }
}
