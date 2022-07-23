using ArtistSite.App.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System;
using System.Threading.Tasks;

namespace ArtistSite.Client.Services
{
    public class TokenManagerWasm : ITokenManager
    {
        private readonly NavigationManager _navigationManager;

        private readonly SignOutSessionStateManager _signOutManager;

        //private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenProvider _tokenProvider;

        private readonly IAccessTokenProvider _tokenProviderWasm;

        public TokenManagerWasm(ITokenProvider tokenProvider,
            //IHttpClientFactory httpClientFactory,
            IAccessTokenProvider tokenProviderWasm,
            SignOutSessionStateManager signOutManager,
            NavigationManager navigationManager)
        {
            _tokenProvider = tokenProvider ??
                throw new ArgumentNullException(nameof(tokenProvider));

            _tokenProviderWasm = tokenProviderWasm ??
                throw new ArgumentNullException(nameof(tokenProviderWasm));

            _signOutManager = signOutManager ??
                throw new ArgumentNullException(nameof(signOutManager));

            _navigationManager = navigationManager ??
                throw new ArgumentNullException(nameof(navigationManager));

            //_httpClientFactory = httpClientFactory ??
            //    throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<string> RetrieveAccessTokenAsync()
        {
            // See if the token is about to expire and refresh, if so.
            var tokenResult = await _tokenProviderWasm.RequestAccessToken();
            if (tokenResult.TryGetToken(out var token))
            {
                _tokenProvider.AccessToken = token.Value;
            }

            return _tokenProvider.AccessToken;
        }

        public async Task SignOutAsync()
        {
            await _signOutManager.SetSignOutState();
            _navigationManager.NavigateTo("authentication/logout");
        }
    }
}