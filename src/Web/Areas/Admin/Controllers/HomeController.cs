using Common.Constants;
using Common.Helpers;
using Microsoft.AspNetCore.Mvc;
using Services.CronJobs;
using Web.Areas.Admin.Models.Letters;

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

        public IActionResult NewsLetterSend()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult NewsLetterSend(LetterInputModel inputModel)
        {
            JobManager.StartSubscriptionEmailJob(inputModel.Text);
            return this.RedirectToAction(nameof(Index));
        }
    }
}
