using ArtistSite.Shared.Constants;
using ArtistSite.Shared.DTO;
using Microsoft.AspNetCore.Components;

namespace ArtistSite.App.ViewModels
{
    public class BaseDetailPageViewModel : BaseViewModel
    {
        [Parameter]
        public int ArtistId { get; set; } = 0;

        public Artist Artist { get; set; }

        public IList<Artwork> Artworks { get; set; }

        [Parameter]
        public int MediumId { get; set; } = 0;

        public Medium Medium { get; set; }

        protected async Task PopulateArtistAsync(bool getArtworks = false)
        {
            var getRelatedEntities = getArtworks ? Enums.RelatedEntitiesType.ImmediateChildren : Enums.RelatedEntitiesType.None;

            var result = await WebApiDataServiceAS.GetArtistAsync(ArtistId, getRelatedEntities);

            if (result.IsSuccessStatusCode)
            {
                Artist = result.Data;
            }
        }

        protected async Task PopulateMediumAsync()
        {
            var result = await WebApiDataServiceAS.GetMediumAsync(MediumId);

            if (result.IsSuccessStatusCode)
            {
                Medium = result.Data;
            }
        }
    }
}
