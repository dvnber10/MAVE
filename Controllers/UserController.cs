using MAVE.Models;
using MAVE.Utilities;
using Microsoft.AspNetCore.Mvc;
using MAVE.Services;


namespace MAVE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UserService _serv;
        private readonly TokenAndEncipt _token;

        public UserController (UserService serv, IConfiguration configuration, TokenAndEncipt token){
            _config = configuration;
            _serv = serv;
            _token = token;
        }
        [HttpDelete]
        [Route("DeleteUser")]
        public async Task<IActionResult>Delete(int? id){
            if (await _serv.UserDelete(id))
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut]
        [Route("UpdateUser")]
        public async Task<IActionResult> UserUpdate([FromForm] UserModel user){
            if (user.NameU == string.Empty)
            {
                ModelState.AddModelError("Nombre", "El nombre no puede estar vacio");
            }
            if (await _serv.UpdateUser(user))
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
            
        }

        [HttpPost]
        [Route("SigIn")]
        public async Task<IActionResult> SigIn([FromForm] UserModel user){
            var password = HttpContext.Request.Form["password"];
            var CPass = HttpContext.Request.Form["confpassword"];
            if (user == null) return BadRequest();
            if (user.NameU == string.Empty)
            {
                ModelState.AddModelError("Nombre", "Nombre no puede estar vacio");
                if (password != CPass)
                {
                    ModelState.AddModelError("Password", "La verificación de contraseña no coincide");
                }
            }
            if(await _serv.CreateUser(user))
            {
                return Ok(Created("created", true));
            }
            else
            {
                return BadRequest(); 
            }
        }

        [HttpPost]
        [Route("LogIn")]
        public async Task<IActionResult> LogIn([FromBody] UserModel user)
        {
            var res = await _serv.LogIn(user);
            if (res == 0)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new {TokenCompleto =""});
            }
            else if (res == 1)
            {
                var token = _token.GenerarToken(user.Email);
                return StatusCode(StatusCodes.Status200OK , new {tokenCompleto = token});
            }
            else 
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new {TokenCompleto =""});
            } 
        }
    }
}