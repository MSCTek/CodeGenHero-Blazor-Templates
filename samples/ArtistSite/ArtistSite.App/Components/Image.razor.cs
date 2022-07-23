using ArtistSite.App.Shared;
using Microsoft.AspNetCore.Components;

namespace ArtistSite.App.Components
{
    public class ImageViewModel : CGHComponentBase
    {
        private string source = string.Empty;

        [Parameter]
        public string Source
        {
            get
            {
                return $"{ImagesBaseAddress}{source}";
            }
            set
            {
                source = value;
            }
        }

        [Parameter]
        public string Class { get; set; } = "py-4";

        [Parameter]
        public string Style { get; set; } = "max-width: 50%; max-height: auto;";
    }
}
