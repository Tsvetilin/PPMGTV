using Data.Contracts.Models;
using System;

namespace Data.Models
{
    public class ContactLetter : BaseDeletableModel<string>
    {
        public ContactLetter()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Description { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string OtherContactInfo { get; set; }
        public ApplicationUser User {get;set;}
        public bool IsPinned { get; set; }
        public bool IsViewed { get; set; }
    }
}
