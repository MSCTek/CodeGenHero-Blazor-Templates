namespace $safeprojectname$.Services
{
    using System.Security.Claims;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
    using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;

    public class CustomAccountFactory
        : AccountClaimsPrincipalFactory<RemoteUserAccount>
    {
        public CustomAccountFactory(NavigationManager navigationManager,
            IAccessTokenProviderAccessor accessor) : base(accessor)
        {
        }

        public override async ValueTask<ClaimsPrincipal> CreateUserAsync(
            RemoteUserAccount account, RemoteAuthenticationUserOptions options)
        {
            var user = await base.CreateUserAsync(account, options);
            if (account == null)
                return user;

            account.AdditionalProperties.TryGetValue("groups", out var groupsClaim);
            var claimsIdentity = user.Identity as ClaimsIdentity;
            if (claimsIdentity != null
                && groupsClaim != null
                && groupsClaim is JsonElement element
                && element.ValueKind == JsonValueKind.Array)
            {
                claimsIdentity.RemoveClaim(claimsIdentity.FindFirst("groups"));
                var claims = element.EnumerateArray().Select(x => new Claim("groups", x.ToString())).ToList();
                claimsIdentity.AddClaims(claims);
            }

            return user;
        }
    }
}
