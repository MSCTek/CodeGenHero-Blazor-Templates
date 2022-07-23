using ArtistSite.App.Services;
using ArtistSite.Shared.DTO;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ArtistSite.App.Shared
{
    public class CGHComponentBase : ComponentBase
    {
        [Inject]
        public IConfiguration Configuration { get; set; }

        public string ImagesBaseAddress
        {
            get
            {
                return CGHAppSettings.ImagesBaseAddress;
            }
        }

        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public INavigationService NavigationService { get; set; }

        public ClaimsPrincipal User { get; set; }

        public CGHAppSettings CGHAppSettings
        {
            get
            {
                return CGHAppSettingsOptions.Value;
            }
        }

        [Inject]
        public IOptions<CGHAppSettings> CGHAppSettingsOptions { get; set; }

        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        protected async Task<ClaimsPrincipal> GetClaimsPrincipalAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var retVal = authState.User;

            return retVal;
        }

        protected string GetClaimsPrincipalClaim(ClaimsPrincipal user, string claimType)
        {
            string retVal = null;

            var matchingClaim = user.FindFirst(c => c.Type == claimType);
            retVal = matchingClaim?.Value;

            return retVal;
        }

        protected async Task<string> GetClaimsPrincipalClaimAsync(string claimType)
        {
            string retVal = null;

            var user = await GetClaimsPrincipalAsync();
            retVal = GetClaimsPrincipalClaim(user, claimType);

            return retVal;
        }

        protected async Task<List<Claim>> GetClaimsPrincipalClaimsAsync()
        {
            List<Claim> retVal = new List<Claim>();
            var user = await GetClaimsPrincipalAsync();
            if (user.Identity.IsAuthenticated)
            {
                retVal.AddRange(user.Claims);
            }

            return retVal;
        }

        protected override async Task OnInitializedAsync()
        {
            User = await GetClaimsPrincipalAsync();
        }
    }
}