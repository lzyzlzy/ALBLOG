using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ALBLOG.Domain.Service;
using ALBLOG.Domain.Model;

namespace ALBLOG.Web.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            HttpContext.Session.TryGetValue("username", out byte[] value);
            if (value == null)
                return View("Login");
            else
                ViewBag.Name = Encoding.Default.GetString(value);
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string userName, string password)
        {
            UserService userService = new UserService();
            var user = userService.GetOne(i => i.UserName == userName.Trim());
            if (user == null)
                return Json(new ReturnMessage { Message = "Invalid username." });
            if (user.Password != password.Trim())
                return Json(new ReturnMessage { Message = "Invalid password." });
            HttpContext.Session.Set("username", Encoding.Default.GetBytes(userName));
            return Json(new ReturnMessage { Message = "ok" });
        }

        [HttpPost]
        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("username");
            return Json(new ReturnMessage { Message = "ok" });
        }

        [HttpGet]
        public IActionResult CreatePost()
        {
            HttpContext.Session.TryGetValue("username", out byte[] value);
            if (value == null)
                return View("Login");
            return View();
        }

        [HttpPost]
        public IActionResult CreatePost(string title, string tags, string context)
        {
            HttpContext.Session.TryGetValue("username", out byte[] value);
            if (value == null)
                return Json(new ReturnMessage { Message = "Login Timeout!" });
            PostService postService = new PostService();
            var post = postService.GetPost(i => i.Title == title);
            if (post != null)
                return Json(new ReturnMessage { Message = "this title already exists" });
            List<string> _tags = tags.Split(',', '，').Where(i => i != "").ToList();
            post = new Post
            {
                Title = title,
                UserName = Encoding.Default.GetString(value),
                Tags = _tags,
                Date = DateTime.Now,
                Context = context
            };
            postService.AddPost(post);
            return Json(new ReturnMessage { Message = "ok" });
        }

    }
}