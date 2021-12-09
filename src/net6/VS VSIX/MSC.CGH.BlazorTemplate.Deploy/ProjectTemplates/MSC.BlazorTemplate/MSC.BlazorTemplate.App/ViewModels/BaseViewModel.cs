using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using $safeprojectname$.Services;
using $ext_safeprojectname$.Shared.DTO;

namespace $safeprojectname$.ViewModels
{
    public class BaseViewModel : ComponentBase
    {
        public string ImagesBaseAddress
        {
            get
            {
                return CGHAppSettings.ImagesBaseAddress;
            }
        }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public INavigationService NavigationService { get; set; }

        public CGHAppSettings CGHAppSettings
        {
            get
            {
                return CGHAppSettingsOptions.Value;
            }
        }

        [Inject]
        public IOptions<CGHAppSettings> CGHAppSettingsOptions { get; set; }
    }
}