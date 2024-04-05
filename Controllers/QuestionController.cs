using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IActionResult> InitialQuestions(int id){
            var questions = await _serv.GetInitialQuestion(id);
            if (questions == null)
            {
                return NotFound("las preguntas ya fueron contestadas por el usuario");
            }else
            {
                return Ok(questions);
            }
        }
    }
}