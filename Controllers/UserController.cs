using MAVE.Utilities;
using Microsoft.AspNetCore.Mvc;
using MAVE.Services;
using MAVE.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;


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
        [Authorize]
        [Route("DeleteUser/{id}")]
        public async Task<IActionResult>Delete(int? id){
            if (await _serv.UserDelete(id))
            {
                return Ok("Usuario Eliminado correctamente");
            }
            else
            {
                return NotFound("algo fallo al eliminar el usuario");
            }
        }

        [HttpPut]
        [Authorize]
        [Route("UpdateUser")]
        public async Task<IActionResult> UserUpdate([FromBody] UserSigInDTO user){
            if (user.UserName == string.Empty)
            {
                ModelState.AddModelError("Nombre", "El nombre no puede estar vacio");
            }
            if (await _serv.UpdateUser(user))
            {
                return Ok("usuario creado correctamente");
            }
            else
            {
                return NotFound("usuario no encontrado en el sistema");
            }
            
        }

        [HttpPost]
        [Route("SigIn")]
        public async Task<IActionResult> SigIn(UserSigInDTO user){
            if (user == null) return BadRequest();
            if (user.UserName == string.Empty)
            {
                ModelState.AddModelError("Nombre", "Nombre no puede estar vacio");
            }
            if(await _serv.CreateUser(user))
            {
                var userA = await _serv.GetUserByMail(user.Email);
                var token = _token.GenerarToken(user.Email,Convert.ToString(userA.UserId));
                var userToken = new JsonFile{
                    Id = Convert.ToString(userA.UserId),
                    Token = token,
                    Message = "Bienvenido al sistema"
                };
                var result = JsonSerializer.Serialize(userToken);
                return StatusCode(StatusCodes.Status200OK , result);
            }
            else
            {
                return BadRequest("Este mail ya existe en el sistema"); 
            }
        }

        [HttpPost]
        [Route("LogIn")]
        public async Task<IActionResult> LogIn([FromBody] UserLogInDTO rest)
        {
            var res = await _serv.LogIn(rest.Email, rest.Pass);
            if (res == 0)
            {
                var userToken = new JsonFile{
                    Message = "Usuario no existe",
                };
                var result = JsonSerializer.Serialize(userToken);
                return StatusCode(StatusCodes.Status404NotFound, result);
            }
            else if (res == 1)
            {
                var userAct =await _serv.GetUserByMail(rest.Email);
                var token = _token.GenerarToken(rest.Email,Convert.ToString(userAct.UserId));
                var userToken = new JsonFile{
                    Id = Convert.ToString(userAct.UserId),
                    Token = token,
                    Message = "Bienvenido al sistema",
                    Status = 5000
                };
                var result = JsonSerializer.Serialize(userToken);
                return StatusCode(StatusCodes.Status200OK , result);
            }
            else 
            {
                var userToken = new JsonFile{
                    Message = "Usuario y contraseña incorrectas"
                };
                var result = JsonSerializer.Serialize(userToken);
                return StatusCode(StatusCodes.Status401Unauthorized, result);
            } 
        }
        [HttpPost]
        [Route ("PasswordRecovery")]
        public async Task<IActionResult> RecoveryPass ([FromBody] RecoveryPassDTO email){
            try
            {
                await _serv.RecoveryPass(email.Data);
                return Ok("Email enviado Revisa tu correo electronico");
            }
            catch (System.Exception ex)
            {
                return BadRequest("el email no existe en el sistema" + ex);
                throw;
            }
        }
        [HttpPut]
        [Authorize]
        [Route ("PasswordReset/{id}")]
        public async Task<IActionResult> PasswordReset ([FromBody] RecoveryPassDTO rest, int? id ){
            if (await _serv.ResetPass(id,rest.Data)==1 )
            {
                return Ok("Contraseña cambiada correctamente");
            }else{
                return BadRequest("Error al actualizar");
            }
        }
        [HttpGet]
        [Authorize]
        [Route ("GetUserInfo/{id}")]
        public async Task<IActionResult> GetUserInfo (int? id){
            try
            {
                var user = await _serv.GetUserById(id);
                var result = JsonSerializer.Serialize(user);
                return Ok(result);   
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex);
                throw;
            }
        }
    }
}