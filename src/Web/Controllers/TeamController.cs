using Common.Constants;
using Common.Helpers;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts.Data;
using Services.Contracts.External;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.Team;

namespace Web.Controllers
{
    public class TeamController : Controller
    {
        private readonly ITeamMemberService teamMemberService;
        private readonly ITeamService teamService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICloudinary cloudinary;
        private readonly IUsersService usersService;

        public TeamController(
            ITeamMemberService teamMemberService,
            ITeamService teamService,
            UserManager<ApplicationUser> userManager,
            ICloudinary cloudinary,
            IUsersService usersService)
        {
            this.teamMemberService = teamMemberService;
            this.teamService = teamService;
            this.userManager = userManager;
            this.cloudinary = cloudinary;
            this.usersService = usersService;
        }

        public async Task<IActionResult> Index()
        {
            var teams = await teamService.GetAllAsync<TeamViewModel>();
            var viewModel = new TeamIndexViewModel
            {
                Teams = teams.ToList()
            };

            return this.View(viewModel);
        }

        [EditorAuthorization]
        public IActionResult Create()
        {
            ViewData[DataParams.AuthenticationCookieViewDataParam] = this.HttpContext.Request.Cookies[DetailsConstants.AuthenticationCookieHeaderName];

            return this.View();
        }

        [HttpPost]
        [EditorAuthorization]
        public async Task<IActionResult> Create(TeamInputModel inputModel)
        {
            if (Guid.TryParse(inputModel.PhotoUrl, out _))
            {
                if (inputModel.PhotoUpload != null)
                {
                    var fileName = $"_{inputModel.TeamTitle}_TeamPhoto";

                    IFormFile file = inputModel.PhotoUpload;

                    using var stream = new MemoryStream();
                    await file.CopyToAsync(stream);
                    var photoUrl = await cloudinary.UploadImageAsync(stream, fileName);
                    if (photoUrl.StartsWith("Error"))
                    {
                        ModelState.AddModelError("Снимка", $"Възникна грешка при качване на снимката: {photoUrl}. Ако не може да я разрешите, свържете се с администратор.");
                    }
                    else
                    {
                        inputModel.PhotoUrl = photoUrl;
                    }
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

            await teamService.CreateAsync(
                inputModel.TeamTitle,
                inputModel.TeamYears,
                inputModel.PhotoUrl,
                inputModel.PreDescription,
                inputModel.Descrtiption);

            return RedirectToAction(nameof(Index));
        }

        [EditorAuthorization]
        public async Task<IActionResult> Edit(string id)
        {
            var inputModel = await teamService.GetTeamByIdAsync<TeamInputModel>(id);

            if (inputModel == null)
            {
                return this.NotFound();
            }

            TempData["VideoId"] = id;
            return this.View(inputModel);
        }


        [EditorAuthorization]
        [HttpPost]
        public async Task<IActionResult> Edit(TeamInputModel inputModel, string id)
        {
            if (!ModelState.IsValid)
            {
                TempData["VideoId"] = id;
                return this.View(inputModel);
            }

            var team = await teamService.GetTeamByIdAsync<TeamInputModel>(id);
            if (team == null)
            {
                return this.NotFound();
            }

            await teamService.UpdateAsync(
                id,
                inputModel.TeamTitle,
                inputModel.TeamYears,
                inputModel.PhotoUrl,
                inputModel.PreDescription,
                inputModel.Descrtiption);

            return this.RedirectToAction(nameof(Index));
        }

        [EditorAuthorization]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (!await teamService.DeleteAsync(id))
            {
                return this.NotFound();
            }

            return this.RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = ApplicationRolesNames.AdminRole)]
        public IActionResult AddMember()
        {
            ViewData[DataParams.AuthenticationCookieViewDataParam] = this.HttpContext.Request.Cookies[DetailsConstants.AuthenticationCookieHeaderName];

            return this.View();
        }

        [HttpPost]
        [Authorize(Roles = ApplicationRolesNames.AdminRole)]
        public async Task<IActionResult> AddMember(TeamMemberInputModel inputModel)
        {
            var userById = await userManager.FindByNameAsync(inputModel.UserUserName);
            var userByUserName = await userManager.FindByIdAsync(inputModel.UserId);

            if (!(userById != null && userByUserName != null && userByUserName.Id == inputModel.UserId))
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
                    var photoUrl = await cloudinary.UploadImageAsync(stream, fileName);
                    if (photoUrl.StartsWith("Error"))
                    {
                        ModelState.AddModelError("Снимка", $"Възникна грешка при качване на снимката: {photoUrl}. Ако не може да я разрешите, свържете се с администратор.");
                    }
                    else
                    {
                        inputModel.PhotoUrl = photoUrl;
                    }
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

            await teamMemberService.CreateAsync(userById, inputModel.IsActiveMember, inputModel.PhotoUrl, inputModel.Descrtiption);

            return RedirectToAction(nameof(Index));
        }
    }
}
