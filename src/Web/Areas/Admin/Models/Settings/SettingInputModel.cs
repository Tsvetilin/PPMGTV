using Data.Models;
using Services.Contracts.Mapping;
using System.ComponentModel;

namespace Web.Areas.Admin.Models.Settings 
{
    public class SettingInputModel : IMapFrom<Setting>
    {
        public string Id { get; set; }

        [DisplayName("Показване на допълнителното съдържание")]
        public bool IsHomePageNewsVisible { get; set; }

        [DisplayName("Показване на най-новата галерия")]
        public bool IsHomePageLastGalleryVisible { get; set; }

        [DisplayName("Заглавие на секцията")]
        public string HomePageNewsSectionTitle { get; set; }

        [DisplayName("Съдържание на секцията")]
        public string HomePageNewsContent { get; set; }
    }
}
