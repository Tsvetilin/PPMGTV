using Common.Constants;
using Data.Models;
using Services.Contracts.Mapping;
using System.Collections.Generic;
using System.Linq;

namespace Web.Areas.Admin.Models.Users
{
    public class UserPermissionsModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public IEnumerable<ApplicationRole> Roles { get; set; }

        public bool IsAdmin
        {
            get
            {
                return Roles.Any(x => x.Name.Equals(ApplicationRolesNames.AdminRole));
            }
        }

        public bool IsEditor
        {
            get
            {
                return Roles.Any(x => x.Name.Equals(ApplicationRolesNames.EditorRole));
            }
        }
    }
}
