using ArtistSiteAAD.Client.Shared;
using MudBlazor;

namespace ArtistSiteAAD.Client.Pages
{
    public class ListArtistsViewModel : BaseArtistSitePageViewModel
    {
        protected override IList<BreadcrumbItem> Breadcrumbs
        {
            get
            {
                return new List<BreadcrumbItem>()
                {
                    new BreadcrumbItem("Home", "/Index"),
                    new BreadcrumbItem("List of Artists", "/Artists", disabled: true)
                };
            }
        }
    }
}
