using Microsoft.AspNetCore.Components;
using $safeprojectname$.Shared;
using MudBlazor;

namespace $safeprojectname$.Components
{
    public partial class ConfirmationDialogViewModel : MSCComponentBase
    {
        [Parameter] public string ButtonText { get; set; }

        [Parameter] public Color Color { get; set; }

        [Parameter] public string ContentText { get; set; }

        [CascadingParameter]
        private MudDialogInstance MudDialog { get; set; }

        protected void Cancel() => MudDialog.Cancel();

        protected void Submit() => MudDialog.Close(DialogResult.Ok(true));
    }
}