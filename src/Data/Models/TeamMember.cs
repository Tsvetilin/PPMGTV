using Data.Contracts.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class TeamMember : BaseDeletableModel<string>
    {
        public TeamMember()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public virtual ApplicationUser User { get; set; }

        [Required]
        public string Descrtiption { get; set; }

        [Required]
        public string PhotoUrl { get; set; }
    }
}
