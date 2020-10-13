using Data.Contracts.Models;
using System;

namespace Data.Models
{
    public class TeamMember : BaseDeletableModel<string>
    {
        public TeamMember()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public ApplicationUser User { get; set; }
        public string Descrtiption { get; set; }
        public string PhotoUrl { get; set; }
    }
}
