using ArtistSite.App.Services;
using ArtistSite.Shared.DTO;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

namespace ArtistSite.App.ViewModels
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

        [Inject]
        public IWebApiDataServiceAS WebApiDataServiceAS { get; set; }
    }
}