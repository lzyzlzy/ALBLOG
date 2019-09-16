using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ALBLOG.Constant;
using ALBLOG.Domain.Service.Interface;
using ALBLOG.Web.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace ALBLOG.Web.Controllers
{
    [Auth]
    public class LogController : Controller
    {
        private readonly ILogService _logService;

        public LogController(ILogService logService)
        {
            this._logService = logService;
        }

        public async Task<IActionResult> Index(int index = 1)
        {
            var page = await _logService.GetPageAsync(GlobalConfig.LogPageSize, index);
            return View(page);
        }

        public async Task<IActionResult> Admin(int index = 1)
        {
            var page = await _logService.GetPageAsync(i => i.IsAdmin, GlobalConfig.LogPageSize, index);
            return View(page);
        }

        public async Task<IActionResult> Visitor(int index = 1)
        {
            var page = await _logService.GetPageAsync(i => i.IsAdmin == false, GlobalConfig.LogPageSize, index);
            return View(page);
        }
    }
}