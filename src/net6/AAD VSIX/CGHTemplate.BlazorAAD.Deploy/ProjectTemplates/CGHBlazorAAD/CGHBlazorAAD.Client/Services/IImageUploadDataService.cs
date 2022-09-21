using System.Net.Http;
using System.Threading.Tasks;

namespace $safeprojectname$.Services
{
    public interface IImageUploadDataService
    {
        Task<HttpResponseMessage> UploadIcon(MultipartFormDataContent content);

        Task<HttpResponseMessage> UploadImage(MultipartFormDataContent content);
    }
}