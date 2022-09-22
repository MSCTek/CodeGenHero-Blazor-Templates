using $safeprojectname$.Shared;

namespace $safeprojectname$.Components
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
