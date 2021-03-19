using Data.Contracts.Models;
using System;

namespace Data.Models
{
    public class Setting : BaseDeletableModel<string>
    {
        public Setting()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public bool IsHomePageNewsVisible { get; set; }

        public bool IsHomePageLastGalleryVisible { get; set; }

        public string HomePageNewsSectionTitle { get; set; }

        public string HomePageNewsContent { get; set; }
    }
}
