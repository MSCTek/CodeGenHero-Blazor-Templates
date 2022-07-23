using Microsoft.AspNetCore.Components;
using System.Net.Http;

namespace ArtistSite.App.Services
{
    public interface ILocalHttpClientService
    {
        HttpClient HttpClient { get; }
        NavigationManager NavigationManager { get; }
    }
}