using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ALBLOG.Web.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            HttpContext.Session.TryGetValue("username", out byte[] value);
            if (value == null)
                return View("Login");
            ViewBag.Name = Encoding.Default.GetString(value);
            return View();
        }
    }
}