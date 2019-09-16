using ALBLOG.Domain.Service.Interface;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ALBLOG.Web.Attributes
{
    public class GlobelExceptionFilter : IExceptionFilter
    {
        private readonly ILogService _logService;

        public GlobelExceptionFilter(ILogService logService)
        {
            this._logService = logService;
        }

        public void OnException(ExceptionContext context)
        {
            context.HttpContext.Session.TryGetValue("username", out byte[] value);
            var remoteIpAddress = context.HttpContext.Connection.RemoteIpAddress;
            var sessionId = context.HttpContext.Session.Id;
            var routeData = context.RouteData;
            var controllerName = context.RouteData.Values["controller"];
            var actionName = context.RouteData.Values["action"];
            var ipAddress = context.HttpContext.Connection.RemoteIpAddress.ToString();
            var content = $"{context.Exception.Message}\n{context.Exception.StackTrace}";
            bool isAdmin = value != null;
            if (context.HttpContext.Request.Headers.TryGetValue("X-Real-IP", out StringValues ipValue))
            {
                ipAddress = StringValues.IsNullOrEmpty(ipValue) == false ? ipValue.ToString() : ipAddress;
            }
            _logService.Exception(sessionId, controllerName as string, actionName as string, ipAddress, content, isAdmin);
        }
    }
}
