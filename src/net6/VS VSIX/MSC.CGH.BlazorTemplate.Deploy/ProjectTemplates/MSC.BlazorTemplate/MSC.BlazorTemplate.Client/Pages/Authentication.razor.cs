namespace $safeprojectname$.Pages
{
    using Microsoft.AspNetCore.Components;
    using $safeprojectname$.ViewModels;

    public class AuthenticationViewModel : BaseViewModel
    {
        [Parameter]
        public string Action { get; set; }
    }
}