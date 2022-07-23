using ArtistSite.App.Services;
using ArtistSite.App.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;

namespace ArtistSite.App.Pages
{
    public partial class AccessDeniedViewModel : CGHComponentBase
    {
        [Inject]
        protected ITokenManager TokenManager { get; set; }

        [Inject]
        protected ITokenProvider TokenProvider { get; set; }

        protected async Task BeginSignOut(MouseEventArgs args)
        {
            await TokenManager.SignOutAsync();
        }
    }
}