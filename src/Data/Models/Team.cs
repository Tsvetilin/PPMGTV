using Data.Contracts.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Team : BaseDeletableModel<string>
    {
        public Team()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string TeamTitle { get; set; }

        [Required]
        public string TeamYears { get; set; }

        public string PreDescription { get; set; }

        [Required]
        public string Descrtiption { get; set; }

        [Required]
        public string PhotoUrl { get; set; }
    }
}
