using Common.Enums;
using Services.Contracts.Mapping;

namespace Web.Models.Image
{
    public class ImageViewModel : IMapFrom<Data.Models.Image>
    {
       public string Id { get; set; }

        public ImageType Category { get; set; }

        public string Description { get; set; }

        public string Note { get; set; }

        public string Url { get; set; }
    }
}
