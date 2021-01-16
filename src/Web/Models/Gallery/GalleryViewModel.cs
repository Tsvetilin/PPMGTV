using AutoMapper;
using Services.Contracts.Mapping;
using System.Collections.Generic;
using System.Linq;

namespace Web.Models.Gallery
{
    public class GalleryViewModel : IMapFrom<Data.Models.Gallery>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public List<string> ImagesUrls { get; set; }

        public string PreDescription { get; set; }

        public string Description { get; set; }

        public string Slug { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Data.Models.Gallery, GalleryViewModel>().
                ForMember(
                    member => member.ImagesUrls, 
                    projection => projection.MapFrom(
                                                    field => field.
                                                             Images.
                                                             OrderBy(p=>p.CreatedOn).
                                                             Select(p => p.Url)
                                                    )
                    );
        }
    }
}
