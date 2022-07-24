namespace $safeprojectname$.Pages
{
    using Microsoft.AspNetCore.Components;
    using $saferootprojectname$.App.ViewModels;

    public class AuthenticationViewModel : BaseViewModel
    {
        [Parameter]
        public string Action { get; set; }
    }
}