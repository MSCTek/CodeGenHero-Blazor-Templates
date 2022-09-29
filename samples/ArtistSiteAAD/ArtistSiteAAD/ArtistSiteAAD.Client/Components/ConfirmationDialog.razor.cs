using Microsoft.AspNetCore.Components;
using ArtistSiteAAD.Client.Shared;
using MudBlazor;

namespace ArtistSiteAAD.Client.Components
{
    public partial class ConfirmationDialogViewModel : CGHComponentBase
    {
        [Parameter] public string ButtonText { get; set; } = string.Empty;

        [Parameter] public Color Color { get; set; }

        [Parameter] public string ContentText { get; set; } = string.Empty;

        [CascadingParameter]
        private MudDialogInstance MudDialog { get; set; } = default!;

        protected void Cancel() => MudDialog.Cancel();

        protected void Submit() => MudDialog.Close(DialogResult.Ok(true));
    }
}