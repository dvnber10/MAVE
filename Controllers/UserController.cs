using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MAVE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;


namespace MAVE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DbAa60a4MavetestContext _context;
        private readonly IConfiguration _config;

        public UserController (DbAa60a4MavetestContext context, IConfiguration configuration){
            _context = context;
            _config = configuration;
        }
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult>Delete(int? id){
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }
            var userDelete = await _context.Users.FindAsync(id);
            if (userDelete != null)
            {
                _context.Remove(userDelete);
            }
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpPut]
        [Route("UpdateUser")]
        public async Task<IActionResult>Update([FromForm] User user){
            if (user.NameU == string.Empty){
                ModelState.AddModelError("Nombre","El nombre no puede estar vacio");
            }
            var userU =await _context.Users.FindAsync(user.Id);
            if (userU == null)return NotFound();
            user.Pass= HashPass(user.Pass);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
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
            var userC = await _context.Users.Where(c => c.Email ==user.Email).FirstOrDefaultAsync();
            if (userC != null)
            {
                ModelState.AddModelError("Email","La direccion de Email ya existe en el sistema");
            }
            user.Pass = HashPass(user.Pass);
            _context.Add(user);
            _context.SaveChanges(); 
            return Ok(Created("created",true));
        }
        [HttpPost]
        [Route("LogIn")]
        public async Task<IActionResult> LogIn([FromBody] User usuario){
            var UserAct = await _context.Users.Where(u => u.Email == usuario.Email).FirstOrDefaultAsync();
            var password = UserAct.Pass;
            if (UserAct == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new {TokenCompleto =""});
            }else if (BCrypt.Net.BCrypt.Verify(usuario.Pass,password))
            {
                var token = GenerarToken(usuario.Email);
                return StatusCode(StatusCodes.Status200OK , new {tokenCompleto = token});
            }else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new {TokenCompleto =""});
            } 
            
            
        }

        public static string HashPass(string HashPass){
            string PassEn = BCrypt.Net.BCrypt.HashPassword(HashPass, BCrypt.Net.BCrypt.GenerateSalt());
            return PassEn;
        }
        private string GenerarToken(string mail){
            var SecretKey = _config.GetSection("Key").GetSection("secretKey").ToString();
            #pragma warning disable CS8604 // Possible null reference argument.
            var security= Encoding.ASCII.GetBytes(SecretKey);
            #pragma warning restore CS8604 // Possible null reference argument.
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(new []{
                    new Claim(ClaimTypes.Email,mail)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(security), SecurityAlgorithms.HmacSha256Signature) 
            };
            var TokenHandler = new JwtSecurityTokenHandler();
            var token = TokenHandler.CreateToken(tokenDescriptor);
            return TokenHandler.WriteToken(token); 
        }
    }
}