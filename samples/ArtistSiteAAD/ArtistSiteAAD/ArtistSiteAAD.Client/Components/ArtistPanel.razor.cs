using ArtistSiteAAD.Client.Shared;
using ArtistSiteAAD.Shared.DTO;
using Microsoft.AspNetCore.Components;

namespace ArtistSiteAAD.Client.Components
{
    public class ArtistPanelViewModel : LinkPanelComponent
    {

        [Parameter, EditorRequired]
        public Artist Artist { get; set; } = default!;

        [Parameter]
        public bool ProfilePage { get; set; } = false;

        protected string ProfileImageUrl => $"artist-profile/{Artist.ArtistId}.png";

        protected override string PanelHref => $"Artists/{Artist.ArtistId}";

        protected string PanelStyle
        {
            get
            {
                return $"{(ProfilePage ? "" : "height: 500px;")} margin: 0px 10px; text-align: center;";
            }
        }

    }
}
