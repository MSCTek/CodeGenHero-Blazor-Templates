using ArtistSiteAAD.Client.Services;
using ArtistSiteAAD.Shared.DTO;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;
using MudBlazor;
using System.Security.Claims;

namespace ArtistSiteAAD.Client.Shared
{
    public abstract class BasePageViewModel : ComponentBase
    {
        [Inject] /// Setup - Add your generated WebApiDataService here
        protected IWebApiDataServiceAS WebApiDataService { get; set; } = default!;

        [CascadingParameter]
        protected Task<AuthenticationState> AuthenticationStateTask { get; set; } = default!;

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        [Inject]
        protected IOptions<AADGroupConfiguration> AADGroupConfigurationOptions { get; set; } = default!;

        protected AADGroupConfiguration AADGroupConfiguration
        {
            get
            {
                return AADGroupConfigurationOptions.Value;
            }
        }

        protected virtual IList<BreadcrumbItem> Breadcrumbs { get; } = new List<BreadcrumbItem>();

        [Inject]
        protected ISnackbar Snackbar { get; set; } = default!;

        [Inject]
        private INavigationService NavigationService { get; set; } = default!;

        protected static ClaimsPrincipal? CurrentUser { get; set; }

        public void SetPageBreadcrumbs(IList<BreadcrumbItem> breadcrumbs)
        {
            NavigationService.SetBreadcrumbs(breadcrumbs);
        }

        protected override async Task OnInitializedAsync()
        {
            CurrentUser = (await AuthenticationStateTask).User;
        }

        protected override async Task OnParametersSetAsync()
        {
            SetPageBreadcrumbs(Breadcrumbs);
        }

        protected bool UserIsInGroup(string[] groupIds)
        {
            bool retVal = false;

            if (CurrentUser.Claims.Any())
            {
                var groupClaims = CurrentUser.Claims.Where(claim => claim.Type == "groups").ToList();
                retVal = groupClaims.Any(x => groupIds.Contains(x.Value));
            }

            return retVal;
        }

        protected bool IsAdmin
        {
            get
            {
                bool retVal = false;

                if (UserIsInGroup(AADGroupConfiguration.AdminGroupIdsArray))
                {
                    retVal = true;
                }

                return retVal;
            }
        }

        protected bool HasUserAccess
        {
            get
            {
                bool retVal = false;

                if (UserIsInGroup(AADGroupConfiguration.AuthorizedUserGroupIdsArray))
                {
                    retVal = true;
                }

                return retVal;
            }
        }

    }
}
