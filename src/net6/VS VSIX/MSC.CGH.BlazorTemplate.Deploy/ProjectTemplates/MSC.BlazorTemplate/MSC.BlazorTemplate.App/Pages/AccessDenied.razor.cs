using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using $safeprojectname$.Services;
using $safeprojectname$.Shared;
using System.Threading.Tasks;

namespace $safeprojectname$.Pages
{
    public partial class AccessDeniedViewModel : MSCComponentBase
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