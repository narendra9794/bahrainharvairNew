using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Bahrin_Harbour.Areas.Administration.Controllers
{
    public class AdminAuthorize : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        
        {
            if (context.HttpContext.Session.IsAvailable && context.HttpContext.User.Identity.IsAuthenticated && context.HttpContext.Session.Keys.Count() != 0)
            {
                if (context.HttpContext.User.IsInRole("SuperAdmin"))
                {
                    return;
                }
            }

            context.Result = new RedirectToActionResult("Signin", "Account", null);
        }
    }
}
