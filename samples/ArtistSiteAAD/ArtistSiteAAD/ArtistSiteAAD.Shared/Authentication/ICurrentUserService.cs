namespace ArtistSiteAAD.Shared.Authentication
{
    public interface ICurrentUserService
    {
        IUserSession GetCurrentUser();
    }
}
