using System.Linq;
using System.Threading.Tasks;
using Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts.Data;
using Web.Areas.Admin.Models.Users;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ApplicationRolesNames.AdminRole)]
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public async Task<IActionResult> Permissions()
        {
            var users = await usersService.GetUsersAsync<UserPermissionsModel>();
            var viewModel = new UserIndexModel
            {
                Users = users.ToList()
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> ManagePermissions(string id)
        {
            var user = await usersService.GetUserByIdAsync<UserPermissionsModel>(id);
            if (user == null)
            {
                return this.NotFound();
            }

            return this.View(user);
        }

        [HttpPost]
        public async Task<IActionResult> ManagePermissions([Bind("IsAdmin", "IsEditor")] UserPermissionsModel inputModel)
        {
            return this.View();
        }
    }
}
