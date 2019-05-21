using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ALBLOG.Models;
using ALBLOG.Domain.Service;
using System.Text;
using ALBLOG.Domain.Model;

namespace ALBLOG.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            PostService postService = new PostService();
            List<Post> posts = postService.GetAllPosts().ToList();
            ViewData.Add("posts", posts);
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
