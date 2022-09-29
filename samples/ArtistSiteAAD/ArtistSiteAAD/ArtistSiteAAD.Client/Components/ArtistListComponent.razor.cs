using ArtistSiteAAD.Client.Shared;
using ArtistSiteAAD.Shared.DTO;

namespace ArtistSiteAAD.Client.Components
{
    public class ArtistListComponentViewModel : CGHComponentBase
    {
        protected IList<Artist> Artists { get; set; } = new List<Artist>();

        protected override async Task OnInitializedAsync()
        {
            Artists = await WebApiDataService.GetAllPagesArtistsAsync();

            FirstLoad = false;
        }

    }
}
