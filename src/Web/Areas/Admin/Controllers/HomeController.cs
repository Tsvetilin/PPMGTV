using Common.Constants;
using Common.Helpers;
using Microsoft.AspNetCore.Mvc;
using Services.CronJobs;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [EditorAuthorization]
    public class HomeController : Controller
    {
        [Route("Admin/[controller]/[action]")]

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult UpdateVideos()
        {
            JobManager.TriggerVideoUpdaterJob();
            this.TempData[DataParams.UpdatedVideosSuccessTempDataParam] = true;
            return this.RedirectToAction(nameof(Index));
        }
    }
}
