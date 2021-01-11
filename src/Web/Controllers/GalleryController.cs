using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class GalleryController : Controller
    {

        public GalleryController()
        {

        }

        public IActionResult Index()
        {
            return this.View();
        }
    }
}
