using ArtistSiteAAD.Client.Services;
using ArtistSiteAAD.Client.Shared;
using MudBlazor;

namespace ArtistSiteAAD.Client.Pages
{
    public class IndexViewModel : BasePageViewModel
    {
        protected override IList<BreadcrumbItem> Breadcrumbs
        {
            get
            {
                return new List<BreadcrumbItem>()
                {
                    new BreadcrumbItem("Home", "/", disabled: true)
                };
            }
        }
    }
}
