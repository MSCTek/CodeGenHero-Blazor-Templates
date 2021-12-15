using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Inflector;
using CodeGenHero.Template.Helpers;
using CodeGenHero.Template.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenHero.Template.Blazor5.Generators
{
    class AdminListPageGenerator : BaseBlazorGenerator
    {
        public AdminListPageGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        public string Generate(
            IEntityType entity,
            string adminListPageViewModelClassName)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(GeneratePageStart(entity, adminListPageViewModelClassName));
            sb.Append(GenerateIfIsReady());
            sb.Append(GenerateIfNotSaved(entity));
            sb.Append(GenerateIfSaved());
            sb.Append(GenerateIfMessageExists());

            sb.AppendLine("\n</div>");

            return sb.ToString();
        }

        private string GeneratePageStart(IEntityType entity, string adminListPageViewModelClassName)
        {
            var entityName = entity.ClrType.Name;
            var entityNamePlural = Inflector.Pluralize(entityName);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"@page \"/admin/{entityNamePlural.ToLower()}\"");
            sb.AppendLine($"@inherits {adminListPageViewModelClassName}");
            sb.AppendLine("<div class=\"mud-palette-override\"> @* This outer div is necessary CSS Isolation to function *@\n");

            return sb.ToString();
        }

        private string GenerateIfIsReady()
        {
            StringBuilder sb = new StringBuilder();

            var ifIsReadyLiteral = @"
    @if (!IsReady)
    {
        <MudText>
            Loading....
        </MudText>
    }";

            sb.AppendLine(ifIsReadyLiteral);
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateIfNotSaved(IEntityType entity)
        {
            var entityName = entity.ClrType.Name;
            var entityNamePlural = Inflector.Pluralize(entityName);
            var primaryKeys = GetPrimaryKeys(entity);

            IndentingStringBuilder sb = new IndentingStringBuilder(1);

            sb.AppendLine("@if (!Saved)");
            sb.AppendLine("{");

            sb.AppendLine($"\t<MudTable Items=\"@{entityNamePlural}\" Dense=\"@Dense\" Hover=\"@Hover\" Bordered=\"@Bordered\" Striped=\"@Striped\"");
            sb.AppendLine("\t\tRowsPerPage=\"7\"");
            sb.AppendLine($"\t\tFilter=\"new Func<{entityName}, bool>(Filter{entityName}1)\" @bind-SelectedItem=\"Selected{entityName}\">");

            sb.AppendLine("\t\t<ToolBarContent>");
            sb.AppendLine($"\t\t\t<MudText Typo=\"Typo.h6\">{entityNamePlural}</MudText>");
            sb.AppendLine($"\t\t\t<MudSpacer />");
            sb.AppendLine("\t\t\t<MudTextField @bind-Value=\"SearchString1\" Placeholder=\"Search\" Adornment=\"Adornment.Start\" AdornmentIcon=\"@Icons.Material.Filled.Search\" IconSize=\"Size.Medium\" Class=\"mt-0\" />");
            sb.AppendLine("\t\t</ToolBarContent>");

            sb.AppendLine("\t\t<HeaderContent>");
            foreach (var pk in primaryKeys)
            {
                sb.AppendLine($"\t\t\t<MudTh>{pk}</MudTh>");
            }
            sb.AppendLine("\t\t\t<MudTh>Is Active</MudTh>");
            sb.AppendLine("\t\t\t<MudTh>&nbsp;</MudTh>");
            sb.AppendLine("\t\t</HeaderContent>");

            sb.AppendLine("\t\t<RowTemplate>");
            foreach (var pk in primaryKeys)
            {
                sb.AppendLine($"\t\t\t<MudTd DataLabel=\"{pk}\">@context.{pk}</MudTd>");
            }
            sb.AppendLine("\t\t\t<MudTd DataLabel=\"Actions\">");
            sb.AppendLine("\t\t\t\t<MudButton @onclick=\"@(()=>ConfirmDeleteAsync(context))\" Variant=\"Variant.Filled\" Color=\"Color.Error\" Style=\"height: 38px; min-width: 44px;\">");
            sb.AppendLine("\t\t\t\t\t<i class=\"fas fa-trash-alt\"></i>");
            sb.AppendLine("\t\t\t\t</MudButton>");
            sb.AppendLine("\t\t\t</MudTd>");
            sb.AppendLine("\t\t</RowTemplate>");

            sb.AppendLine("\t\t<PagerContent>");
            sb.AppendLine("\t\t\t<MudTablePager />");
            sb.AppendLine("\t\t</PagerContent>");
            sb.AppendLine("\t</MudTable>");

            sb.Append("}");

            return sb.ToString();
        }

        private string GenerateIfSaved()
        {
            var ifSavedLiteral = @"
	else
    {
        <div>
            <a class=""btn btn-secondary m-2"" @onclick=""@ReturnToList"">
                Return to list
            </a>
        </div>
    }";

            return ifSavedLiteral;
        }

        private string GenerateIfMessageExists()
        {
            var ifMessageExistsLiteral = @"
    @if (!string.IsNullOrWhiteSpace(Message))
    {
        <div class=""alert @StatusClass"">
            @Message
                <MudButton @onclick=""@ClearMessage"" Variant=""Variant.Filled"" Color=""Color.Error"" Style=""height: 12px; width: 12px; min-width: 12px; padding: 1px; top: -6px;"">
                    <i class=""fas fa-times""></i>
                </MudButton>
        </div>
    }";

            return ifMessageExistsLiteral;
        }
    }
}
