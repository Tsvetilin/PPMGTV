using Common.Constants;
using System.Security.Claims;

namespace Common.Helpers
{
    public static class UserClaimsExtensions
    {
        public static bool IsEditor(this ClaimsPrincipal user)
        {
            return user.IsInRole(ApplicationRolesNames.AdminRole) || user.IsInRole(ApplicationRolesNames.EditorRole);
        }

        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            return user.IsInRole(ApplicationRolesNames.AdminRole);
        }
    }
}
