using Common.Extensions;
using Common.Helpers;
using Data.Models;
using Services.Contracts.Mapping;
using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.Home
{
    public class RecentVideoModel : IMapFrom<Video>
    {
        private const int ElipsisLength = 250;
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ThumbnailUrl { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime PremiredOn { get; set; }

        public string Slug
        {
            get
            {
                return this.Title.GenerateSlug();
            }

        }

        public string ShortDescription
        {
            get
            {
                return this.Description.AddElipsisAtLength(ElipsisLength).SanitizeHtml();
            }
        }
    }
}
