namespace $safeprojectname$.Services
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
    using Microsoft.Extensions.Options;

    public class CustomAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public CustomAuthorizationMessageHandler(IAccessTokenProvider provider,
            NavigationManager navigationManager,
            IOptions<CGHAppSettings> cghAppSettings)
            : base(provider, navigationManager)
        {
            var apiBaseAddress = cghAppSettings.Value.ApiBaseAddress;
            var apiScope = cghAppSettings.Value.ApiScope;

            ConfigureHandler(
                authorizedUrls: new[] { apiBaseAddress },
                scopes: new[] { apiScope });
        }
    }
}
