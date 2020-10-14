using System.Collections.Generic;

namespace Web.Models.Videos
{
    public class VideosIndexViewModel
    {
        public IEnumerable<VideoViewModel> Videos { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }
    }
}
