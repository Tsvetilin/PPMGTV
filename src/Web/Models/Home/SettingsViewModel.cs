using Data.Models;
using Services.Contracts.Mapping;

namespace Web.Models.Home
{
    public class SettingsViewModel : IMapFrom<Setting>
    {
        public bool IsHomePageNewsVisible { get; set; }

        public bool IsHomePageLastGalleryVisible { get; set; }

        public string HomePageNewsSectionTitle { get; set; }

        public string HomePageNewsContent { get; set; }

        public bool IsHomePageGalleryPreTextVisible { get; set; }

        public string HomePageNewsPreContent { get; set; }

        public bool IsHomePageGalleryPostTextVisible { get; set; }
    }
}
