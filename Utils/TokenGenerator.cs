using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Project_for_Aceleration_Csharp_Tryitter.Utils
{
    public class TokenGenerator
    {
        public string CreateToken(string email, bool admin)
        {
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = AddClaims(email, admin),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Token.GetSecret())),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Expires = DateTime.Now.AddDays(1)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private ClaimsIdentity AddClaims(string email, bool admin)
        {
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim("Email", email));
            claims.AddClaim(new Claim("Admin", admin.ToString()));

            return claims;
        }
    }
}
