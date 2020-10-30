using Common.Helpers;
using Data.Contracts.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Video : BaseDeletableModel<string>
    {
        public Video()
        {
            this.Id = Guid.NewGuid().GenerateShortId();
        }

        //https://img.youtube.com/vi/<videoId>/hqdefault.jpg

        [Required]
        [MaxLength(20)]
        public string YouTubeId { get; set; }

        [Required]
        public string VideoUrl { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ThumbnailUrl { get; set; }

        public virtual ApplicationUser AddedBy { get; set; }

        public bool IsVisible { get; set; }

        [Required]
        public DateTime PremiredOn { get; set; }
    }
}
