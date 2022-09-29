using ArtistSiteAAD.Client.Shared;

namespace ArtistSiteAAD.Client.Components
{
    public class ApplicationDrawerViewModel : CGHComponentBase
    {
        protected bool DrawerOpen { get; set; } = true;

        protected void ToggleDrawer()
        {
            DrawerOpen = !DrawerOpen;
        }
    }
}
