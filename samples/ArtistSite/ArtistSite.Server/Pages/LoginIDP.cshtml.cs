using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace ArtistSite.Server.Pages
{
    public class LoginIDPModel : PageModel
    {
        //[BindProperty(SupportsGet = true)]
        //public string RedirectUri { get; set; }

        //private static string _saveRedirectUri = Guid.NewGuid().ToString();

        public async Task OnGetAsync(string redirectUri)
        {
            if (string.IsNullOrWhiteSpace(redirectUri)) //|| (redirectUri == _saveRedirectUri && HttpContext.User.Identity.IsAuthenticated))
            {
                redirectUri = Url.Content("~/");
            }

            //if (HttpContext.User.Identity.IsAuthenticated)
            //{
            //    // Avoid a potentially-endless redirection loop if a user is on a page they are not authorized to view and they log in.
            //    // For example, an artist trying to view the /admin/artists page, which requires the WA_ADMIN role.
            //    _saveRedirectUri = redirectUri;

            //    Response.Redirect(redirectUri);
            //}

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                // If the user hitting the login page is already authenticated, chances are that they got redirected here
                // by the App.razor page's NotAuthorized path. The user may have tried to directly access a page
                // for which they do not have access.

                Response.Redirect("/Authorization/AccessDenied");
            }
            else
            {
                await HttpContext.ChallengeAsync(
                   OpenIdConnectDefaults.AuthenticationScheme,
                   new AuthenticationProperties { RedirectUri = redirectUri });
            }
        }
    }
}