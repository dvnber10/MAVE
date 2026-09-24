using MAVE.Utilities;
using Microsoft.AspNetCore.Mvc;
using MAVE.Services;
using MAVE.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using Microsoft.AspNetCore.Http;


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
        private void SetTokenCookie(string token)
        {
            var secure = Request.IsHttps;
            Response.Cookies.Append("token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = secure,
                SameSite = secure ? SameSiteMode.None : SameSiteMode.Lax,
                Path = "/",
                Expires = DateTimeOffset.UtcNow.AddHours(2)
            });
        }
        private void DeleteTokenCookie()
        {
            var secure = Request.IsHttps;
            Response.Cookies.Delete("token", new CookieOptions
            {
                HttpOnly = true,
                Secure = secure,
                SameSite = secure ? SameSiteMode.None : SameSiteMode.Lax,
                Path = "/"
            });
        }
        [HttpPost]
        [Route("LogOut")]
        public IActionResult LogOut()
        {
            DeleteTokenCookie();
            return Ok("Sesión cerrada");
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
        [Route("UpdateUser/{id}")]
        public async Task<IActionResult> UserUpdate([FromBody] UpdateUserDTO user, int? id){
            if (await _serv.GetUserById(id)== null)
            {
                return BadRequest("Error al encontrar el usuario actual");
            }
            if (await _serv.UpdateUser(user, id))
            {
                return Ok("usuario actualizado correctamente");
            }
            else
            {
                return NotFound("usuario no encontrado en el sistema");
            }
            
        }

        [HttpPut]
        [Authorize]
        [Route("UpdateProfile/{id}")]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileUpdateDTO dto, int? id){
            int res = await _serv.UpdateProfile(dto, id);
            if (res == 1) return Ok("perfil actualizado correctamente");
            else if (res == 2) return BadRequest("Ese correo ya está en uso");
            else return BadRequest("Datos inválidos");
        }

        [HttpPut]
        [Authorize]
        [Route("ChangePassword/{id}")]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordChangeDTO dto, int? id){
            int res = await _serv.ChangePassword(dto, id);
            if (res == 1) return Ok("contraseña actualizada correctamente");
            else if (res == 2) return BadRequest("La contraseña actual no es correcta");
            else return BadRequest("Datos inválidos (mínimo 6 caracteres)");
        }

        [HttpGet]
        [Authorize]
        [Route("MyPatients/{psychologistId}")]
        public async Task<IActionResult> MyPatients(int psychologistId){
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? string.Empty;
            var list = await _serv.GetMyPatients(email, psychologistId);
            if (list == null) return StatusCode(StatusCodes.Status403Forbidden, "No autorizado");
            return Ok(list);
        }

        [HttpPut]
        [Authorize]
        [Route("SetPsychologist/{id}")]
        public async Task<IActionResult> SetPsychologist([FromBody] SetPsychologistDTO dto, int? id){
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? string.Empty;
            int res = await _serv.SetPsychologist(email, id ?? 0, dto.PsychologistId);
            if (res == 1) return Ok("psicólogo asignado correctamente");
            else if (res == 2) return StatusCode(StatusCodes.Status403Forbidden, "No autorizado");
            else return BadRequest("Datos inválidos");
        }

        public class BlockDTO
        {
            public bool Blocked { get; set; }
        }

        [HttpPut]
        [Authorize]
        [Route("BlockUser/{id}")]
        public async Task<IActionResult> BlockUser([FromBody] BlockDTO dto, int? id){
            if (await _serv.BlockUser(id, dto.Blocked) == 1)
                return Ok(dto.Blocked ? "cuenta bloqueada" : "cuenta desbloqueada");
            return BadRequest("Datos inválidos");
        }

        [HttpGet]
        [Authorize]
        [Route("PendingPsychologists")]
        public async Task<IActionResult> PendingPsychologists(){
            return Ok(await _serv.GetPendingPsychologists());
        }

        [HttpPost]
        [Route("RegisterProfessional")]
        public async Task<IActionResult> RegisterProfessional([FromForm] ProfessionalSignupDTO dto, Microsoft.AspNetCore.Http.IFormFile? credential){
            int res = await _serv.RegisterProfessional(dto.UserName, dto.Email, dto.Phone, dto.Password, dto.Description, credential);
            if (res == 1) return Ok("registro recibido, pendiente de verificación");
            else if (res == 2) return BadRequest("Ese correo o nombre ya está en uso");
            else return BadRequest("Datos inválidos");
        }

        [HttpPost]
        [Route("SigIn")]
        public async Task<IActionResult> SigIn(UserSigInDTO user){
            if (user == null) return BadRequest("No se ingresaron todos los datos");
            if (user.UserName == string.Empty)
            {
                ModelState.AddModelError("Nombre", "Nombre no puede estar vacio");
            }
            if (await _serv.GetUserByName(user.UserName) == false) return BadRequest("Este nombre de usuario ya existe intenta utilizar otro");
            if(await _serv.CreateUser(user))
            {
                var userA = await _serv.GetUserByMail(user.Email);
                var token = _token.GenerarToken(user.Email,Convert.ToString(userA.UserId));
                SetTokenCookie(token);
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
                SetTokenCookie(token);
                var userToken = new JsonFile{
                    Id = Convert.ToString(userAct.UserId),
                    Token = token,
                    Message = "Bienvenido al sistema",
                    Status = 5000
                };
                var result = JsonSerializer.Serialize(userToken);
                return StatusCode(StatusCodes.Status200OK , result);
            }
            else if (res == 3)
            {
                var userToken = new JsonFile{
                    Message = "Cuenta bloqueada, contacta al administrador"
                };
                var result = JsonSerializer.Serialize(userToken);
                return StatusCode(StatusCodes.Status403Forbidden, result);
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
                if (user == null)
                {
                    return NotFound("Usuario no encontrado");
                }
                var result = JsonSerializer.Serialize(new
                {
                    user.UserId,
                    user.UserName,
                    user.Email,
                    user.Phone,
                    user.RoleId,
                    user.EvaluationId,
                    user.StatusId,
                    user.HealthProfessionalId
                });
                return Ok(result);   
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex);
                throw;
            }
        }
        [HttpGet]
        [Authorize]
        [Route ("GetAllUsers/{id}")]
        public async Task<IActionResult> GetAllUsers (int? id){
            try
            {
                var users =await _serv.GetAllUsers(id);
                if (users == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest,"Hubo un error al consultar los datos");
                }else
                {
                    return Ok(users);
                }
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest,ex.Message);
                throw;
            }
        }
        [HttpGet]
        [Authorize]
        [Route ("GetPsychologists")]
        public async Task<IActionResult> GetPsychologists (){
            try
            {
                var pros = await _serv.GetPsychologists();
                if (pros == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest,"Hubo un error al consultar los datos");
                }else
                {
                    return Ok(pros);
                }
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest,ex.Message);
                throw;
            }
        }
    }
}