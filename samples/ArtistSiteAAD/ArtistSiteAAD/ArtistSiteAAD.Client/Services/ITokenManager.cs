using System.Threading.Tasks;

namespace ArtistSiteAAD.Client.Services
{
    public interface ITokenManager
    {
        Task<string> RetrieveAccessTokenAsync();

        Task SignOutAsync();
    }
}