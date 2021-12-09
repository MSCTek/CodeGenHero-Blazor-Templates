using $safeprojectname$.Shared;
using $ext_safeprojectname$.Shared.DataService;
using $ext_safeprojectname$.Shared.DTO;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using enums = $ext_safeprojectname$.Shared.Constants.Enums;

namespace $safeprojectname$.Pages
{
    public partial class IndexViewModel : MSCComponentBase
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        protected override async Task OnInitializedAsync()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>()
                {
                    new BreadcrumbItem("Home", "/", disabled: true)
                };
                NavigationService.SetBreadcrumbs(breadcrumbs);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}