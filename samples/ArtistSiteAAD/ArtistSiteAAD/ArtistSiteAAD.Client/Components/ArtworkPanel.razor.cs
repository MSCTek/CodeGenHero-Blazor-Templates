using ArtistSiteAAD.Client.Shared;
using ArtistSiteAAD.Shared.DTO;
using Microsoft.AspNetCore.Components;

namespace ArtistSiteAAD.Client.Components
{
    public class ArtworkPanelViewModel : LinkPanelComponent
    {
        [Parameter, EditorRequired]
        public Artwork Artwork { get; set; } = default!;

        [Parameter]
        public bool FullSize { get; set; } = true;

        [Parameter]
        public int? ArtistId { get; set; }

        [Parameter]
        public int? MediumId { get; set; }

        protected override string PanelHref
        {
            get
            {
                if (MediumId.HasValue == ArtistId.HasValue)
                    return string.Empty;

                if (MediumId.HasValue)
                    return $"/Media/{MediumId.Value}/{Artwork.ArtworkId}";

                return $"/Artists/{ArtistId}/{Artwork.ArtworkId}";
            }
        }

        protected string GetArtworkSource()
        {
            if (FullSize)
                return $"artwork/{Artwork.ImageUri}";

            return $"artwork/{Artwork.IconUri}";
        }

        protected string GetPanelStyle()
        {
            if (FullSize)
                return string.Empty;

            return "margin: 20px; height: 300px; text-align: center;";
        }

        protected string GetImageStyle()
        {
            if (FullSize)
                return string.Empty;

            return "height: 200px; text-align: center;";
        }
    }
}
