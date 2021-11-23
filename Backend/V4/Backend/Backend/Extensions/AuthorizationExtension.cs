using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Backend.Extensions
{
    public static class AuthorizationExtension
    {
        public static long? GetUserId(this HttpContext context)
        {
            string userIdStr = context.User.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
            
            if (Int64.TryParse(userIdStr, out Int64 result))
            {
                return result;
            }

            return null;
        }
    }
}