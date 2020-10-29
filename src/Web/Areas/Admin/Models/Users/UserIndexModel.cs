using System.Collections.Generic;

namespace Web.Areas.Admin.Models.Users
{
    public class UserIndexModel
    {
        public IEnumerable<UserPermissionsModel> Users { get; set; }
    }
}
