using ArtistSiteAAD.Client.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ArtistSiteAAD.Client.Pages
{
    public class ArtistDetailViewModel : BaseArtistSitePageViewModel
    {
        [Parameter]
        public int ArtistId { get; set; }

        protected override IList<BreadcrumbItem> Breadcrumbs
        {
            get
            {
                return new List<BreadcrumbItem>()
                {
                    new BreadcrumbItem("Home", "/Index"),
                    new BreadcrumbItem("List of Artists", "/Artists"),
                    new BreadcrumbItem($"{Artist.Name}", $"/Artists/{ArtistId}", disabled: true)
                };
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            await GetArtistAsync(ArtistId);

            await base.OnParametersSetAsync();
        }
    }
}
