using ArtistSite.Shared.Constants;

namespace ArtistSite.App.Shared
{
    public class AdminPageBase : CGHComponentBase
    {
        // used to store state of screen
        protected string Message = string.Empty;

        protected string StatusClass = string.Empty;

        protected int CurrentUserId
        {
            get
            {
                int retVal = 0;
                var userIdClaim = GetClaimsPrincipalClaim(User, Consts.CLAIM_USERID);
                if (!string.IsNullOrWhiteSpace(userIdClaim))
                {
                    if (int.TryParse(userIdClaim, out int userId))
                    {
                        retVal = userId;
                    }
                }

                return retVal;
            }
        }

        protected bool IsReady { get; set; }

        protected bool Saved { get; set; }

        protected void ClearMessage()
        {
            Message = string.Empty;
        }
    }
}