using ArtistSiteAAD.Client.Shared;
using ArtistSiteAAD.Shared.DTO;

namespace ArtistSiteAAD.Client.Components
{
    public class MediaListComponentViewModel : CGHComponentBase
    {

        protected IList<Medium> Media { get; set; } = new List<Medium>();

        protected override async Task OnInitializedAsync()
        {
            Media = await WebApiDataService.GetAllPagesMediaAsync();

            FirstLoad = false;
        }
    }
}
