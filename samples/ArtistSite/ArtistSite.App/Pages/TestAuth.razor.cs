using ArtistSite.App.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtistSite.App.ViewModels
{
    public class TestAuthViewModel : BaseViewModel
    {
        [Inject]
        public ITestAuthDataService TestAuthDataService { get; set; }

        public string TestAuthResponse { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            if (string.IsNullOrWhiteSpace(TestAuthResponse))
            {
                TestAuthResponse = await TestAuthDataService.GetTestAuthAsync();
            }

            List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>()
            {
                new BreadcrumbItem("Home", "/"),
                new BreadcrumbItem("Test Auth", "/", disabled: true)
            };

            NavigationService.SetBreadcrumbs(breadcrumbs);
        }
    }
}