using System;

namespace ArtistSiteAAD.Client.Services
{
    public interface ITokenProvider
    {
        string AccessToken { get; set; }
        DateTimeOffset ExpiresAt { get; set; }
        string RefreshToken { get; set; }
        string XsrfToken { get; set; }
    }
}