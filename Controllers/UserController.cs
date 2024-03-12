using MAVE.Models;
using MAVE.Repositories;
using MAVE.Utilities;
using Microsoft.AspNetCore.Mvc;


namespace MAVE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DbAa60a4MavetestContext _context;
        private readonly UserRepositories _repo;
        private readonly TokenAndEncipt _token;
        private readonly IConfiguration _config;

        public UserController (DbAa60a4MavetestContext context, IConfiguration configuration, UserRepositories repo, TokenAndEncipt token){
            _context = context;
            _config = configuration;
            _repo = repo;
            _token = token;
        }
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult>Delete(int? id){
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }
            var userDelete = await _repo.GetUserByID(id);
            if (userDelete != null)
            {
                await _repo.DeleteUser(userDelete);
            }
            return Ok();
        }
        [HttpPut]
        [Route("UpdateUser")]
        public async Task<IActionResult>Update([FromForm] User user){
            if (user.NameU == string.Empty){
                ModelState.AddModelError("Nombre","El nombre no puede estar vacio");
            }
            var userU = _repo.GetUserByID(user.Id);
            if (userU == null)return NotFound();
            user.Pass= TokenAndEncipt.HashPass(user.Pass);
            await _repo.UpdateUser(user);
            return Ok();
            
        }
        [HttpPost]
        [Route("SigIn")]
        public async Task<IActionResult>SigIn([FromForm] User user){
            var password = HttpContext.Request.Form["password"];
            var CPass = HttpContext.Request.Form["confpassword"];
            if (user == null) return BadRequest();
            if (user.NameU == string.Empty)
            {
                ModelState.AddModelError("Nombre","Nombre no puede estar vacio");
                if (password != CPass)
                {
                    ModelState.AddModelError("Password","La verificación de contraseña no coincide");
                }   
            }
            var userC = _repo.GetUserByMail(user.Email);
            if (userC != null)
            {
                ModelState.AddModelError("Email","La direccion de Email ya existe en el sistema");
            }
            user.Pass = TokenAndEncipt.HashPass(user.Pass);
            await _repo.CreateUser(user);
            return Ok(Created("created",true));
        }
        [HttpPost]
        [Route("LogIn")]
        public async Task<IActionResult> LogIn([FromBody] User user){
            var UserAct = await _repo.GetUserByMail(user.Email);
            var password = UserAct.Pass;
            if (UserAct == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new {TokenCompleto =""});
            }else if (BCrypt.Net.BCrypt.Verify(user.Pass,password))
            {
                var token = _token.GenerarToken(user.Email);
                return StatusCode(StatusCodes.Status200OK , new {tokenCompleto = token});
            }else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new {TokenCompleto =""});
            } 
        }
    }
}