using Data.Models;
using Services.Contracts.Mapping;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.Videos
{
    public class VideoInputModel : IMapTo<Video>, IMapFrom<Video>
    {
        [Required]
        [MaxLength(20)]
        [DisplayName("YouTube Video ID")]
        public string YouTubeId { get; set; }

        [Required]
        [DisplayName("Заглавие")]
        public string Title { get; set; }

        [Required]
        [DisplayName("Описание")]
        public string Description { get; set; }

        [Required]
        [DisplayName("Видимо")]
        public bool IsVisible { get; set; }

        [Required]
        [DisplayName("Премиера")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime PremiredOn { get; set; }
    }
}
