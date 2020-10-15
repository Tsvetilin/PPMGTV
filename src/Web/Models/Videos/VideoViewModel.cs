using Data.Models;
using Services.Contracts.Mapping;
using System;

namespace Web.Models.Videos
{
    public class VideoViewModel : IMapFrom<Video>
    {
        public string Id { get; set; }
        public string VideoUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ThumbnailUrl { get; set; }
        public DateTime PremiredOn { get; set; }
    }
}
