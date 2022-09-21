using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace $safeprojectname$.Shared
{
    public class ApplicationLayoutViewModel : LayoutComponentBase
    {
        protected MudTheme ApplicationCustomTheme = new MudTheme()
        {
            Palette = new Palette()
            {
                Primary = Colors.Blue.Default,
                Secondary = Colors.Grey.Lighten5
                //AppbarBackground = Colors.,
            },
            PaletteDark = new Palette()
            {
                Primary = Colors.Blue.Lighten1
            },

            LayoutProperties = new LayoutProperties()
            {
                DrawerWidthLeft = "260px",
                DrawerWidthRight = "300px"
            }
        };
    }
}
