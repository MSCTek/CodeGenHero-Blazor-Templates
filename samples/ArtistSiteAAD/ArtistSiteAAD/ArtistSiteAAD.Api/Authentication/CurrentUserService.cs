using ArtistSiteAAD.Shared.Authentication;
using Microsoft.AspNetCore.Http;

namespace ArtistSiteAAD.Api.Authentication
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IUserSession GetCurrentUser()
        {
            if (_httpContextAccessor?.HttpContext == null)
            {
                return new UserSession();
            }

            var user = _httpContextAccessor.HttpContext.User;
            string userName = user.Identity.Name;
            var userNameClaim = user.Claims.FirstOrDefault(claim => claim.Type.Equals("name", StringComparison.InvariantCultureIgnoreCase));
            if (userNameClaim != null)
            {   // Allow the claim value to override what may be in the Identity.Name property.
                userName = userNameClaim.Value;
            }

            IUserSession currentUser = new UserSession
            {
                IsAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated,
                LoginName = userName
            };

            return currentUser;
        }
    }
}
