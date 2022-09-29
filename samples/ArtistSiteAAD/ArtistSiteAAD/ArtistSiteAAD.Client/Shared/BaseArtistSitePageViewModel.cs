using ArtistSiteAAD.Shared.Constants;
using ArtistSiteAAD.Shared.DTO;

namespace ArtistSiteAAD.Client.Shared
{
    public class BaseArtistSitePageViewModel : BasePageViewModel
    {

        public Artist Artist { get; set; } = default!;

        public Artwork Artwork { get; set; } = default!;

        public Medium Medium { get; set; } = default!;

        public async Task GetArtistAsync(int artistId)
        {
            var artistResult = await WebApiDataService.GetArtistAsync(artistId);

            if (artistResult.IsSuccessStatusCode)
                Artist = artistResult.Data;
        }

        public async Task GetArtworkAsync(int artworkId, Enums.RelatedEntitiesType relatedEntitiesType = Enums.RelatedEntitiesType.None)
        {
            var artworkResult = await WebApiDataService.GetArtworkAsync(artworkId, relatedEntitiesType);

            if (artworkResult.IsSuccessStatusCode)
                Artwork = artworkResult.Data;
        }

        public async Task GetMediumAsync(int mediumId)
        {
            var mediumResult = await WebApiDataService.GetMediumAsync(mediumId);

            if (mediumResult.IsSuccessStatusCode)
                Medium = mediumResult.Data;
        }
    }
}
