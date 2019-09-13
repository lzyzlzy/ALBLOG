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
using ALBLOG.Domain.Service.Interface;
using ALBLOG.Constant;
using ALBLOG.Domain.Dto;

namespace ALBLOG.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostService _postService;
        private readonly IUserService _userService;
        private readonly ISettingService _settingService;

        public HomeController(IPostService postService, IUserService userService, ISettingService settingService)
        {
            this._postService = postService;
            this._userService = userService;
            this._settingService = settingService;
        }

        public async Task<IActionResult> Index(int index = 1)
        {
            var page = await _postService.GetPageAsync(i => i.IsDraft == false, GlobalConfig.PostPageSize, index);
            return View(page);
        }

        public IActionResult CV()
        {
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

        [HttpGet]
        public async Task<IActionResult> Post(string Id)
        {
            HttpContext.Session.TryGetValue("username", out byte[] value);
            var post = value != null ? await _postService.GetOneAsync(i => i.Id == Id)
                                     : await _postService.GetOneAndAddPageViewsAsync(i => i.Id == Id);
            if (post == null)
                return RedirectToAction("Index", "Home");
            return View(post);
        }

        [HttpGet]
        public async Task<IActionResult> Tag(string name, int index = 1)
        {
            var page = await _postService.GetPageAsync(i => i.IsDraft == false && i.Tags.Contains(name), GlobalConfig.PostPageSize, index);
            ViewData.Add("Title", name);
            return View(page);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string key)
        {
            if (key.IsNullOrEmpty())
                return RedirectToAction("index");
            ViewData.Add("Title", key);
            key = key.ToLower();
            var page = await _postService.GetPageAsync(i => (i.Title.ToLower().Contains(key) || i.Tags.Contains(key) || i.Context.ToLower().Contains(key)) && i.IsDraft == false, 100, 1);
            return View(page);
        }

        [HttpGet]
        public async Task<IActionResult> GetProfilePhotoPath()
        {
            return Json(new ReturnDto { Data = (await _settingService.GetProfileImgPathAsync()).ShowPath, State = "success" });
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            return Json(new ReturnDto { Data = await _settingService.GetProfileAsync(), Message = "ok", State = "sucess" });
        }

        [HttpGet]
        public async Task<IActionResult> GetCV()
        {
            var service = new SettingService();
            return Json(new ReturnDto { Data = await _settingService.GetCVAsync(), Message = "ok", State = "sucess" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAbout()
        {
            var service = new SettingService();
            return Json(new ReturnDto { Data = await _settingService.GetAboutAsync(), Message = "ok", State = "success" });
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserDto userDto)
        {
            var returnModel = await _userService.LoginAsync(userDto.userName, userDto.password);
            if (returnModel.IsSuccess)
            {
                HttpContext.Session.Set("username", Encoding.Default.GetBytes(userDto.userName));
                return Json(new ReturnDto { Message = "ok" });
            }
            else
            {
                return Json(new ReturnDto { State = "fail", Message = returnModel.Message });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
