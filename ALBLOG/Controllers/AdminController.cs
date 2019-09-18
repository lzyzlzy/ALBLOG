using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ALBLOG.Domain.Service;
using ALBLOG.Domain.Model;
using ALBLOG.Domain.Dto;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;
using ALBLOG.Constant;
using ALBLOG.Domain.Service.Interface;
using System.Linq.Expressions;
using ALBLOG.Web.Attributes;

namespace ALBLOG.Web.Controllers
{
    [Auth]
    [TypeFilter(typeof(LogAttribute))]
    public class AdminController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IPostService _postService;
        private readonly IUserService _userService;
        private readonly ISettingService _settingService;

        public AdminController(IHostingEnvironment hostingEnvironment, IPostService postService, IUserService userService, ISettingService settingService)
        {
            _hostingEnvironment = hostingEnvironment;
            _postService = postService;
            _userService = userService;
            _settingService = settingService;
        }

        public async Task<IActionResult> Index(int index = 1)
        {
            HttpContext.Session.TryGetValue("username", out byte[] value);
            ViewBag.Name = Encoding.Default.GetString(value);
            var page = await _postService.GetPageAsync(i => i.IsDraft == false, GlobalConfig.AdminPageSize, index);
            var draftCount = (await _postService.GetAllAsync(i => i.IsDraft == true)).Count();
            ViewData.Add("draftCount", draftCount);
            return View(page);
        }

        [HttpGet]
        public async Task<IActionResult> Drafts(int index = 1)
        {
            var page = await _postService.GetPageAsync(i => i.IsDraft == true, GlobalConfig.AdminPageSize, index);
            return View(page);
        }

        [HttpGet]
        public async Task<IActionResult> DeletePost(string id)
        {
            await _postService.DeleteAsync(i => i.Id == id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("username");
            return Json(new ReturnDto { Message = "ok" });
        }

        [HttpGet]
        public async Task<IActionResult> EditPost(string id)
        {
            ViewData["Title"] = "Edit";
            var post = await _postService.GetOneAsync(i => i.Id == id);
            return View("Post", post);
        }

        [HttpGet]
        public IActionResult Post()
        {
            ViewData["Title"] = "Create";
            return View(ALBLOG.Domain.Model.Post.CreateEmptyPost());
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(PostDto post)
        {
            var tags = post.tags.Split(',').ToList();
            await _postService.EditAsync(post.id, post.title, post.context, tags);
            return Json(new ReturnDto { Message = "ok" });
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(PostDto postDto, bool isDraft = false)
        {
            var isExist = (await _postService.GetOneAsync(i => i.Title == postDto.title)) != null;
            if (isExist)
                return Json(new ReturnDto { State = "fail", Message = "存在相同标题的文章，请更改标题后重试" });
            var tags = postDto.tags.Split(',', '，').Where(i => i != "").ToList();
            await _postService.AddAsync(postDto.title, tags, postDto.context, isDraft);
            return Json(new ReturnDto { Message = "ok" });
        }

        [HttpGet]
        public async Task<IActionResult> PostDraft(string id)
        {
            await _postService.ChangeDraftToPostAsync(i => i.Id == id);
            return RedirectToAction("Drafts");
        }

        [HttpGet]
        public async Task<IActionResult> GoToDraftBox(string id)
        {
            await _postService.ChangePostToDraftAsync(i => i.Id == id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpLoad(UpLoadImgDto imgDto)
        {
            return Json(new UpLoadImgDto { Errno = 0, Data = (await UpLoadImg(_hostingEnvironment)) });
        }

        [HttpPost]
        public async Task<IActionResult> UpLoadProfileImg(string url)
        {
            await _settingService.ChangeProfileImgPathAsync(url);
            return Json(new ReturnDto { State = "success", Message = "success", Data = url });
        }

        private async Task<List<string>> UpLoadImg(IHostingEnvironment environment)
        {
            List<string> pathList = new List<string>();
            var fileDir = Path.Combine(environment.WebRootPath, "images", DateTime.Now.ToString("yyyy-MM-dd"));
            var showDir = $@"/images/{DateTime.Now.ToString("yyyy-MM-dd")}/";
            if (!Directory.Exists(fileDir))
                Directory.CreateDirectory(fileDir);
            foreach (var file in Request.Form.Files)
            {
                //var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).Name.Trim('"');
                var filePath = fileDir + "/" + file.FileName;
                using (FileStream fileStream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(fileStream);
                    fileStream.Flush();
                }
                pathList.Add(showDir + file.FileName);
            }
            return pathList;
        }

        [HttpPost]
        public async Task<IActionResult> ChangeIntroduction(SettingType type, string context)
        {
            try
            {
                switch (type)
                {
                    case SettingType.Profile:
                        await _settingService.ChangeProfileAsync(context);
                        break;
                    case SettingType.CV:
                        await _settingService.ChangeCVAsync(context);
                        break;
                    case SettingType.About:
                        await _settingService.ChangeAboutAsync(context);
                        break;
                    default:
                        break;
                }
                return Json(new ReturnDto { Message = "ok", State = "success" });
            }
            catch (Exception e)
            {
                return Json(new ReturnDto { Message = e.Message, State = "error" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            ViewData["context"] = await _settingService.GetProfileAsync();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CV()
        {
            ViewData["context"] = await _settingService.GetCVAsync();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> About()
        {
            ViewData["context"] = await _settingService.GetAboutAsync();
            return View();
        }

        [HttpGet]
        public IActionResult Image()
        {
            var rootPath = _hostingEnvironment.WebRootPath;
            var extensions = GlobalConfig.ImgExtensions;
            var fileDir = Path.Combine(rootPath, "images");
            var files = PathHelper.FindFileByExtension(fileDir, extensions)
                                  .Select(i => i.GetShowPath())
                                  .ToList();
            ViewData.Add("files", files);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeProfilePhoto()
        {
            return Json(new ReturnDto { Data = (await _settingService.GetProfileImgPathAsync()).ShowPath, State = "success" });
        }
    }
}