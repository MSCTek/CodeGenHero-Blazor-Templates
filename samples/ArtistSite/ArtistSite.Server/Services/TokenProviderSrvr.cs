using ArtistSite.App.Services;
using System;

namespace ArtistSite.Server.Services
{
    public class TokenProviderSrvr : ITokenProvider
    {
        public string AccessToken { get; set; }
        public DateTimeOffset ExpiresAt { get; set; }
        public string RefreshToken { get; set; }
        public string XsrfToken { get; set; }
    }
}