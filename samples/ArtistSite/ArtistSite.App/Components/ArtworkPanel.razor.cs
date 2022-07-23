using ArtistSite.App.Shared;
using ArtistSite.Shared.DTO;
using Microsoft.AspNetCore.Components;

namespace ArtistSite.App.Components
{
    public partial class ArtworkPanelViewModel : CGHComponentBase
    {
        [Parameter]
        public Artwork Artwork { get; set; }

        public string IconURL
        {
            get
            {
                return $"Images/{Artwork.IconUri}";
            }
        }

        private string detailURL;
        [Parameter]
        public string DetailURL
        {
            get
            {
                if (!string.IsNullOrEmpty(detailURL))
                    return detailURL;   // Custom URL was provided instantiating component.

                var autoURL = $"{NavigationManager.Uri}/{Artwork.ArtworkId}";

                return autoURL;
            }
            set
            {
                detailURL = value;
            }
        }
    }
}
