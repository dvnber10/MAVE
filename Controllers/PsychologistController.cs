using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MAVE.DTO;
using MAVE.Services;

namespace MAVE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PsychologistController : Controller
    {
        private readonly PsychologistService _serv;

        public PsychologistController(PsychologistService serv)
        {
            _serv = serv;
        }

        private string CallerEmail() =>
            User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? string.Empty;

        public class DescriptionDTO
        {
            public string Description { get; set; } = string.Empty;
        }

        public class VerifyDTO
        {
            public bool Verified { get; set; }
        }

        /// <summary>Perfil del psicólogo (descripción, documento, verificado). Solo dueño o admin.</summary>
        [HttpGet("profile/{userId}")]
        [Authorize]
        public async Task<IActionResult> Profile(int userId)
        {
            // La visibilidad del documento se controla en frontend (solo dueño/admin lo consultan).
            var dto = await _serv.GetProfile(userId);
            if (dto == null) return NotFound("Sin perfil");
            return Ok(dto);
        }

        /// <summary>Guarda la descripción pública del psicólogo.</summary>
        [HttpPut("profile/{id}")]
        [Authorize]
        public async Task<IActionResult> SaveDescription([FromBody] DescriptionDTO dto, int? id)
        {
            if (await _serv.SaveDescription(CallerEmail(), id ?? 0, dto.Description))
                return Ok("descripción guardada");
            return BadRequest("No autorizado o datos inválidos");
        }

        /// <summary>Carga el documento que lo ratifica como psicólogo (PDF/JPG/PNG, máx 10MB).</summary>
        [HttpPost("credential/{id}")]
        [Authorize]
        public async Task<IActionResult> UploadCredential(int? id, IFormFile file)
        {
            var (ok, message) = await _serv.UploadCredential(CallerEmail(), id ?? 0, file);
            if (ok) return Ok(message);
            return BadRequest(message);
        }

        /// <summary>Verifica o retira la verificación (solo admin).</summary>
        [HttpPut("verify/{userId}")]
        [Authorize]
        public async Task<IActionResult> Verify(int userId, [FromBody] VerifyDTO dto)
        {
            if (await _serv.Verify(CallerEmail(), userId, dto.Verified))
                return Ok(dto.Verified ? "psicólogo verificado" : "verificación retirada");
            return StatusCode(StatusCodes.Status403Forbidden, "No autorizado");
        }
    }
}
