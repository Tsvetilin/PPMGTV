using Microsoft.AspNetCore.Http;
using Services.Contracts.Mapping;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.Team
{
    public class TeamInputModel :IMapFrom<Data.Models.Team>
    {
        [Required]
        [DisplayName("Заглавие на екипа")]
        public string TeamTitle { get; set; }

        [Required]
        [DisplayName("Години на активност")]
        public string TeamYears { get; set; }

        [Required]
        [DisplayName("Кратък текст / предописание")]
        public string PreDescription { get; set; }

        [Required]
        [DisplayName("Описание")]
        [DataType(DataType.Html)]
        public string Descrtiption { get; set; }

        [Required]
        [DisplayName("Линк към снимка")]
        public string PhotoUrl { get; set; }

        [DisplayName("Качи снимка")]
        public IFormFile PhotoUpload { get; set; }
    }
}
