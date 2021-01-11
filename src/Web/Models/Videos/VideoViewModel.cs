using Common.Helpers;
using Data.Models;
using Services.Contracts.Mapping;
using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.Videos
{
    public class VideoViewModel : IMapFrom<Video>
    {
        public string Id { get; set; }
        public string VideoUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ThumbnailUrl { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime PremiredOn { get; set; }

        public string Slug
        {
            get
            {
                return SlugGeneratorExtensions.GenerateSlug(this.Title);
            }
        }
    }
}
