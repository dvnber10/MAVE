using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MAVE.DTO;
using MAVE.Services;

namespace MAVE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : Controller
    {
        private readonly ChatService _serv;

        public ChatController(ChatService serv)
        {
            _serv = serv;
        }

        private string CallerEmail() =>
            User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? string.Empty;

        /// <summary>Conversaciones del usuario (último mensaje + no leídos).</summary>
        [HttpGet("conversations/{userId}")]
        [Authorize]
        public async Task<IActionResult> Conversations(int userId)
        {
            var list = await _serv.Conversations(CallerEmail(), userId);
            if (list == null) return StatusCode(StatusCodes.Status403Forbidden, "No autorizado");
            return Ok(list);
        }

        /// <summary>Historial con otro usuario (marca como leídos).</summary>
        [HttpGet("messages/{userId}/{otherId}")]
        [Authorize]
        public async Task<IActionResult> History(int userId, int otherId)
        {
            var list = await _serv.History(CallerEmail(), userId, otherId);
            if (list == null) return StatusCode(StatusCodes.Status403Forbidden, "No autorizado");
            return Ok(list);
        }

        public class SendDTO
        {
            public int SenderId { get; set; }
            public int ReceiverId { get; set; }
            public string Text { get; set; } = string.Empty;
        }

        /// <summary>Envía un mensaje (máx. 500 caracteres).</summary>
        [HttpPost("send")]
        [Authorize]
        public async Task<IActionResult> Send([FromBody] SendDTO dto)
        {
            var m = await _serv.Send(CallerEmail(), dto.SenderId, dto.ReceiverId, dto.Text);
            if (m == null) return BadRequest("No se pudo enviar");
            return Ok(m);
        }
    }
}
