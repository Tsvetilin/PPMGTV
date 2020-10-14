using Data.Contracts.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class ContactLetter : BaseDeletableModel<string>
    {
        public ContactLetter()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string Description { get; set; }

        [Required]
        [MaxLength(50)]
        public string SenderName { get; set; }

        [Required]
        [MaxLength(50)]
        public string SenderEmail { get; set; }

        public string OtherContactInfo { get; set; }

        public virtual ApplicationUser User {get;set;}

        public bool IsPinned { get; set; }

        public bool IsViewed { get; set; }
    }
}
