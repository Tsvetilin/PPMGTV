using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts.Data;
using Services.Data;
using Web.Models;
using Web.Models.Index;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVideosService videosService;

        public HomeController(IVideosService videosService)
        {
            this.videosService = videosService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = await videosService.GetLatestVideo<HomeIndexViewModel>();
            return this.View(viewModel);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel 
                { 
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier 
                }
            );
        }

        public IActionResult StatusError(int Id)
        {
            return this.View(Id);
        }
    }
}
