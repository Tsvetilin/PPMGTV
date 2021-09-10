using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class CastController : Controller
    {
        public IActionResult Index()
        {
            return this.Redirect("https://applications.ppmgtv.com");
        }

        public IActionResult Form()
        {
            return this.Redirect("https://applications.ppmgtv.com/form.html");
        }
    }
}
