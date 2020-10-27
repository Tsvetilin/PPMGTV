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
using Web.Models.Videos;

namespace Web.Controllers
{
    public class VideosController : Controller
    {
        private readonly IVideosService videosService;
        private readonly UserManager<ApplicationUser> userManager;

        public VideosController(IVideosService videosService, UserManager<ApplicationUser> userManager)
        {
            this.videosService = videosService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index(int id = 1)
        {
            var pageCount = (int)Math.Ceiling((double)videosService.CountAllFilms() / PaginationItemsCount.VideosOnPage);
            if (id > pageCount || id < 1)
            {
                id = 1;
            }

            var videos = await videosService.GetVideosOnPageAsync<VideoViewModel>(id, PaginationItemsCount.VideosOnPage);
            var viewModel = new VideosIndexViewModel
            {
                Videos = videos.ToList(),
                CurrentPage = id,
                PagesCount = pageCount
            };

            return this.View(viewModel);
        }

        [Authorize]
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
        public async Task<IActionResult> Edit(string id)
        {
            if (!(User.IsInRole(ApplicationRolesNames.EditorRole) || User.IsInRole(ApplicationRolesNames.AdminRole)))
            {
                return this.RedirectToAction("Index", "Videos");
            }

            var inputModel = await videosService.GetVideoByIdAsync<VideoInputModel>(id);

            if (inputModel == null)
            {
                return this.NotFound();
            }

            return this.View(inputModel);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(VideoInputModel inputModel, string id)
        {
            if (!(User.IsInRole(ApplicationRolesNames.EditorRole) || User.IsInRole(ApplicationRolesNames.AdminRole)))
            {
                return this.RedirectToAction("Index", "Videos");
            }

            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var video = await videosService.GetVideoByIdAsync<VideoInputModel>(id);
            if (video == null)
            {
                return this.NotFound();
            }

            var user = await userManager.GetUserAsync(User);
            await videosService.UpdateAsync(
                id,
                inputModel.YouTubeId,
                inputModel.Title,
                inputModel.Description,
                inputModel.PremiredOn.ConvertToUtc(),
                inputModel.IsVisible,
                user);

            return this.RedirectToAction("Watch", "Videos", new { id });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (!(User.IsInRole(ApplicationRolesNames.EditorRole) || User.IsInRole(ApplicationRolesNames.AdminRole)))
            {
                return this.RedirectToAction("Index", "Videos");
            }

            if (!await videosService.DeleteAsync(id))
            {
                return this.NotFound();
            }

            return this.RedirectToAction("Index", "Videos");
        }

    }
}
