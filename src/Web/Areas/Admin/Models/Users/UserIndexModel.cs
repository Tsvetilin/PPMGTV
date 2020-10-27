using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Areas.Admin.Models.Users
{
    public class UserIndexModel
    {
        public IEnumerable<UserPermissionsModel> Users { get; set; }
    }
}
