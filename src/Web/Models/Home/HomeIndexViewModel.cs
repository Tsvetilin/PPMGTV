using Data.Models;
using Services.Contracts.Mapping;

namespace Web.Models.Index
{
    public class HomeIndexViewModel : IMapFrom<Video>
    {
        public string VideoUrl { get; set; }
    }
}
