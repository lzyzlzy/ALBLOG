using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace ALBLOG.Web.Attributes
{
    public class AuthAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
#if DEBUG
            context.HttpContext.Session.Set("username", Encoding.Default.GetBytes("Debuger"));
#endif
            context.HttpContext.Session.TryGetValue("username", out byte[] value);
            if (value == null)
                context.Result = new RedirectToActionResult("login", "Home", null);
        }
    }
}
