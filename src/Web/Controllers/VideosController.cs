using System;
using System.Linq;
using System.Threading.Tasks;
using Common.Constants;
using Common.Helpers;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts.Data;
using Services.Mapping;
using Web.Models.Videos;

namespace Web.Controllers
{
    public class VideosController : Controller
    {
        private const int videosOnPage = 10;

        private readonly IVideosService videosService;
        private readonly UserManager<ApplicationUser> userManager;

        public VideosController(IVideosService videosService , UserManager<ApplicationUser> userManager)
        {
            this.videosService = videosService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index(int id = 1)
        {
            var pageCount = (int)Math.Ceiling((double)videosService.CountAllFilms() / videosOnPage);
            if (id > pageCount || id < 1)
            {
                id = 1;
            }

            var videos = await videosService.GetVideosOnPageAsync<VideoViewModel>(id, videosOnPage);
            var viewModel = new VideosIndexViewModel
            {
                Videos = videos.ToList(),
                CurrentPage = 1,
                PagesCount = pageCount
            };

            return this.View(viewModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            if (!(User.IsInRole(ApplicationRolesNames.EditorRole) || User.IsInRole(ApplicationRolesNames.AdminRole)))
            {
                return this.RedirectToAction("Index", "Videos");
            }

            return this.View();
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(VideoInputModel inputModel)
        {
            if (!(User.IsInRole(ApplicationRolesNames.EditorRole) || User.IsInRole(ApplicationRolesNames.AdminRole)))
            {
                return this.RedirectToAction("Index", "Videos");
            }

            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var user = await userManager.GetUserAsync(User);
            var video = await videosService.CreateAsync(
                inputModel.YouTubeId,
                inputModel.Title,
                inputModel.Description,
                inputModel.PremiredOn.ConvertToUtc(),
                inputModel.IsVisible,
                user);

            return this.RedirectToAction("Watch", "Videos", new { video.Id });
        }

        public async Task<IActionResult> Watch(string id)
        {
            var viewModel = await videosService.GetVideoByIdAsync<VideoViewModel>(id);
            if (viewModel == null)
            {
                return NotFound();
            }

            return this.View(viewModel);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (!(User.IsInRole(ApplicationRolesNames.EditorRole) || User.IsInRole(ApplicationRolesNames.AdminRole)))
            {
                return this.RedirectToAction("Index", "Videos");
            }

            var inputModel = await videosService.GetVideoByIdAsync<VideoInputModel>(id);

            if(inputModel==null)
            {
                return this.NotFound();
            }

            return this.View(inputModel);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(VideoInputModel inputModel)
        {
            if (!(User.IsInRole(ApplicationRolesNames.EditorRole) || User.IsInRole(ApplicationRolesNames.AdminRole)))
            {
                return this.RedirectToAction("Index", "Videos");
            }

            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var user = await userManager.GetUserAsync(User);
            await videosService.CreateAsync(
                inputModel.YouTubeId,
                inputModel.Title,
                inputModel.Description,
                inputModel.PremiredOn.ConvertToUtc(),
                inputModel.IsVisible,
                user);

            return this.RedirectToAction("Watch", "Videos", new { Id = "0" });
        }
    }
}
