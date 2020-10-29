using Common.Constants;
using Common.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.Videos;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [EditorAuthorization]
    public class VideosController : Controller
    {
        private readonly IVideosService videosService;

        public VideosController(IVideosService videosService)
        {
            this.videosService = videosService;
        }

        public async Task<IActionResult> Unlisted(int id = 1)
        {
            var pageCount = (int)Math.Ceiling((double)videosService.CountAllFilms() / PaginationItemsCount.VideosOnPage);
            if (id > pageCount || id < 1)
            {
                id = 1;
            }

            var videos = await videosService.GetUnlistedVideosOnPageAsync<VideoViewModel>(id, PaginationItemsCount.VideosOnPage);
            var viewModel = new VideosIndexViewModel
            {
                Videos = videos.ToList(),
                CurrentPage = id,
                PagesCount = pageCount
            };

            return this.View(viewModel);
        }
    }
}
