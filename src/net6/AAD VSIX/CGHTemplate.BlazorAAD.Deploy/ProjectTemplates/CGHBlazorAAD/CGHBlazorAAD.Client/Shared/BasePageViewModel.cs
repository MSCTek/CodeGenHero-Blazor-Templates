using $safeprojectname$.Services;
using $ext_safeprojectname$.Shared.DTO;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;
using MudBlazor;
using System.Security.Claims;

namespace $safeprojectname$.Shared
{
    public abstract class BasePageViewModel : ComponentBase
    {
        // [Inject] /// Setup - Add your generated WebApiDataService here
        // protected IWebApiDataService WebApiDataService { get; set; } = default!;

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

        [Inject]
        protected ISnackbar Snackbar { get; set; } = default!;

        protected static ClaimsPrincipal? CurrentUser { get; set; }

        protected override async Task OnInitializedAsync()
        {
            CurrentUser = (await AuthenticationStateTask).User;
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
