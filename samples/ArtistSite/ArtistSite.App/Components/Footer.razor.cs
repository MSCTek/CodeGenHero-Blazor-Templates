using ArtistSite.App.Services;
using ArtistSite.App.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace ArtistSite.App.Components
{
    public partial class FooterViewModel : CGHComponentBase
    {
        [Inject]
        protected IConfiguration Config { get; set; }

        protected string RegisterUrl
        {
            get
            {
                string retVal = $"{Config?["OidcConfiguration:Authority"]}/Identity/Account/Register"; // https://localhost:5401/Identity/Account/Register

                return retVal;
            }
        }

        [Inject]
        protected ITokenManager TokenManager { get; set; }

        [Inject]
        protected ITokenProvider TokenProvider { get; set; }

        protected async Task BeginSignOut(MouseEventArgs args)
        {
            await TokenManager.SignOutAsync();
        }

        protected async Task WriteToken()
        {
            var accessToken = await TokenManager.RetrieveAccessTokenAsync();
            System.Diagnostics.Debug.WriteLine(accessToken);
        }
    }
}