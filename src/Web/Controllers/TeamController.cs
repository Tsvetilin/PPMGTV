using Microsoft.AspNetCore.Mvc;
using Services.Contracts.Data;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.Team;

namespace Web.Controllers
{
    public class TeamController : Controller
    {
        private readonly ITeamService teamService;

        public TeamController(ITeamService teamService)
        {
            this.teamService = teamService;
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
    }
}
