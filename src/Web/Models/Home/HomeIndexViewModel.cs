using Data.Models;
using Services.Contracts.Mapping;
using Web.Models.Gallery;
using Web.Models.Home;

namespace Web.Models.Index
{
    public class HomeIndexViewModel : IMapFrom<Video>
    {
        public SettingsViewModel Settings { get; set; }
        public string VideoUrl { get; set; }
        public string Title { get; set; }
        public GalleryViewModel Gallery { get; set; }
    }
}
