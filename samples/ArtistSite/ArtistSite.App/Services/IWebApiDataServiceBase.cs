using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace ArtistSite.App.Services
{
    public partial interface IWebApiDataServiceBase
    {
        HttpClient HttpClient { get; }

        string IsServiceOnlineRelativeUrl { get; set; }

        ILogger Log { get; set; }

        Task<bool> IsServiceOnlineAsync();
    }
}