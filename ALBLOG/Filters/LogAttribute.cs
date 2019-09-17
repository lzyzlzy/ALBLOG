using ALBLOG.Domain.Service.Interface;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ALBLOG.Web.Attributes
{
    public class LogAttribute : IActionFilter
    {
        private readonly ILogService _logService;

        public LogAttribute(ILogService logService)
        {
            this._logService = logService;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Session.TryGetValue("username", out byte[] value);
            var remoteIpAddress = context.HttpContext.Connection.RemoteIpAddress;
            var sessionId = context.HttpContext.Session.Id;
            var routeData = context.RouteData;
            var controllerName = context.RouteData.Values["controller"] as string;
            var actionName = context.RouteData.Values["action"] as string;
            var ipAddress = context.HttpContext.Connection.RemoteIpAddress.ToString();
            var content = context.HttpContext.Request.QueryString.Value;
            var isAdmin = value != null;
            if (context.HttpContext.Request.Headers.TryGetValue("X-Real-IP", out StringValues ipValue))
            {
                ipAddress = StringValues.IsNullOrEmpty(ipValue) == false ? ipValue.ToString() : ipAddress;
            }
            _logService.Log(sessionId, controllerName, actionName, ipAddress, content.TrimEnd(']'), isAdmin);
        }
    }
}
