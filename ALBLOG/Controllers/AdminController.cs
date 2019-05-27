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
            var allPosts = postService.GetAllPosts(i => i.IsDraft == false);
            var posts = allPosts.Skip((index - 1) * 10).Take(postNumOfOnePage).ToList();
            var draftCount= postService.GetAllPosts(i => i.IsDraft == true).Count();
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
            ViewData.Add("draftCount", draftCount);
            return View();
        }

        public IActionResult Drafts(int index = 1)
        {
            HttpContext.Session.TryGetValue("username", out byte[] value);
            if (value == null)
                return View("Login");
            else
                ViewBag.Name = Encoding.Default.GetString(value);
            int postNumOfOnePage = 10;
            PostService postService = new PostService();
            var allPosts = postService.GetAllPosts(i => i.IsDraft == true);
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

        public IActionResult DeletePost(string title)
        {
            PostService postService = new PostService();
            postService.Delete(title.Trim());
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Login(UserDto userDto)
        {
            UserService userService = new UserService();
            var user = userService.GetOne(i => i.UserName == userDto.userName.Trim());
            if (user == null)
                return Json(new ReturnDto { Message = "Invalid username." });
            if (user.Password != userDto.password.Trim())
                return Json(new ReturnDto { Message = "Invalid password." });
            HttpContext.Session.Set("username", Encoding.Default.GetBytes(userDto.userName));
            return Json(new ReturnDto { Message = "ok" });
        }

        [HttpPost]
        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("username");
            return Json(new ReturnDto { Message = "ok" });
        }

        public IActionResult EditPost(string title)
        {
            ViewData["Title"] = "Edit";
            PostService postService = new PostService();
            var post = postService.GetPost(i => i.Title == title.Trim());
            ViewData.Add("postTitle", post.Title);
            ViewData.Add("tags", post.Tags);
            ViewData.Add("context", post.Context);
            return View("CreatePost");
        }

        [HttpGet]
        public IActionResult CreatePost()
        {
            ViewData["Title"] = "Create";
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
                return Json(new ReturnDto { Message = "Login Timeout!" });
            PostService postService = new PostService();
            postService.Delete(postDto.title.Trim());
            List<string> _tags = postDto.tags.Split(',', '，').Where(i => i != "").ToList();
            var _post = new Post
            {
                Title = postDto.title,
                UserName = Encoding.Default.GetString(value),
                Tags = _tags,
                Date = DateTime.Now,
                Context = postDto.context,
                IsDraft = false
            };
            postService.AddPost(_post);
            return Json(new ReturnDto { Message = "ok" });
        }
        [HttpPost]
        public IActionResult CreateDraft(PostDto postDto)
        {
            HttpContext.Session.TryGetValue("username", out byte[] value);
            if (value == null)
                return Json(new ReturnDto { Message = "Login Timeout!" });
            PostService postService = new PostService();
            postService.Delete(postDto.title.Trim());
            List<string> _tags = postDto.tags.Split(',', '，').Where(i => i != "").ToList();
            var _post = new Post
            {
                Title = postDto.title,
                UserName = Encoding.Default.GetString(value),
                Tags = _tags,
                Date = DateTime.Now,
                Context = postDto.context,
                IsDraft = true
            };
            postService.AddPost(_post);
            return Json(new ReturnDto { Message = "ok" });
        }

        public IActionResult PostDraft(string title)
        {
            PostService postService = new PostService();
            var post = postService.GetPost(i => i.Title == title.Trim());
            post.IsDraft = false;
            postService.Update(post);
            return RedirectToAction("Drafts");
        }
        public IActionResult GoToDraftBox(string title)
        {
            PostService postService = new PostService();
            var post = postService.GetPost(i => i.Title == title.Trim());
            post.IsDraft = true;
            postService.Update(post);
            return RedirectToAction("Index");
        }
    }
}