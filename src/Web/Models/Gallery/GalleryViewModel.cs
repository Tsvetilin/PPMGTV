using Services.Contracts.Mapping;
using System.Collections.Generic;

namespace Web.Models.Gallery
{
    public class GalleryViewModel : IMapFrom<Data.Models.Gallery>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public List<string> ImagesUrls { get; set; }

        public string PreDescription { get; set; }

        public string Description { get; set; }

    }
}
