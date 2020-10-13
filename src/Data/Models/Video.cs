using Data.Contracts.Models;
using System;

namespace Data.Models
{
    public class Video : BaseDeletableModel<string>
    {
        public Video()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        //https://img.youtube.com/vi/<videoId>/hqdefault.jpg
        //
        public string YouTubeId { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string ThumbnailUrl { get; set; }
        public ApplicationUser AddedBy { get; set; }
        public bool IsVisible { get; set; }
    }
}
