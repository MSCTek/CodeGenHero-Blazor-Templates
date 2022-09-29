using ArtistSiteAAD.Client.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ArtistSiteAAD.Client.Shared
{
    public class ApplicationLayoutViewModel : LayoutComponentBase
    {
        public List<BreadcrumbItem> Breadcrumbs { get; set; } = new List<BreadcrumbItem>();

        [Inject]
        public INavigationService NavigationService { get; set; }

        protected bool IsReady { get; set; }

        protected bool IsServerSide { get; set; } = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            // Only run if the task failed in on init (i.e. We're using client-side Wasm, not server-side Blazor)
            if (IsServerSide && firstRender)
            {
                IsReady = true;
                await InvokeAsync(StateHasChanged);
            }
        }

        protected override async Task OnInitializedAsync()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                NavigationService.OnBreadcrumbsChanged += OnBreadcrumbsChanged;
                Breadcrumbs = NavigationService.GetBreadcrumbs();

                IsReady = true;
            }
            catch (Exception)
            {
                IsServerSide = true; // Javascript was not ready so when it fails we handle it in OnAfterRenderAsync()
            }
        }

        private void OnBreadcrumbsChanged()
        {
            Breadcrumbs = NavigationService.GetBreadcrumbs();
            StateHasChanged();
        }

        protected MudTheme ApplicationCustomTheme = new MudTheme()
        {
            Palette = new Palette()
            {
                Primary = Colors.Blue.Default,
                Secondary = Colors.Grey.Darken2
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
