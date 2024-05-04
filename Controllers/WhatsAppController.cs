using Microsoft.AspNetCore.Mvc;
using MAVE.Services;

namespace MAVE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WhatsAppController : ControllerBase
    {
        readonly WhatsAppService _serv;
        public WhatsAppController(WhatsAppService serv)
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
    }
}