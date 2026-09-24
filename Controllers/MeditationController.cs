using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MAVE.DTO;
using MAVE.Services;

namespace MAVE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeditationController : Controller
    {
        private readonly MeditationService _serv;

        public MeditationController(MeditationService serv)
        {
            _serv = serv;
        }

        /// <summary>
        /// Devuelve una pista aleatoria de Freesound (CC0, libre de derechos)
        /// con URL de streaming propia para el reproductor HTML5.
        /// GET /api/meditation/random-sound
        /// </summary>
        [HttpGet("random-sound")]
        public async Task<IActionResult> RandomSound()
        {
            if (!_serv.IsConfigured())
                return StatusCode(502, "Falta la API key de Freesound (Freesound:ApiKey en appsettings).");
            try
            {
                var track = await _serv.GetRandomSoundAsync();
                if (track == null) return BadRequest("Freesound no devolvió resultados, intenta de nuevo");
                var dto = new MeditationSoundDTO
                {
                    Id = track.Id,
                    Title = track.Name,
                    Author = track.Username,
                    DurationSeconds = (int)Math.Round(track.Duration),
                    StreamUrl = $"{Request.Scheme}://{Request.Host}/api/meditation/stream/{track.Id}"
                };
                return Ok(dto);
            }
            catch (Exception e)
            {
                return StatusCode(502, "Freesound falló: " + e.Message);
            }
        }

        /// <summary>
        /// Lista N pistas distintas para que el usuario escoja (todas CC0).
        /// GET /api/meditation/sounds?count=10
        /// </summary>
        [HttpGet("sounds")]
        public async Task<IActionResult> Sounds([FromQuery] int count = 10)
        {
            if (!_serv.IsConfigured())
                return StatusCode(502, "Falta la API key de Freesound (Freesound:ApiKey en appsettings).");
            try
            {
                var tracks = await _serv.GetSoundsAsync(count);
                var list = tracks.Select(t => new MeditationSoundDTO
                {
                    Id = t.Id,
                    Title = t.Name,
                    Author = t.Username,
                    DurationSeconds = (int)Math.Round(t.Duration),
                    StreamUrl = $"{Request.Scheme}://{Request.Host}/api/meditation/stream/{t.Id}"
                }).ToList();
                return Ok(list);
            }
            catch (Exception e)
            {
                return StatusCode(502, "Freesound falló: " + e.Message);
            }
        }

        /// <summary>
        /// Registra segundos de escucha del usuario (para reportes de actividad).
        /// POST /api/meditation/log-time
        /// </summary>
        [HttpPost("log-time")]
        [Authorize]
        public async Task<IActionResult> LogTime(MeditationTimeDTO dto)
        {
            if (dto == null || dto.UserId <= 0 || dto.Seconds <= 0)
                return BadRequest("Datos inválidos");
            try
            {
                await _serv.LogTimeAsync(dto.UserId, dto.SoundId, dto.Seconds);
                return Ok("Tiempo registrado");
            }
            catch (Exception e)
            {
                return BadRequest("Algo salió mal: " + e.Message);
            }
        }

        /// <summary>
        /// Transmite el MP3 (proxea el preview de Freesound para no exponer la API key).
        /// GET /api/meditation/stream/{id}
        /// </summary>
        [HttpGet("stream/{id}")]
        public async Task<IActionResult> Stream(int id)
        {
            if (!_serv.IsConfigured())
                return StatusCode(502, "Falta la API key de Freesound (Freesound:ApiKey en appsettings).");
            try
            {
                string? range = Request.Headers.TryGetValue("Range", out var r) ? r.ToString() : null;
                await _serv.StreamPreviewAsync(id, Response, range, HttpContext.RequestAborted);
                return new EmptyResult();
            }
            catch (OperationCanceledException)
            {
                return new EmptyResult();
            }
            catch (Exception e)
            {
                if (!Response.HasStarted)
                    return StatusCode(502, "No se pudo transmitir el audio: " + e.Message);
                return new EmptyResult();
            }
        }
    }
}
