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
            var SecretKey = _config["Key:secretKey"];
            #pragma warning disable CS8604 // Possible null reference argument.
            var security= Encoding.ASCII.GetBytes(SecretKey);
            #pragma warning restore CS8604 // Possible null reference argument.
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(new []{
                    new Claim(ClaimTypes.Email,mail),
                    new Claim(ClaimTypes.Role,rol)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(security){KeyId = "mave-signing-key"}, SecurityAlgorithms.HmacSha256Signature) 
            };
            var TokenHandler = new JwtSecurityTokenHandler();
            var token = TokenHandler.CreateToken(tokenDescriptor);
            return TokenHandler.WriteToken(token);
        }
        /// <summary>
        /// Valida un JWT con la misma llave y vigencia del login.
        /// Devuelve (Ok, Email, UserId). Nunca lanza.
        /// </summary>
        public (bool Ok, string? Email, string? UserId) ValidateToken(string? token)
        {
            if (string.IsNullOrWhiteSpace(token)) return (false, null, null);
            try
            {
                var secret = _config["Key:secretKey"];
                if (string.IsNullOrEmpty(secret)) return (false, null, null);
                var handler = new JwtSecurityTokenHandler();
                var parameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)){ KeyId = "mave-signing-key" },
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                var principal = handler.ValidateToken(token.Trim(), parameters, out _);
                var email = principal.FindFirst(ClaimTypes.Email)?.Value;
                var uid = principal.FindFirst(ClaimTypes.Role)?.Value;
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(uid)) return (false, null, null);
                return (true, email, uid);
            }
            catch { return (false, null, null); }
        }
    }
}