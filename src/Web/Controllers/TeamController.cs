using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class TeamController : Controller
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
