using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts.Data;
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
            var viewModel = await videosService.GetLatestVideoAsync<HomeIndexViewModel>();
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

        public IActionResult SetConsentCookie(string id)
        {
            HttpContext.Response.Cookies.Append(".AspNet.Consent", "yes", new CookieOptions
            {
                Secure = true,
                SameSite = SameSiteMode.None,
                HttpOnly = true,
                Path = "/",
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                IsEssential = true
            });

            string controller = null, action = null, args = null;

            if (id != null)
            {
                var path = id.Split('/', StringSplitOptions.RemoveEmptyEntries).ToArray();

                if (path.Length > 0)
                {
                    controller = path[0];
                    if (path.Length > 1)
                    {
                        action = path[1];
                        if (path.Length > 2)
                        {
                            args = path[2];
                        }
                    }
                }
            }

            return RedirectToAction(action ?? "Index", controller ?? "Home", new { Id = args });
            //".AspNet.Consent=yes; expires={datetime}; path=/; secure; samesite=none; httponly"
        }
    }
}
