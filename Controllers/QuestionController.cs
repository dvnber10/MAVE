
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
        [Route ("InitialQuestions")]
        public async Task<IActionResult> InitialQuestions(EvaluationDTO answer){
            //var questions = await _serv.GetInitialQuestion(id);
            try
            {
                if (await _serv.SetIntialQuestion(answer.Option, answer.Id) == 1)
                {
                    return BadRequest("Los datos no se guardaron");
                }
                else if(await _serv.SetIntialQuestion(answer.Option, answer.Id) == 0)
                {
                    return Ok("Los datos se guardaron exitosamente");
                }
                else
                {
                    return BadRequest("Algo salió mal durante el análisis de la evaluación"); 
                }
            }catch (Exception ex)
            {
                return BadRequest("Ocurrio un error: "+ex.Message);
            }
        
    }
    }
}