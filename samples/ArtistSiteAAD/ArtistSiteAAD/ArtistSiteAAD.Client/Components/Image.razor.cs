using ArtistSiteAAD.Client.Shared;
using Microsoft.AspNetCore.Components;

namespace ArtistSiteAAD.Client.Components
{
    public class ImageViewModel : CGHComponentBase
    {
        [Parameter]
        public bool UseBaseAddress { get; set; } = true;

        private string _source = string.Empty;
        [Parameter]
        public string Source
        {
            get
            {
                if (!UseBaseAddress)
                    return _source;
                else
                    return $"{CGHAppSettings.ImagesBaseAddress}{_source}";
            }
            set
            {
                _source = value;
            }
        }

        [Parameter]
        public string Style { get; set; } = string.Empty;
    }
}
