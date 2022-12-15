using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;

namespace Project_for_Aceleration_Csharp_Tryitter.Utils
{
    public static class Token
    {
        public static JwtSecurityToken GetTokenClaims(StringValues token)
        {
            var jwt = token.ToString();

            if (jwt.Contains("Bearer"))
            {
                jwt = jwt.Replace("Bearer", "").Trim();
            }

            var handler = new JwtSecurityTokenHandler();

            return handler.ReadJwtToken(jwt); ;
        }

        public static string GetSecret()
        {
            return "*test#project123*";
        }
    }
}
