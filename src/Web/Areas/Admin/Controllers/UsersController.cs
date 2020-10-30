using System;
using System.Linq;
using System.Threading.Tasks;
using Common.Constants;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Services.Contracts.Data;
using Web.Areas.Admin.Models.Users;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ApplicationRolesNames.AdminRole)]
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersController(IUsersService usersService, UserManager<ApplicationUser> userManager)
        {
            this.usersService = usersService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Permissions()
        {
            var users = await usersService.GetUsersAsync<UserPermissionsModel>();
            foreach (var user in users)
            {
                var rolesNames = await userManager.GetRolesAsync(await userManager.FindByIdAsync(user.Id));
                user.IsEditor = rolesNames?.Any(x => x.Equals(ApplicationRolesNames.EditorRole)) ?? false;
                user.IsAdmin = rolesNames?.Any(x => x.Equals(ApplicationRolesNames.AdminRole)) ?? false;
            }
            var viewModel = new UserIndexModel
            {
                Users = users.ToList()
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> ManagePermissions(string id)
        {
            var user = await usersService.GetUserByIdAsync<UserPermissionsModel>(id);
            var rolesNames = await userManager.GetRolesAsync(await userManager.FindByIdAsync(user.Id));
            user.IsEditor = rolesNames?.Any(x => x.Equals(ApplicationRolesNames.EditorRole)) ?? false;
            user.IsAdmin = rolesNames?.Any(x => x.Equals(ApplicationRolesNames.AdminRole)) ?? false;

            if (user == null)
            {
                return this.NotFound();
            }

            return this.View(user);
        }

        [HttpPost]
        public async Task<IActionResult> ManagePermissions([Bind("IsAdmin", "IsEditor", "Id")] UserPermissionsModel inputModel)
        {
            var user = await  userManager.FindByIdAsync(inputModel.Id);
            if( user == null)
            {
                return this.NotFound();
            }

            if(inputModel.IsAdmin)
            {
                await userManager.AddToRoleAsync(user, ApplicationRolesNames.AdminRole);
            }
            else
            {
                await userManager.RemoveFromRoleAsync(user, ApplicationRolesNames.AdminRole);
            }

            if (inputModel.IsEditor)
            {
                await userManager.AddToRoleAsync(user, ApplicationRolesNames.EditorRole);
            }
            else
            {
                await userManager.RemoveFromRoleAsync(user, ApplicationRolesNames.EditorRole);
            }

            return this.RedirectToAction(nameof(ManagePermissions), new { id = inputModel.Id });
        }

        public async Task<IActionResult> Lock(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return this.NotFound();
            }

            await userManager.SetLockoutEndDateAsync(user, DateTime.UtcNow.AddYears(DetailsConstants.YearsToAddToInfinity));

            return this.RedirectToAction(nameof(ManagePermissions), new { id });
        }

        public async Task<IActionResult> Unlock(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return this.NotFound();
            }

            await userManager.SetLockoutEndDateAsync(user, DateTime.UtcNow);

            return this.RedirectToAction(nameof(ManagePermissions), new { id });
        }
    }
}
