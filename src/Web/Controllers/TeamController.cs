using Common.Constants;
using Common.Helpers;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Services.Contracts.Data;
using Services.Contracts.External;
using System;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Web.Models.Team;

namespace Web.Controllers
{
    public class TeamController : Controller
    {
        private readonly ITeamService teamService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICloudinary cloudinary;
        private readonly IUsersService usersService;

        public TeamController(ITeamService teamService, UserManager<ApplicationUser> userManager, ICloudinary cloudinary, IUsersService usersService)
        {
            this.teamService = teamService;
            this.userManager = userManager;
            this.cloudinary = cloudinary;
            this.usersService = usersService;
        }

        public async Task<IActionResult> Index()
        {
            var members = await teamService.GetAllAsync<TeamMemberViewModel>();
            var viewModel = new TeamIndexViewModel
            {
                ActiveMembers = members.Where(x => x.IsActiveMember).ToList(),
                InactiveMembers = members.Where(x => !x.IsActiveMember).ToList(),
            };

            return this.View(viewModel);
        }

        [Authorize(Roles = ApplicationRolesNames.AdminRole)]
        public IActionResult Create()
        {
            ViewData[DataParams.AuthenticationCookieViewDataParam] = this.HttpContext.Request.Cookies[DetailsConstants.AuthenticationCookieHeaderName];

            return this.View();
        }

        [HttpPost]
        [Authorize(Roles =ApplicationRolesNames.AdminRole)]
        public async Task<IActionResult> Create(TeamMemberInputModel inputModel)
        {
            var userById = await userManager.FindByNameAsync(inputModel.UserUserName);
            var userByUserName = await userManager.FindByIdAsync(inputModel.UserId);

            if (!(userById != null && userByUserName != null && userByUserName.Id==inputModel.UserId))
            {
                ModelState.AddModelError("Потребител", "Посочените потребителски идентификационен номер и потребителско име не съвпадат.");
            }
            else if (Guid.TryParse(inputModel.PhotoUrl, out _))
            {
                if (inputModel.PhotoUpload != null)
                {
                    var fileName = $"_{inputModel.UserUserName}_MemberPhoto";

                    IFormFile file = inputModel.PhotoUpload;

                    using var stream = new MemoryStream();
                    await file.CopyToAsync(stream);
                    var posterUrl = await cloudinary.UploadImageAsync(stream, fileName);
                    inputModel.PhotoUrl = posterUrl;
                }
                else
                {
                    ModelState.AddModelError("Снимка", "Снимката е задължителна! Въведете връзка към снимка или качете такава.");
                }
            }

            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await teamService.CreateAsync(userById, inputModel.IsActiveMember, inputModel.PhotoUrl, inputModel.Descrtiption);

            return RedirectToAction(nameof(Index));
        }
    }
}
