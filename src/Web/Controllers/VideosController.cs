using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts.Data;
using Web.Models.Videos;

namespace Web.Controllers
{
    public class VideosController : Controller
    {
        private const int videosOnPage = 10;

        private readonly IVideosService videosService;

        public VideosController(IVideosService videosService)
        {
            this.videosService = videosService;
        }

        public async Task<IActionResult> Index(int id = 1)
        {
            var pageCount = (int)Math.Ceiling((double)videosService.CountAllFilms() / videosOnPage);
            if (id > pageCount || id < 1)
            {
                id = 1;
            }

            var videos = await videosService.GetVideosOnPage<VideoViewModel>(id,videosOnPage);
            var viewModel = new VideosIndexViewModel
            {
                Videos = videos.ToList(),
                CurrentPage = 1,
                PagesCount = pageCount
            };

            return this.View(viewModel);
        }
    }
}
