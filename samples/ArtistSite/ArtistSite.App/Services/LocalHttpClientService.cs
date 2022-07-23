using Microsoft.AspNetCore.Components;
using System;
using System.Net.Http;

namespace ArtistSite.App.Services
{
    public class LocalHttpClientService : ILocalHttpClientService
    {
        public LocalHttpClientService(HttpClient httpClient, NavigationManager navigationManager)
        {
            HttpClient = httpClient;
            NavigationManager = navigationManager;
            HttpClient.BaseAddress = new Uri(NavigationManager.BaseUri);
        }

        public HttpClient HttpClient { get; }
        public NavigationManager NavigationManager { get; }
    }
}