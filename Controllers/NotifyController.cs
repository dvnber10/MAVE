using Microsoft.AspNetCore.Mvc;
using MAVE.Services;

namespace MAVE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotifyController : ControllerBase
    {
        readonly NotifyService _serv;
        public NotifyController(NotifyService serv)
        {
            _serv = serv;
        }

        [HttpPost]
        [Route("SendMesssages")]
        public async Task<IActionResult> SendMessages()
        {
            try
            {
                int res = await _serv.SendMessages();
                if(res == 1) return BadRequest("Algo ha salido mal en los servicios");
                else if(res == 2) return BadRequest("Algunos o todos los mensajes no han sido enviados");
                else return Ok("Mensajes enviados");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Sugerencia diaria rotativa + pendientes de hoy para la campana web.
        /// GET /api/notify/daily-suggestion/{id}
        /// </summary>
        [HttpGet("daily-suggestion/{id}")]
        public async Task<IActionResult> DailySuggestion(int? id)
        {
            try
            {
                var dto = await _serv.DailySuggestion(id);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest("Algo salió mal: " + ex.Message);
            }
        }
    }
}