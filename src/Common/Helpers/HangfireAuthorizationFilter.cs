using Common.Constants;
using Hangfire.Dashboard;

namespace Common.Helpers
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            return httpContext.User.IsInRole(ApplicationRolesNames.AdminRole);
        }
    }
}
