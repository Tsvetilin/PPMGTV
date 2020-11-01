using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.Team
{
    public class TeamMemberInputModel
    {
        [Required]
        [DisplayName("Потребителски идентификационен номер")]
        public string UserId { get; set; }

        [Required]
        [DisplayName("Потребителско име")]
        public string UserUserName { get; set; }

        [Required]
        [DisplayName("Описание")]
        public string Descrtiption { get; set; }

        [Required]
        [DisplayName("Линк към снимка")] 
        public string PhotoUrl { get; set; }

        [DisplayName("Активен член на екипа")]
        public bool IsActiveMember { get; set; }

        [DisplayName("Качи снимка")]
        public IFormFile PhotoUpload { get; set; }
    }
}
