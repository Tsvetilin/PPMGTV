using Data.Models;
using Services.Contracts.Mapping;

namespace Web.Models.Team
{
    public class TeamMemberViewModel : IMapFrom<TeamMember>
    {
        public string UserUserName { get; set; }

        public string UserId { get; set; }

        public string Descrtiption { get; set; }

        public string PhotoUrl { get; set; }

        public bool IsActiveMember { get; set; }
    }
}
