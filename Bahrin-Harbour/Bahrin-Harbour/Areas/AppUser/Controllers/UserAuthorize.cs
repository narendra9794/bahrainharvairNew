using Bahrin.Harbour.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Bahrin_Harbour.Areas.AppUser.Controllers
{
    public class UserAuthorize : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {

            var authHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (authHeader != null && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();

                var tokenHandler = new JwtSecurityTokenHandler();
                try
                {
                    var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("12345678901234567890123456789012")), 
                        ValidateIssuer = false, 
                        ValidateAudience = false, 
                        ClockSkew = TimeSpan.Zero 
                    }, out SecurityToken validatedToken);

                    if (principal.IsInRole(Constants.AppUser))
                    {
                        var jwtToken = validatedToken as JwtSecurityToken;
                        var sidClaim = jwtToken?.Claims.Where(x=>x.Type == "nameid").FirstOrDefault().Value;

                        if (!string.IsNullOrEmpty(sidClaim))
                        {
                            context.HttpContext.Items["RepresentativeId"] = sidClaim;
                        }

                        return; 
                    }
                }
                catch (SecurityTokenException)
                {

                }
            }

            context.Result = new UnauthorizedResult();
        }
    }
}
