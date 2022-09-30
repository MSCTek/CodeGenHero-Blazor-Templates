using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace ArtistSiteAAD.Api.Infrastructure
{
    public class IsMemberOfGroupHandler : AuthorizationHandler<IsMemberOfGroupRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsMemberOfGroupRequirement requirement)
        {
            var groupClaim = context.User.Claims
                .FirstOrDefault(claim => claim.Type == "groups"
                    && requirement.GroupIds.Contains(claim.Value));

            if (groupClaim != null)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }


    }
}
