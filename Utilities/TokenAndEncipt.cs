using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MAVE.Utilities
{
    public class TokenAndEncipt
    {
        private readonly IConfiguration _config;
        public TokenAndEncipt(IConfiguration config ){
            _config = config;
        }
        public static string HashPass(string HashPass){
            string PassEn = BCrypt.Net.BCrypt.HashPassword(HashPass, BCrypt.Net.BCrypt.GenerateSalt());
            return PassEn;
        }
        public string GenerarToken(string mail, string rol){
            var SecretKey = _config.GetSection("Key").GetSection("secretKey").ToString();
            #pragma warning disable CS8604 // Possible null reference argument.
            var security= Encoding.ASCII.GetBytes(SecretKey);
            #pragma warning restore CS8604 // Possible null reference argument.
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(new []{
                    new Claim(ClaimTypes.Email,mail),
                    new Claim(ClaimTypes.Role,rol)
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