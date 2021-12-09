using Microsoft.AspNetCore.Components;
using $safeprojectname$.Services;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace $safeprojectname$.ViewModels
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