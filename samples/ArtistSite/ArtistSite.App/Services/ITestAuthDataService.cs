using System.Threading.Tasks;

namespace ArtistSite.App.Services
{
    public interface ITestAuthDataService
    {
        Task<string> GetTestAuthAsync();
    }
}