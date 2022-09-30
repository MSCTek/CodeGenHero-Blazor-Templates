using Microsoft.AspNetCore.Components;
using System.Net.Http;

namespace ArtistSiteAAD.Client.Services
{
    public interface ILocalHttpClientService
    {
        HttpClient HttpClient { get; }
        NavigationManager NavigationManager { get; }
    }
}