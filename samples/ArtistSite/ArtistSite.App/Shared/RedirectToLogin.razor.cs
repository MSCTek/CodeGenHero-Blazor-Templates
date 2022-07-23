using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Threading.Tasks;

namespace ArtistSite.App.Shared
{
    public partial class RedirectToLoginViewModel : CGHComponentBase
    {
        [CascadingParameter]
        public Task<AuthenticationState> AuthState { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthState;
            var user = authState.User;

            // If the user hitting the login page is already authenticated, chances are that they got redirected here by the App.razor page's NotAuthorized path. The user may have tried to directly access a page for which they do not have access.
            if (user != null && user.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo($"/Authorization/AccessDenied");
            }

            if (CGHAppSettings.IsWebAssembly)
            {
                NavigationManager.NavigateTo($"authentication/login?returnUrl={Uri.EscapeDataString(NavigationManager.Uri)}");
            }
            else
            {
                NavigationManager.NavigateTo($"LoginIDP?redirectUri={Uri.EscapeDataString(NavigationManager.Uri)}",
                    forceLoad: true);
            }
        }
    }
}