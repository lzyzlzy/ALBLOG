using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ALBLOG.Domain.Service;
using ALBLOG.Domain.Model;
using ALBLOG.Domain.Dto;

namespace ALBLOG.Web.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index(int index = 1)
        {
            HttpContext.Session.TryGetValue("username", out byte[] value);
            if (value == null)
                return View("Login");
            else
                ViewBag.Name = Encoding.Default.GetString(value);
            int postNumOfOnePage = 10;
            PostService postService = new PostService();
            var allPosts = postService.GetAllPosts();
            var posts = allPosts.Skip((index - 1) * 10).Take(postNumOfOnePage).ToList();
            if (posts.Count == 0)
            {
                posts = allPosts.Take(10).ToList();
                index = 1;
            }
            ViewData.Add("haveNext", allPosts.Count() > index * postNumOfOnePage ? "true" : "false");
            ViewData.Add("haveLast", index > 1 ? "true" : "false");
            ViewData.Add("posts", posts);
            ViewData.Add("sum", allPosts.Count() % postNumOfOnePage != 0 ? allPosts.Count() / postNumOfOnePage + 1 : allPosts.Count() / postNumOfOnePage);
            ViewData.Add("page", index);
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserDto userDto)
        {
            UserService userService = new UserService();
            var user = userService.GetOne(i => i.UserName == userDto.userName.Trim());
            if (user == null)
                return Json(new ReturnMessage { Message = "Invalid username." });
            if (user.Password != userDto.password.Trim())
                return Json(new ReturnMessage { Message = "Invalid password." });
            HttpContext.Session.Set("username", Encoding.Default.GetBytes(userDto.userName));
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
        public IActionResult CreatePost(PostDto postDto)
        {
            HttpContext.Session.TryGetValue("username", out byte[] value);
            if (value == null)
                return Json(new ReturnMessage { Message = "Login Timeout!" });
            PostService postService = new PostService();
            var post = postService.GetPost(i => i.Title == postDto.title);
            if (post != null)
                return Json(new ReturnMessage { Message = "this title already exists" });
            List<string> _tags = postDto.tags.Split(',', '，').Where(i => i != "").ToList();
            post = new Post
            {
                Title = postDto.title,
                UserName = Encoding.Default.GetString(value),
                Tags = _tags,
                Date = DateTime.Now,
                Context = postDto.context
            };
            postService.AddPost(post);
            return Json(new ReturnMessage { Message = "ok" });
        }

    }
}