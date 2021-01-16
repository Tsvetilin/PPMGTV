using AutoMapper;
using Services.Contracts.Mapping;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Web.Models.Gallery
{
    public class GalleryInputModel : IMapFrom<Data.Models.Gallery>, IHaveCustomMappings
    {
        [Required]
        [DisplayName("Заглавие")]
        public string Title { get; set; }

        [Required]
        [DisplayName("Линкове към снимките")]
        public List<string> ImagesUrls { get; set; }

        [DisplayName("Бележка за снимките")]
        public string ImagesNote { get; set; }

        [DisplayName("Описание за снимките")]
        public string ImagesDescription { get; set; }

        [DisplayName("Предописание")]
        [Required]
        public string PreDescription { get; set; }

        [DataType(DataType.Html)]
        [DisplayName("Описание")]
        [Required]
        public string Description { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Data.Models.Gallery, GalleryInputModel>().
                ForMember(
                    member => member.ImagesUrls,
                    projection => projection.MapFrom(
                                                    field => field.Images.Select(p => p.Url)
                                                    )
                    );
        }
    }
}
