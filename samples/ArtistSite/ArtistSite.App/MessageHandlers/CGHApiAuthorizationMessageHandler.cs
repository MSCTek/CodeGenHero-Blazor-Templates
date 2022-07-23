using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace ArtistSite.App.MessageHandlers
{
    public class CGHApiAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public CGHApiAuthorizationMessageHandler(
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