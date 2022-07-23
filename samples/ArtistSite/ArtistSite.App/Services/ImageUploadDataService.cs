using System.Net.Http;
using System.Threading.Tasks;

namespace ArtistSite.App.Services
{
    public class ImageUploadDataService : BaseDataService, IImageUploadDataService
    {
        public ImageUploadDataService(IHttpClientFactory httpClientFactory) //, ITokenManager tokenManager)
            : base(httpClientFactory)
        {
            //TokenManager = tokenManager;
        }

        //public ITokenManager TokenManager { get; set; }

        public async Task<HttpResponseMessage> UploadIcon(MultipartFormDataContent content)
        {
            //var accessToken = await TokenManager.RetrieveAccessTokenAsync();
            //HttpClient.SetBearerToken(accessToken);

            var retVal = await HttpClient.PostAsync($"api/Upload/UploadIcon", content);
            return retVal;
        }

        public async Task<HttpResponseMessage> UploadImage(MultipartFormDataContent content)
        {
            //var accessToken = await TokenManager.RetrieveAccessTokenAsync();
            //HttpClient.SetBearerToken(accessToken);

            var retVal = await HttpClient.PostAsync($"api/Upload/UploadImage", content);
            return retVal;
        }
    }
}