using ArtistSite.App.ViewModels;
using ArtistSite.Shared.DTO;
using MudBlazor;

namespace ArtistSite.App.Pages
{
    public class ArtistDetailViewModel : BaseDetailPageViewModel
    {
        public string ProfileImageUrl
        {
            get
            {
                return $"Images/Profile/{Artist.ArtistId}.jpg";
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            if (Artist?.ArtistId == ArtistId)
                return;

            await PopulateArtistAsync(getArtworks: true);

            List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>()
                {
                    new BreadcrumbItem("Home", "/"),
                    new BreadcrumbItem("Browse Artists", "/Artists"),
                    new BreadcrumbItem(Artist.Name, $"/Artists/{Artist.ArtistId}", disabled: true)
                };

            NavigationService.SetBreadcrumbs(breadcrumbs);
        }
    }
}
