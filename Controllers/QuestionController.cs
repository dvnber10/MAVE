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
        public QuestionController(QuestionService serv)
        {
            _serv = serv;
        }

        [HttpGet]
        [Authorize]
        [Route("GetHabitQuestions/{id}")]
        public async Task<IActionResult> GetHabitQuestions(int? id)
        {
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
                int res = await _serv.SetHabitQuestion(id, habit);
                if (res == 0)
                {
                    return Ok("Se guardaron los datos exitósamente");
                }
                else if (res == 1)
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
                return BadRequest("Ocurrió un error: " + e.Message);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("GetInitialEvaluation/{id}")]
        public async Task<IActionResult> GetInitialEvaluation(int id)
        {
            try
            {
                var questions = await _serv.GetInitialQuestion(id);
                if (questions == null)
                {
                    return StatusCode(StatusCodes.Status202Accepted, "El usuario ya realizo la evaluacion inicial");
                }
                else
                {
                    return Ok(questions);
                }
            }
            catch (Exception e)
            {
                return BadRequest("Ha ocurrido un error: " + e.Message);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("SetInitialEvaluation/{id}")]
        public async Task<IActionResult> SetInitialQuestions(EvaluationDTO answer, int? id)
        {
            try
            {
                if (answer.Option == null)
                {
                    return BadRequest("No hay información para guardar");
                } 
                else
                {
                    int res = await _serv.SetIntialQuestion(answer.Option, id);
                    if (answer.Option != null)
                    {
                        if (res == 1)
                        {
                            return BadRequest("Los datos no se guardaron");
                        }
                        else if (res == 0)
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

            }
            catch (Exception ex)
            {
                return BadRequest("Ocurrio un error: " + ex.Message);

            }
        }

        [HttpGet]
        [Authorize]
        [Route("GetInitialGraphic/{id}")]
        public async Task<IActionResult> GetInitialGraphic(int? id)
        {
            try
            {
                InitialGraphicDTO? iniVals = new InitialGraphicDTO();
                if (id == null)
                {
                    return NotFound("El id esta vacío");
                }
                iniVals = await _serv.GetInitialGraphic(id);
                if (iniVals == null)
                {
                    return NotFound("No se encontraron datos");
                }
                else
                {
                    return Ok(iniVals);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Algo salió mal: "+ex);
            }
        } 
    }
}