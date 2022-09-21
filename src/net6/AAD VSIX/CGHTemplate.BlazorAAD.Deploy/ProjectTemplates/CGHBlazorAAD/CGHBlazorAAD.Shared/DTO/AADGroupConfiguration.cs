namespace $safeprojectname$.DTO
{
    public class AADGroupConfiguration
    {
        public string AdminGroupIds { get; set; }
        public string AuthorizedUserGroupIds { get; set; }

        public string[] AdminGroupIdsArray
        {
            get
            {
                return AdminGroupIds?.Split('\u002C');
            }
        }

        public string[] AuthorizedUserGroupIdsArray
        {
            get
            {
                return AuthorizedUserGroupIds?.Split('\u002C');
            }
        }
    }
}