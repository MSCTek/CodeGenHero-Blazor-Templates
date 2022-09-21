using System.Threading.Tasks;

namespace $safeprojectname$.Services
{
    public interface ITestAuthDataService
    {
        Task<string> GetTestAuthAsync();
    }
}