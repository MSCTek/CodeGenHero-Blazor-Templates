using Microsoft.AspNetCore.Components;

namespace ArtistSiteAAD.Client.Shared
{
    public abstract class LinkPanelComponent : CGHComponentBase
    {
        [Parameter]
        public bool ActAsLink { get; set; } = false;

        protected abstract string PanelHref { get; }
    }
}
