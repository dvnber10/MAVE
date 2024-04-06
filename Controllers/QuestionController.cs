using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAVE.DTO;
using MAVE.Services;
using Microsoft.AspNetCore.Mvc;

namespace MAVE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly QuestionService _serv;
        public QuestionController(QuestionService service){
            _serv = service;
        }
        [HttpGet]
        [Route("InitialQuestions")]
        public async Task<IActionResult> InitialQuestions(EvaluationDTO answer, int? Id){
            //var questions = await _serv.GetInitialQuestion(id);
            
            if (await _serv.SetIntialQuestion(answer.Option, Id) == 1)
            {
                return NotFound("Los datos no se guardaron");
            }else
            {
                return Ok("Los datos se guardaron exitosamente");
            }
        }
    }
}