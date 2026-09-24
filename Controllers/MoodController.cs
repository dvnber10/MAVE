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

        /// <summary>
        /// Chequeo PHQ-4 (4 ítems 0-3: 2 depresión + 2 ansiedad). 1 por día.
        /// POST /api/mood/phq4/{id}
        /// </summary>
        [HttpPost]
        [Authorize]
        [Route("phq4/{id}")]
        public async Task<IActionResult> SetPhq4(Phq4DTO dto, int? id)
        {
            int res = await _serv.SetPhq4(dto, id);
            if (res == 1) return Ok("Chequeo guardado");
            else if (res == 2) return BadRequest("Ya registraste tu chequeo hoy");
            else return BadRequest("Datos inválidos");
        }

        /// <summary>
        /// Historial PHQ-4 (fecha, D, A, total) para gráficas y reporte clínico.
        /// GET /api/mood/phq4-history/{id}
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("phq4-history/{id}")]
        public async Task<IActionResult> Phq4History(int? id)
        {
            try
            {
                return Ok(await _serv.Phq4History(id, 30));
            }
            catch (Exception e)
            {
                return BadRequest("Algo salió mal: " + e.Message);
            }
        }

        /// <summary>
        /// Análisis psicológico del seguimiento: promedio, distribución,
        /// volatilidad, racha, tendencia e índice compuesto 0-100.
        /// GET /api/mood/analysis/{id}
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("analysis/{id}")]
        public async Task<IActionResult> Analysis(int? id)
        {
            try
            {
                return Ok(await _serv.Analysis(id));
            }
            catch (Exception e)
            {
                return BadRequest("Algo salió mal: " + e.Message);
            }
        }

    }
}