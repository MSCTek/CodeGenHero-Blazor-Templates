using ArtistSite.Shared.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ArtistSite.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestAuthController : Controller
    {
        public TestAuthController()
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var retVal = await WriteOutIdentityInformation(); // "Congrats, you are authorized!";

            return Ok(retVal);
        }

        [Authorize(Roles = Consts.ROLE_ADMIN)]
        [HttpGet("TestAdmin")]
        public IActionResult TestAdmin()
        {
            var retVal = "Congrats, you have the ADMIN role!";

            return Ok(retVal);
        }

        [Authorize(Roles = Consts.ROLE_ADMIN_OR_USER)]
        [HttpGet("TestAdminOrUser")]
        public IActionResult TestAdminOrUser()
        {
            var retVal = "Congrats, you have either the ADMIN or the USER role!";

            return Ok(retVal);
        }

        [Authorize(Roles = Consts.ROLE_USER)]
        [HttpGet("TestUser")]
        public IActionResult TestUser()
        {
            var retVal = "Congrats, you have the USER role!";

            return Ok(retVal);
        }

        private async Task<string> WriteOutIdentityInformation()
        {
            StringBuilder sb = new StringBuilder();
            // get the saved identity token
            var identityToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);

            // write it out
            sb.AppendLine($"Identity token: {identityToken}");

            // write out the user claims
            foreach (var claim in User.Claims)
            {
                sb.AppendLine($"Claim type: {claim.Type} - Claim value: {claim.Value}");
            }

            Debug.Write(sb.ToString());
            return sb.ToString();
        }
    }
}