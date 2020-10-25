using Common.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.Contact
{
    public class ContactInputModel 
    {
        [Required]
        [MaxLength(100)]
        [DisplayName("Относно")]
        public string About { get; set; }

        [Required]
        [DisplayName("Описание")]
        public string Description { get; set; }

        [Required]
        [MaxLength(50)]
        [DisplayName("Подател (име)")]
        public string SenderName { get; set; }

        [Required]
        [MaxLength(50)]
        [EmailAddress]
        [DisplayName("Имейл за връзка")]
        public string SenderEmail { get; set; }

        [DisplayName("Други контакти за връзка (социална мрежа, телефонен номер и др.)")]
        public string OtherContactInfo { get; set; }

        [GoogleReCaptchaValidation]
        public string RecaptchaValue { get; set; }
    }
}
