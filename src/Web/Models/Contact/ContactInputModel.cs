using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.Contact
{
    public class ContactInputModel 
    {
        [Required]
        [DisplayName("Описание")]
        public string Description { get; set; }

        [Required]
        [MaxLength(50)]
        [DisplayName("Подател (име)")]
        public string SenderName { get; set; }

        [Required]
        [MaxLength(50)]
        [DisplayName("Имейл за връзка")]
        public string SenderEmail { get; set; }

        [DisplayName("Други контакти за връзка (социална мрежа, телефонен номер и др.)")]
        public string OtherContactInfo { get; set; }
    }
}
