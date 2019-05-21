using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ALBLOG.Domain.Service;
using Microsoft.AspNetCore.Mvc;

namespace ALBLOG.Web.Controllers
{
    public class PostController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult GetPost(string title)
        {
            PostService postService = new PostService();
            var post = postService.GetPost(i => i.Title == title);
            if (post == null)
                return RedirectToAction("Index", "Home");
            string tags = "";
            post.Tags.ForEach(i => tags += i + " ");
            ViewData.Add("date", post.Date.AddHours(8).ToString("yyyy-MM-dd HH:mm"));
            ViewData.Add("name", post.UserName);
            ViewData.Add("tags", tags);
            ViewData.Add("title", post.Title);
            ViewData.Add("context", post.Context);
            return View();
        }
    }
}