using ArtistSiteAAD.Client.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Text;

namespace ArtistSiteAAD.Client.Pages
{
    public partial class AuthTestViewModel : BasePageViewModel
    {
        [Inject]
        protected IWebAssemblyHostEnvironment Env { get; set; } = default!;


        protected string? IdentityInformation;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                IdentityInformation = await GetIdentityInformationAsync();
            }
        }

        private async Task<string> GetIdentityInformationAsync()
        {
            StringBuilder sb = new StringBuilder();

            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;


            sb.AppendLine($"You are running in the {Env.Environment} environment.");


            if ((user?.Identity?.IsAuthenticated ?? false) == true)
            {
                sb.AppendLine($"{user?.Identity?.Name} is authenticated.");
                var claims = user.Claims.ToList();

                foreach (var claim in claims)
                {
                    sb.AppendLine($"Claim type: {claim.Type} - Claim value: {claim.Value}\n");
                }
            }

            // Manual checks
            sb.AppendLine($"\n\n");
            sb.AppendLine($"IsAdmin: {base.IsAdmin}");
            sb.AppendLine($"HasUserAccess: {base.HasUserAccess}");

            Console.WriteLine($"User claims information:\n {sb.ToString()}");

            return sb.ToString();
        }
    }
}
