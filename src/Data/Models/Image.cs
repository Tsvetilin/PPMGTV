using Common.Enums;
using Data.Contracts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Image : BaseDeletableModel<string>
    {
        public Image()
        {
            Id = Guid.NewGuid().ToString();
        }

        public ImageType Category { get; set; }

        public string Description { get; set; }

        public string Note { get; set; }

        [Required]
        public string Url { get; set; }

        public string GalleryId { get; set; }
    }
}
