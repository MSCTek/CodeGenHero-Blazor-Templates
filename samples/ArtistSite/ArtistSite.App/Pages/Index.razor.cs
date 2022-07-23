using ArtistSite.App.Shared;
using ArtistSite.Shared.DataService;
using ArtistSite.Shared.DTO;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using enums = ArtistSite.Shared.Constants.Enums;

namespace ArtistSite.App.Pages
{
    public partial class IndexViewModel : CGHComponentBase
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