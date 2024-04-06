using MAVE.Models;
using MAVE.Utilities;
using Microsoft.AspNetCore.Mvc;
using MAVE.Services;
using MAVE.DTO;
using Microsoft.AspNetCore.SignalR.Protocol;
using Org.BouncyCastle.Asn1.Iana;


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
                return Ok("Usuario Eliminado correctamente");
            }
            else
            {
                return NotFound("algo fallo al eliminar el usuario");
            }
        }

        [HttpPut]
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
                var userA = _serv.GetUserByMail(user.Email);
                var token = _token.GenerarToken(user.Email,Convert.ToString(userA.Id));
                return Ok(Created(token, true));
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
                return StatusCode(StatusCodes.Status401Unauthorized, new {TokenCompleto =""}+ "\nError Usuario no existe en el sistema");
            }
            else if (res == 1)
            {
                var token = _token.GenerarToken(rest.Email,"1");
                return StatusCode(StatusCodes.Status200OK , new {tokenCompleto = token} + "\nBienvenido");
            }
            else 
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new {TokenCompleto =""}+ "\nCredenciales incorrectas");
            } 
        }
        [HttpPost]
        [Route ("PasswordRecovery")]
        public async Task<IActionResult> RecoveryPass ([FromBody] RecoveryPassDTO email){
            try
            {
                await _serv.RecoveryPass(email.Email);
                return Ok("Email enviado Revisa tu correo electronico");
            }
            catch (System.Exception ex)
            {
                return BadRequest("el email no existe en el sistema" + ex);
                throw;
            }
        }
        [HttpPost]
        [Route ("PasswordReset")]
        public async Task<IActionResult> PasswordReset ([FromBody] UserLogInDTO rest ){
            if (await _serv.ResetPass(rest.Email,rest.Pass)==1 )
            {
                return Ok("Contraseña cambiada correctamente");
            }else{
                return BadRequest("Error al actualizar");
            }
        }
    }
}