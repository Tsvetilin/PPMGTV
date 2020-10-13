using Data.Contracts.Models;
using System;

namespace Data.Models
{
    public class Video : BaseDeletableModel<string>
    {
        public Video()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string VideoUrl { get; set; }
        public ApplicationUser AddedBy { get; set; }
        public bool IsVisible { get; set; }
    }
}
