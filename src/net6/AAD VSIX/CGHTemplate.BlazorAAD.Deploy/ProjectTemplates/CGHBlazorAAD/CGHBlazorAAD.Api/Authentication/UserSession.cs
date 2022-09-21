namespace $safeprojectname$.Authentication
{
    using $ext_safeprojectname$.Shared.Authentication;

    public class UserSession : IUserSession
    {
        public string LoginName { get; set; }

        public bool IsAuthenticated { get; set; }
    }
}
