using ArtistSite.App.Shared;
using ArtistSite.Shared.DTO;
using Microsoft.AspNetCore.Components;

namespace ArtistSite.App.Components
{
    public class ArtistPanelViewModel : CGHComponentBase
    {
        [Parameter]
        public Artist Artist { get; set; }

        public string ArtistHref
        {
            get
            {
                return $"/Artists/{Artist.ArtistId}";
            }
        }

        public string ProfileImageUrl
        {
            get
            {
                return $"Images/Profile/{Artist.ArtistId}.jpg";
            }
        }
    }
}
