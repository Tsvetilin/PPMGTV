using Common.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;

namespace Web.Models.Image
{
    public class ImageInputModel
    {
        [DisplayName("Качи снимка")]
        public IFormFile PhotoUpload { get; set; }

        /*
        [DisplayName("Категория снимка")]
        public ImageType Category { get; set; }

        [DisplayName("Опиание")]
        public string Description { get; set; }

        [DisplayName("Бележка")]
        public string Note { get; set; }
        */
    }
}
