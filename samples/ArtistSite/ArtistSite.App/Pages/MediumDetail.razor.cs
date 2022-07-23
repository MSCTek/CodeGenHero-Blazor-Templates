using ArtistSite.App.ViewModels;
using ArtistSite.Shared.Constants;
using ArtistSite.Shared.DataService;
using ArtistSite.Shared.DTO;
using MudBlazor;

namespace ArtistSite.App.Pages
{
    public class MediumDetailViewModel : BaseDetailPageViewModel
    {

        public IList<ArtworkMediumXref> ArtworkMediumXrefs { get; set; }

        protected async override Task OnParametersSetAsync()
        {
            await PopulateMediaPageContentAsync(MediumId);
        }

        private async Task PopulateMediaPageContentAsync(int mediumId)
        {
            if (Medium?.MediumId == mediumId)
                return;

            var populateMedium = PopulateMediumAsync();

            var populateMediumXrefs = PopulateArtworkMediumXrefsAsync();

            _= CreateBreadcrumbs(populateMedium);

            await populateMediumXrefs;
            await PopulateArtworksFromMediumXrefAsync();
        }

        private async Task CreateBreadcrumbs(Task awaitMedium)
        {
            await awaitMedium;

            List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>()
                {
                    new BreadcrumbItem("Home", "/"),
                    new BreadcrumbItem("Browse Media", "/Media"),
                    new BreadcrumbItem(Medium.Name, $"/Media/{Medium.MediumId}", disabled: true)
                };

            NavigationService.SetBreadcrumbs(breadcrumbs);
        }

        private async Task PopulateArtworksFromMediumXrefAsync()
        {
            if (ArtworkMediumXrefs == null)
                return;

            var artworkIds = ArtworkMediumXrefs.Select(x => x.ArtworkId).ToList();
            string artworkIdQuery = string.Join('|', artworkIds);

            List<IFilterCriterion> filterCriteria = new List<IFilterCriterion>()
            {
                new FilterCriterion(nameof(Artwork.ArtworkId), Enums.CriterionCondition.IsContainedIn, artworkIdQuery)
            };

            var result = await WebApiDataServiceAS.GetArtworksAsync(filterCriteria);

            if (result.IsSuccessStatusCode)
            {
                Artworks = result.Data.Data;
            }
        }

        private async Task PopulateArtworkMediumXrefsAsync()
        {
            List<IFilterCriterion> filterCriteria = new List<IFilterCriterion>()
            {
                new FilterCriterion(nameof(ArtworkMediumXref.MediumId), Enums.CriterionCondition.IsEqualTo, MediumId)
            };

            var result = await WebApiDataServiceAS.GetArtworkMediumXrefsAsync(filterCriteria: filterCriteria);

            if (result.IsSuccessStatusCode)
            {
                ArtworkMediumXrefs = result.Data.Data;
            }
        }

    }
}
