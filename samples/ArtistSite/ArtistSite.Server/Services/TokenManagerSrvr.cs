using ArtistSite.App.Services;
using IdentityModel.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ArtistSite.Server.Services
{
    public class TokenManagerSrvr : ITokenManager
    {
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly NavigationManager _navigationManager;
        private readonly ITokenProvider _tokenProvider;

        public TokenManagerSrvr(ITokenProvider tokenProvider,
            IHttpClientFactory httpClientFactory,
            NavigationManager navigationManager,
            IConfiguration config)
        {
            _tokenProvider = tokenProvider ??
                throw new ArgumentNullException(nameof(tokenProvider));

            _httpClientFactory = httpClientFactory ??
                throw new ArgumentNullException(nameof(httpClientFactory));

            _navigationManager = navigationManager ??
                throw new ArgumentNullException(nameof(navigationManager));

            _config = config ??
                throw new ArgumentNullException(nameof(config));
        }

        public async Task<string> RetrieveAccessTokenAsync()
        {
            // should we refresh?

            if ((_tokenProvider.ExpiresAt.AddSeconds(-60)).ToUniversalTime()
                    > DateTime.UtcNow)
            {
                // no need to refresh, return the access token
                return _tokenProvider.AccessToken;
            }

            // refresh
            var idpClient = _httpClientFactory.CreateClient();

            var discoveryReponse = await idpClient
                .GetDiscoveryDocumentAsync(_config["OidcConfiguration:Authority"]); // "https://localhost:5401");

            var refreshResponse = await idpClient.RequestRefreshTokenAsync(
               new RefreshTokenRequest
               {
                   Address = discoveryReponse.TokenEndpoint,
                   ClientId = _config["OidcConfiguration:ClientId"],
                   ClientSecret = _config["OidcConfiguration:ClientSecret"],
                   RefreshToken = _tokenProvider.RefreshToken
               });

            _tokenProvider.AccessToken = refreshResponse.AccessToken;
            _tokenProvider.RefreshToken = refreshResponse.RefreshToken;
            _tokenProvider.ExpiresAt = DateTime.UtcNow.AddSeconds(refreshResponse.ExpiresIn);

            return _tokenProvider.AccessToken;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task SignOutAsync()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            _navigationManager.NavigateTo("/logoutIDP");
        }
    }
}