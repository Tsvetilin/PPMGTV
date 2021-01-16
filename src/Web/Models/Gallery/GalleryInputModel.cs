using System.Collections.Generic;

namespace Web.Models.Gallery
{
    public class GalleryInputModel
    {
        public string Title { get; set; }

        public List<string> ImagesUrls { get; set; }

        public string ImagesNote { get; set; }

        public string ImagesDescription { get; set; }

        public string PreDescription { get; set; }

        public string Description { get; set; }
    }
}
