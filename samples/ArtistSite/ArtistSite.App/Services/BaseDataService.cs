using System.Net.Http;

namespace ArtistSite.App.Services
{
    public class BaseDataService
    {
        public BaseDataService(IHttpClientFactory httpClientFactory, string httpClientName = "CGHApi")
        {
            HttpClientFactory = httpClientFactory;
            if (!string.IsNullOrWhiteSpace(httpClientName))
            {
                HttpClientName = httpClientName;
            }
        }

        public virtual HttpClient HttpClient
        {
            get
            {
                return HttpClientFactory.CreateClient(HttpClientName);
            }
        }

        public IHttpClientFactory HttpClientFactory { get; set; }
        protected string HttpClientName { get; set; }
    }
}