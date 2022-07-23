using System;

namespace ArtistSite.App.Services
{
    public class InitialApplicationState
    {
        public string AccessToken { get; set; }
        public DateTimeOffset ExpiresAt { get; set; }
        public string RefreshToken { get; set; }
        public string XsrfToken { get; set; }
    }
}