using Data.Models;
using Microsoft.AspNetCore.Cors;
using Services.Contracts.Mapping;
using System;
using System.ComponentModel;

namespace Web.Models.Contact
{
    public class ContactViewModel : IMapFrom<ContactLetter>
    {
        public string Id { get; set; }

        public string About { get; set; }

        public string Description { get; set; }

        public string SenderName { get; set; }

        public string SenderEmail { get; set; }

        public string OtherContactInfo { get; set; }

        public string UserUserName { get; set; }

        [DisplayName("Закачено")]
        public bool IsPinned { get; set; }

        [DisplayName("Прегледано")]
        public bool IsViewed { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
