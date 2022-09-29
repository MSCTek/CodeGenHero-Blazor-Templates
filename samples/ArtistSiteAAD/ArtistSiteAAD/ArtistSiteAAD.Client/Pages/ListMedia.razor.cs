using ArtistSiteAAD.Client.Shared;
using MudBlazor;

namespace ArtistSiteAAD.Client.Pages
{
    public class ListMediaViewModel : BasePageViewModel
    {
        protected override IList<BreadcrumbItem> Breadcrumbs
        {
            get
            {
                return new List<BreadcrumbItem>()
                {
                    new BreadcrumbItem("Home", "/Index"),
                    new BreadcrumbItem("List of Media", "/Media", disabled: true)
                };
            }
        }
    }
}
