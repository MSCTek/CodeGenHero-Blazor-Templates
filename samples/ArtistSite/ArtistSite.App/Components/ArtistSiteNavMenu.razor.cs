using ArtistSite.App.ViewModels;
using ArtistSite.Shared.DTO;

namespace ArtistSite.App.Components
{
    public class ArtistSiteNavMenuViewModel : BaseDetailPageViewModel
    {
        public IList<Artist> Artists { get; set; }

        public IList<Medium> Media { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var getArtists = WebApiDataServiceAS.GetAllPagesArtistsAsync();
            var getMedia = WebApiDataServiceAS.GetAllPagesMediaAsync();

            Artists = await getArtists;
            Media = await getMedia;
        }
    }
}
