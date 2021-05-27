using Data.Models;
using Services.Contracts.Mapping;

namespace Web.Models.Index
{
    public class HomeLatestViewModel : HomeIndexViewModel, IMapFrom<Video>
    {
        public string ThumbnailUrl { get; set; }
    }
}
