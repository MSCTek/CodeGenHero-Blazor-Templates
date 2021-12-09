using $ext_safeprojectname$.App.Services;
using System;

namespace $safeprojectname$.Services
{
    public class TokenProviderWasm : ITokenProvider
    {
        public string AccessToken { get; set; }
        public DateTimeOffset ExpiresAt { get; set; }
        public string RefreshToken { get; set; }
        public string XsrfToken { get; set; }
    }
}