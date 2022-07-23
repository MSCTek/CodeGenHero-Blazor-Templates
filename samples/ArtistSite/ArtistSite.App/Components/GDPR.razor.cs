using ArtistSite.App.Shared;
using Majorsoft.Blazor.Components.GdprConsent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtistSite.App.Components
{
    public partial class GDPRViewModel : CGHComponentBase
    {
        protected GdprBanner _gdprBanner;

        protected string _gdprBannerColor = "#27272f"; // "lightblue"; // "var(--mud-palette-appbar-background)";
        protected int _gdprBannerConsentValidDays = 365;
        protected double _gdprBannerOpacity = 90;
        protected List<GdprConsentDetail> _gdprConsents;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        protected override async Task OnInitializedAsync()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            _gdprConsents = new List<GdprConsentDetail>()
            {
                new GdprConsentDetail() { ConsentName = "Required", IsAccepted = true },
                new GdprConsentDetail() { ConsentName = "Session", IsAccepted = true },
                new GdprConsentDetail() { ConsentName = "Tracking", IsAccepted = true },
            };
        }
    }
}