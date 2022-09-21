namespace $safeprojectname$.Infrastructure
{
    using Microsoft.AspNetCore.Authorization;

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
