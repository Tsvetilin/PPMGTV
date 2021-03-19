using Common.Constants;
using Common.Helpers;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts.Data;
using Services.CronJobs;
using System.Threading.Tasks;
using Web.Areas.Admin.Models.Letters;
using Web.Areas.Admin.Models.Settings;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [EditorAuthorization]
    public class HomeController : Controller
    {
        private readonly ISettingsService settingsService;

        public HomeController(ISettingsService settingsService)
        {
            this.settingsService = settingsService;
        }

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
            this.TempData[DataParams.SendNewsLetterSuccessTempDataParam] = true;
            return this.RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UpdateHomePage()
        {
            var inputModel = await this.settingsService.GetSettingAsync<SettingInputModel>();
            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateHomePage(SettingInputModel inputModel)
        {
            await this.settingsService.UpdateAsync(
                inputModel.Id,
                inputModel.IsHomePageNewsVisible,
                inputModel.IsHomePageLastGalleryVisible,
                inputModel.HomePageNewsSectionTitle,
                inputModel.HomePageNewsContent
                );

            return this.RedirectToAction(nameof(Index), "Home", new { area = "" });
        }
    }
}
