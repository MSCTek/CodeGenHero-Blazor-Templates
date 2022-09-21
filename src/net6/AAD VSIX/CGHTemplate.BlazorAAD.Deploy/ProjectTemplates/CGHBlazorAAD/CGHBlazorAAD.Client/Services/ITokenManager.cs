using System.Threading.Tasks;

namespace $safeprojectname$.Services
{
    public interface ITokenManager
    {
        Task<string> RetrieveAccessTokenAsync();

        Task SignOutAsync();
    }
}