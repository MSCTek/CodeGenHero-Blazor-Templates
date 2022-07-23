using ArtistSite.App.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ArtistSite.App.Components
{
    public partial class ConfirmationDialogViewModel : CGHComponentBase
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