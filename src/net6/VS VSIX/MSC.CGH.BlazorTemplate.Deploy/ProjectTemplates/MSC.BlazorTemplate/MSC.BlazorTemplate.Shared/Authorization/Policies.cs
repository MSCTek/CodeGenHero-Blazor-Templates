using Microsoft.AspNetCore.Authorization;

namespace $safeprojectname$.Authorization
{
    public static class Policies
    {
        public const string CanManageEmployees = "CanManageEmployees";

        public static AuthorizationPolicy CanManageEmployeesPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("country", "USA")
                .Build();
        }
    }
}