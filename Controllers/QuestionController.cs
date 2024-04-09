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
        public QuestionController(QuestionService service){
            _serv = service;
        }
        [HttpGet]
        [Authorize]
        [Route("HabitQuestions/{id}")]
        public async Task<IActionResult> HabitQuestions(int? id){
            try
            {
                var questions = await _serv.GetHabitQuestion(id);
                return Ok(questions); 
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
            
            
        }
    }
}