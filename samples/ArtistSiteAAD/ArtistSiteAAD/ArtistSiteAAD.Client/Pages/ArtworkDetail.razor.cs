using ArtistSiteAAD.Client.Shared;
using ArtistSiteAAD.Shared.Constants;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ArtistSiteAAD.Client.Pages
{
    public class ArtworkDetailViewModel : BaseArtistSitePageViewModel
    {

        [Parameter]
        public int? ArtistId { get; set; }

        [Parameter]
        public int ArtworkId { get; set; }

        [Parameter]
        public int? MediumId { get; set; }

        protected override IList<BreadcrumbItem> Breadcrumbs
        {
            get
            {
                if (MediumId.HasValue)
                {
                    return new List<BreadcrumbItem>()
                    {
                        new BreadcrumbItem("Home", "/Index"),
                        new BreadcrumbItem("List of Media", "/Media"),
                        new BreadcrumbItem($"{Medium.Name}", $"/Media/{MediumId}"),
                        new BreadcrumbItem($"{Artwork.Name}", $"/Media/{MediumId}/{ArtworkId}", disabled: true)
                    };
                }

                // Artist
                return new List<BreadcrumbItem>()
                {
                    new BreadcrumbItem("Home", "/Index"),
                    new BreadcrumbItem("List of Artists", "/Artists"),
                    new BreadcrumbItem($"{Artwork.Artist.Name}", $"/Artists/{ArtistId}"),
                    new BreadcrumbItem($"{Artwork.Name}", $"/Artists/{ArtistId}/{ArtworkId}", disabled: true)
                };
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            await GetArtworkRouteParent();
            await GetArtworkAsync(ArtworkId, Enums.RelatedEntitiesType.ImmediateChildren);

            await base.OnParametersSetAsync();
        }

        private async Task GetArtworkRouteParent()
        {
            if (MediumId.HasValue)
            {
                await GetMediumAsync(MediumId.Value);
            }
        }
    }
}
