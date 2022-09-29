using ArtistSiteAAD.Client.Shared;
using ArtistSiteAAD.Shared.DTO;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ArtistSiteAAD.Client.Pages
{
    public class MediumDetailViewModel : BaseArtistSitePageViewModel
    {
        [Parameter]
        public int MediumId { get; set; }

        protected override IList<BreadcrumbItem> Breadcrumbs
        {
            get
            {
                return new List<BreadcrumbItem>()
                {
                    new BreadcrumbItem("Home", "/Index"),
                    new BreadcrumbItem("List of Media", "/Media"),
                    new BreadcrumbItem($"{Medium.Name}", $"/Media/{MediumId}", disabled: true)
                };
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            await GetMediumAsync(MediumId);
            await base.OnParametersSetAsync();
        }

    }
}
