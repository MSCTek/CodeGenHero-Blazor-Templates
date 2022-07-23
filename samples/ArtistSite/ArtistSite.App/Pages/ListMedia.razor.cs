using ArtistSite.App.ViewModels;
using ArtistSite.Shared.DTO;
using MudBlazor;

namespace ArtistSite.App.Pages
{
    public class ListMediaViewModel : BaseViewModel
    {

        public IList<Medium> Media { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Media = await WebApiDataServiceAS.GetAllPagesMediaAsync();

            List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>()
                {
                    new BreadcrumbItem("Home", "/"),
                    new BreadcrumbItem("Browse Media", "/Media", disabled: true),
                };

            NavigationService.SetBreadcrumbs(breadcrumbs);
        }

    }
}
