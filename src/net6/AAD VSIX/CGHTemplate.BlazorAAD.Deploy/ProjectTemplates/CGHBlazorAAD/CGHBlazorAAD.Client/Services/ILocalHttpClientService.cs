using Microsoft.AspNetCore.Components;
using System.Net.Http;

namespace $safeprojectname$.Services
{
    public interface ILocalHttpClientService
    {
        HttpClient HttpClient { get; }
        NavigationManager NavigationManager { get; }
    }
}