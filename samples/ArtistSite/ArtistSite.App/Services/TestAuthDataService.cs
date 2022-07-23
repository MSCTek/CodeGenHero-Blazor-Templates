using IdentityModel.Client;
using System.Net.Http;
using System.Threading.Tasks;

namespace ArtistSite.App.Services
{
    public class TestAuthDataService : BaseDataService, ITestAuthDataService
    {
        public TestAuthDataService(IHttpClientFactory httpClientFactory, ITokenManager tokenManager)
            : base(httpClientFactory)
        {
            TokenManager = tokenManager;
        }

        public ITokenManager TokenManager { get; set; }

        public async Task<string> GetTestAuthAsync()
        {
            var accessToken = await TokenManager.RetrieveAccessTokenAsync();
            HttpClient.SetBearerToken(accessToken);

            string retVal = await HttpClient.GetStringAsync($"api/TestAuth/");

            return retVal;
        }
    }
}