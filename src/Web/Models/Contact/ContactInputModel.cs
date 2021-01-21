using Common.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.Contact
{
    public class ContactInputModel 
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Моля въведете тема на съобщението")]
        [StringLength(100, ErrorMessage = "Съобщението трябва да е поне {2} символа и не повече от {1} символа.", MinimumLength = 3)]
        [DisplayName("Относно")]
        public string About { get; set; }

        [DisplayName("Описание")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Моля въведете съдържание на съобщението")]
        [StringLength(100000, ErrorMessage = "Съобщението трябва да е поне {2} символа.", MinimumLength = 20)]
        public string Description { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Моля въведете вашето име")]
        [StringLength(50, ErrorMessage = "Дължината на името трябва да е поне {2} символа и не повече от {1} символа.", MinimumLength = 2)]
        [DisplayName("Подател (име)")]
        public string SenderName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Моля въведете вашето име")]
        [MaxLength(50)]
        [EmailAddress(ErrorMessage ="Невалиден имейл адрес.")]
        [DisplayName("Имейл за връзка")]
        public string SenderEmail { get; set; }

        [DisplayName("Други контакти за връзка (социална мрежа, телефонен номер и др.)")]
        public string OtherContactInfo { get; set; }

        [GoogleReCaptchaValidation]
        public string RecaptchaValue { get; set; }
    }
}
