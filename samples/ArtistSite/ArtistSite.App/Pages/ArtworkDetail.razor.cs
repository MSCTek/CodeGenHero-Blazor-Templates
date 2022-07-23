using ArtistSite.App.ViewModels;
using ArtistSite.Shared.DTO;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ArtistSite.App.Pages
{
    public class ArtworkDetailViewModel : BaseDetailPageViewModel
    {

        [Parameter]
        public int ArtworkId { get; set; }

        public Artwork Artwork { get; set; }

        public string ImageURL
        {
            get
            {
                return $"Images/{Artwork.ImageUri}";
            }
        }

        protected override async Task OnInitializedAsync()
        {
            var populateArtwork = PopulateArtwork();

            if (MediumId != 0)
            {
                var populateMedium = PopulateMediumAsync();
                await CreateMediumBreadcrumbs(populateMedium, populateArtwork);
            }

            if (ArtistId != 0)
            {
                var populateArtist = PopulateArtistAsync();
                await CreateArtistBreadcrumbs(populateArtist, populateArtwork);
            }
        }

        // Example P7E1 - Using the relatedEntitiesType enum to access Artist alongside Artwork.
        private async Task PopulateArtwork()
        {
            var result = await WebApiDataServiceAS.GetArtworkAsync(ArtworkId, ArtistSite.Shared.Constants.Enums.RelatedEntitiesType.ImmediateChildren);

            if (result.IsSuccessStatusCode)
            {
                Artwork = result.Data;
            }
        }

        private async Task CreateMediumBreadcrumbs(Task awaitMedium, Task awaitArtwork)
        {
            await awaitArtwork;
            await awaitMedium;

            List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>()
                {
                    new BreadcrumbItem("Home", "/"),
                    new BreadcrumbItem("Browse Media", "/Media"),
                    new BreadcrumbItem(Medium.Name, $"/Media/{Medium.MediumId}"),
                    new BreadcrumbItem(Artwork.Name, $"/Media/{Medium.MediumId}/{Artwork.ArtworkId}", disabled: true)
                };

            NavigationService.SetBreadcrumbs(breadcrumbs);
        }

        private async Task CreateArtistBreadcrumbs(Task awaitArtist, Task awaitArtwork)
        {
            await awaitArtwork;
            await awaitArtist;

            List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>()
                {
                    new BreadcrumbItem("Home", "/"),
                    new BreadcrumbItem("Browse Artists", "/Artists"),
                    new BreadcrumbItem(Artist.Name, $"/Artists/{Artist.ArtistId}"),
                    new BreadcrumbItem(Artwork.Name, $"/Artists/{Artist.ArtistId}/{Artwork.ArtworkId}", disabled: true)
                };

            NavigationService.SetBreadcrumbs(breadcrumbs);
        }

    }
}
