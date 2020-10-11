using Data.Contracts.Models;
using System;

namespace Data.Models
{
    public class TestModel : BaseDeletableModel<string>
    {
        public TestModel()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Name { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
