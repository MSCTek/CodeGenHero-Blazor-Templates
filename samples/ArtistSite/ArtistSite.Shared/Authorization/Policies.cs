using Microsoft.AspNetCore.Authorization;

namespace ArtistSite.Shared.Authorization
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