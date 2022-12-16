using Microsoft.Extensions.Primitives;
using Project_for_Aceleration_Csharp_Tryitter.Models;
using System.Text.RegularExpressions;

namespace Project_for_Aceleration_Csharp_Tryitter.Validate
{
    public static class Validate
    {
        public static bool ValidateUser(User? user)
        {
            return user is not null;
        }
        public static bool ValidateAdminUser(User? user)
        {
            return user is not null;
        }

        public static bool IsAdminUser(StringValues token)
        {
            var claims = Utils.Token.GetTokenClaims(token);

            return bool.Parse(claims.Claims.ElementAt(1).Value.ToString());
        }
        
        public static bool ValidateEmail(string email)
        {
            string expression = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";

            if (Regex.IsMatch(email, expression) && Regex.Replace(email, expression, string.Empty).Length == 0)
            {
                return true;
            }
            return false;
        }
    }
}
