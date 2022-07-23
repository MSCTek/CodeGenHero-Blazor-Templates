using ArtistSite.App.ViewModels;
using ArtistSite.Shared.DTO;
using MudBlazor;

namespace ArtistSite.App.Pages
{
    public class ListArtistsViewModel : BaseViewModel
    {
        public IList<Artist> Artists { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Artists = await WebApiDataServiceAS.GetAllPagesArtistsAsync();

            List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>()
                {
                    new BreadcrumbItem("Home", "/"),
                    new BreadcrumbItem("Browse Artists", "/Artists", disabled: true),
                };

            NavigationService.SetBreadcrumbs(breadcrumbs);
        }
    }
}
