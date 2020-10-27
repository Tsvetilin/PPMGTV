﻿using Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.CronJobs;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ApplicationRolesNames.AdminRole)]
    public class HomeController : Controller
    {
        private const string UpdatedVideos = "UpdatedVideos";


        [Route("Admin/[controller]/[action]")]

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult UpdateVideos()
        {
            JobManager.TriggerVideoUpdaterJob();
            this.TempData[UpdatedVideos] = true;
            return this.RedirectToAction("Index", "Home");
        }
    }
}
