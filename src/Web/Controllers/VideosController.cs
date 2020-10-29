﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Common.Constants;
using Common.Helpers;
using Data.Models;
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

        [EditorAuthorization]
        public IActionResult Create()
        {
            return this.View();
        }


        [EditorAuthorization]
        [HttpPost]
        public async Task<IActionResult> Create(VideoInputModel inputModel)
        {
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

            return this.RedirectToAction(nameof(Watch), "Videos", new { video.Id });
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

        [EditorAuthorization]
        public async Task<IActionResult> Edit(string id)
        {
            var inputModel = await videosService.GetVideoByIdAsync<VideoInputModel>(id);

            if (inputModel == null)
            {
                return this.NotFound();
            }

            return this.View(inputModel);
        }


        [EditorAuthorization]
        [HttpPost]
        public async Task<IActionResult> Edit(VideoInputModel inputModel, string id)
        {
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

            return this.RedirectToAction(nameof(Watch), "Videos", new { id });
        }

        [EditorAuthorization]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (!await videosService.DeleteAsync(id))
            {
                return this.NotFound();
            }

            return this.RedirectToAction(nameof(Index));
        }

    }
}
