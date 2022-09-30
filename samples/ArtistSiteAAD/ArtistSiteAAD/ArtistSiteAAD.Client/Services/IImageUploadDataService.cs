using System.Net.Http;
using System.Threading.Tasks;

namespace ArtistSiteAAD.Client.Services
{
    public interface IImageUploadDataService
    {
        Task<HttpResponseMessage> UploadIcon(MultipartFormDataContent content);

        Task<HttpResponseMessage> UploadImage(MultipartFormDataContent content);
    }
}