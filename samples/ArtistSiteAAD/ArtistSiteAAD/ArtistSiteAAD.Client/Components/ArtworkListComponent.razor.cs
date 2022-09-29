using ArtistSiteAAD.Client.Shared;
using ArtistSiteAAD.Shared.Constants;
using ArtistSiteAAD.Shared.DataService;
using ArtistSiteAAD.Shared.DTO;
using Microsoft.AspNetCore.Components;

namespace ArtistSiteAAD.Client.Components
{
    public class ArtworkListComponentViewModel : CGHComponentBase
    {
        [Parameter]
        public int? MediumId { get; set; }

        [Parameter]
        public int? ArtistId { get; set; }

        [Parameter]
        public IList<Artwork> Artworks { get; set; } = new List<Artwork>();

        protected bool BothIdsSetError { get; set; } = false;

        protected override async Task OnParametersSetAsync()
        {
            if (Artworks.Any())
                return; // Getting Artwork already handled by instantiating component. IE, Artist page having Child Object to provide.

            if (MediumId.HasValue && ArtistId.HasValue)
            {
                BothIdsSetError = true;
                return;
            }

            if (MediumId.HasValue)
                await GetArtworksByMediumIdAsync(MediumId.Value);
            else if (ArtistId.HasValue)
                await GetArtworksByArtistIdAsync(ArtistId.Value);
        }

        protected string GetArtworkDetailUrl(int artworkId)
        {
            if (MediumId != 0)
                return $"/Media/{MediumId}/{artworkId}";
            
            if (ArtistId != 0)
                return $"/Artists/{ArtistId}/{artworkId}";

            return string.Empty;
        }

        private async Task GetArtworksByArtistIdAsync(int artistId)
        {
            var artistFilter = new List<IFilterCriterion>()
            {
                new FilterCriterion(nameof(Artwork.ArtistId), Enums.CriterionCondition.IsEqualTo, artistId)
            };

            Artworks = await WebApiDataService.GetAllPagesArtworksAsync(artistFilter);
        }

        private async Task GetArtworksByMediumIdAsync(int mediumId)
        {
            var xrefs = await GetArtworkMediumXrefsAsync(mediumId);
            await PopulateArtworksUsingMediumXrefsAsync(xrefs);
        }

        private async Task<IList<ArtworkMediumXref>> GetArtworkMediumXrefsAsync(int mediumId)
        {
            var xrefFilterCriteria = new List<IFilterCriterion>()
            {
                new FilterCriterion(nameof(ArtworkMediumXref.MediumId), Enums.CriterionCondition.IsEqualTo, mediumId)
            };

            var artworkMediumXrefs = await WebApiDataService.GetAllPagesArtworkMediumXrefsAsync(xrefFilterCriteria);

            return artworkMediumXrefs;
        }

        private async Task PopulateArtworksUsingMediumXrefsAsync(IList<ArtworkMediumXref> artworkMediumXrefs)
        {
            var artworkIds = artworkMediumXrefs.Select(x => x.ArtworkId).ToList();
            var artworkIdsString = string.Join('|', artworkIds);

            var artworkFilterCriteria = new List<IFilterCriterion>()
            {
                new FilterCriterion(nameof(Artwork.ArtworkId), Enums.CriterionCondition.IsContainedIn, artworkIdsString)
            };

            Artworks = await WebApiDataService.GetAllPagesArtworksAsync(artworkFilterCriteria);
        }
    }
}
