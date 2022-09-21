namespace $safeprojectname$.Authentication
{
    public interface ICurrentUserService
    {
        IUserSession GetCurrentUser();
    }
}
