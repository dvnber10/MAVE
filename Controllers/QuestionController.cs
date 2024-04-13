using MAVE.DTO;
using MAVE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MAVE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly QuestionService _serv;
        public QuestionController(QuestionService serv){
            _serv = serv;
        }

        [HttpGet]
        [Authorize]
        [Route("GetHabitQuestions/{id}")]
        public async Task<IActionResult> GetHabitQuestions(int? id){
            try
            {
                var questions = await _serv.GetHabitQuestion(id);
                return Ok(questions); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
             }
        }

        [HttpPost]
        [Authorize]
        [Route("SetHabitScore/{id}")]
        public async Task<IActionResult> SetHabitScore(int? id, HabitDTO habit)
        {
            try
            {
                if(await _serv.SetHabitQuestion(id,habit) == 0)
                {
                    return Ok("Se guardaron los datos exitósamente");
                }
                else if(await _serv.SetHabitQuestion(id,habit) == 1)
                {
                    return BadRequest("Hubo un problema con la base de datos");
                }
                else
                {
                    return BadRequest("Algo salió mal");
                }
            }
            catch (Exception e)
            {
                return BadRequest("Ocurrió un error: "+ e.Message);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("GetInitialEvaluation/{id}")]
        public async Task<IActionResult> GetInitialEvaluation(int id){
            try{
                var questions = await _serv.GetInitialQuestion(id);
                if(questions == null)
                {
                    return BadRequest("El usuario ya hizo la evaluación inicial");
                }
                else
                {
                    return Ok(questions);
                }
            }
            catch(Exception e)
            {
                return BadRequest("Ha ocurrido un error: " + e.Message);
            }
        }

        [HttpPost]
        [Authorize]
        [Route ("SetInitialEvaluation/{id}")]
        public async Task<IActionResult> SetInitialQuestions(EvaluationDTO answer, int? id){
            try
            {
                if(answer.Option != null)
                {
                    if (await _serv.SetIntialQuestion(answer.Option, id) == 1)
                    {
                        return BadRequest("Los datos no se guardaron");
                    }
                    else if(await _serv.SetIntialQuestion(answer.Option, id) == 0)
                    {
                        return Ok("Los datos se guardaron exitosamente");
                    }
                    else
                    {
                        return BadRequest("Algo salió mal durante el análisis de la evaluación"); 
                    }
                }
                else
                {
                    return BadRequest("El listado de opciones esta vacío");
                }
                
            }
            catch (Exception ex)
            {
                return BadRequest("Ocurrio un error: "+ex.Message);

            }
            
            
        }
    }
}