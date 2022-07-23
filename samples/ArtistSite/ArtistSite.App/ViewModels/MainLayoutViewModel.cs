using ArtistSite.App.Services;
using ArtistSite.Shared.DTO;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtistSite.App.ViewModels
{
    public class MainLayoutViewModel : LayoutComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public INavigationService NavigationService { get; set; }

        [Inject]
        public IOptions<CGHAppSettings> CGHAppSettingsOptions { get; set; }

        public CGHAppSettings CGHAppSettings
        {
            get
            {
                return CGHAppSettingsOptions.Value;
            }
        }

        public List<BreadcrumbItem> Breadcrumbs { get; set; }

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

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
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
    }
}
