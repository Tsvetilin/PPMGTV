using Data.Contracts.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Gallery : BaseDeletableModel<string>
    {
        public Gallery()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string Title { get; set; }

        [Required]
        public virtual IEnumerable<Image> Images { get; set; }

        public string PreDescription { get; set; }

        public string Description { get; set; }
    }
}
