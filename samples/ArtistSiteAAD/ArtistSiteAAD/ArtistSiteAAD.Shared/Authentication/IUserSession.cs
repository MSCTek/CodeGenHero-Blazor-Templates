namespace ArtistSiteAAD.Shared.Authentication
{
    public interface IUserSession
    {
        string LoginName { get; set; }

        bool IsAuthenticated { get; set; }
    }
}
