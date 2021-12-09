using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace $safeprojectname$.MessageHandlers
{
    public class MSCApiAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public MSCApiAuthorizationMessageHandler(
            IAccessTokenProvider provider, NavigationManager navigation)
            : base(provider, navigation)
        {
            ConfigureHandler(
                  authorizedUrls: new[] {
                      "https://localhost:44323/",
                      "https://localhost:5301"
                  });
        }
    }
}