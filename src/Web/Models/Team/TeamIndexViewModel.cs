using System.Collections.Generic;

namespace Web.Models.Team
{
    public class TeamIndexViewModel
    {
        public IEnumerable<TeamMemberViewModel> ActiveMembers { get; set; }
        public IEnumerable<TeamMemberViewModel> InactiveMembers { get; set; }

    }
}
