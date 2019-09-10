using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ALBLOG.Web.Attributes
{
    public class LogoutAttribute : ActionFilterAttribute, IActionFilter
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            context.HttpContext.Session.TryGetValue("username", out byte[] value);
            if (value == null)
                context.Result = new RedirectToActionResult("login", "Home", null);
        }
    }
}
