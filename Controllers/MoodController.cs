using Microsoft.AspNetCore.Mvc;
using MAVE.DTO;
using MAVE.Services;
using Microsoft.AspNetCore.Authorization;

namespace MAVE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoodController: Controller
    {
        private readonly MoodService _serv;

        public MoodController(MoodService serv)
        {
            _serv = serv;
        }

        [HttpPost]
        [Authorize]
        [Route("SetMood/{id}")]
        public async Task<IActionResult> SetMood(MoodDTO mood, int? id){
            if (mood.Score <= 0 || mood.Score > 5) return BadRequest("Puntaje inválido");
            int res = await _serv.SetMood(mood, id);
            if(res == 0)
            {
                return BadRequest("No se envio ningún dato");
            }
            else if (res == 2)
            {
                return BadRequest("Ya elegiste tu estado de ánimo hoy");
            }
            else
            {
                return Ok("Se guardaron los datos con éxito");
            }   
        }

        [HttpGet]
        [Authorize]
        [Route("GetMood/{id}")]
        public async Task<IActionResult> GetMood(int? id)
        {
            try
            {
                int res = await _serv.GetMood(id);
                if(res == 0)
                {
                    return Ok();
                }
                else if(res == 1)
                {
                    return BadRequest("Hubo un problema con la base de datos");
                }
                else 
                {
                    return BadRequest("Algo salió mal");
                }
            }
            catch(Exception e)
            {
                return BadRequest("Algo falló" + e.Message);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("GetMoodGraphic/{id}")]
        public async Task<IActionResult> GetMoodGraphic(int? id)
        {
            try
            {
                MoodGraphicDTO? mood = await _serv.GetMoodGraphic(id);
                if(mood == null) return BadRequest("No se encontró información de este usuario");
                else return Ok(mood);
            }
            catch(Exception e)
            {
                return BadRequest("Algo salió mal: "+e);            
            }
        }

    }
}