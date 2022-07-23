using System.Threading.Tasks;

namespace ArtistSite.App.Services
{
    public interface ITokenManager
    {
        Task<string> RetrieveAccessTokenAsync();

        Task SignOutAsync();
    }
}