using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using ArtistSiteAAD.Client.Services;
using ArtistSiteAAD.Shared.DTO;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ArtistSiteAAD.Client.Shared
{
    public class CGHComponentBase : ComponentBase
    {
        [Inject] /// Setup - Add your generated WebApiDataService here
        protected IWebApiDataServiceAS WebApiDataService { get; set; } = default!;

        [Inject]
        public IConfiguration Configuration { get; set; } = default!;

        public string ImagesBaseAddress
        {
            get
            {
                return CGHAppSettings.ImagesBaseAddress;
            }
        }

        [Inject]
        public IJSRuntime JsRuntime { get; set; } = default!;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        public INavigationService NavigationService { get; set; } = default!;

        public ClaimsPrincipal User { get; set; }

        public CGHAppSettings CGHAppSettings
        {
            get
            {
                return CGHAppSettingsOptions.Value;
            }
        }

        public bool FirstLoad { get; set; } = true;

        [Inject]
        public IOptions<CGHAppSettings> CGHAppSettingsOptions { get; set; } = default!;

        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

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