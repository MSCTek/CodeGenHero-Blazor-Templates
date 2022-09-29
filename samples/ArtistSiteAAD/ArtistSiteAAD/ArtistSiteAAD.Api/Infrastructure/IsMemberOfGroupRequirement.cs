using Microsoft.AspNetCore.Authorization;

namespace ArtistSiteAAD.Api.Infrastructure
{
    public class IsMemberOfGroupRequirement : IAuthorizationRequirement
    {
        public readonly string[] GroupIds;
        public readonly string GroupName;

        public IsMemberOfGroupRequirement(string groupName, params string[] groupIds)
        {
            GroupName = groupName;
            GroupIds = groupIds;
        }
    }
}
