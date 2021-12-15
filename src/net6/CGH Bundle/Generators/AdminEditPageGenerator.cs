using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Inflector;
using CodeGenHero.Template.Helpers;
using CodeGenHero.Template.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenHero.Template.Blazor5.Generators
{
    class AdminEditPageGenerator : BaseBlazorGenerator
    {

        public AdminEditPageGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        public string Generate(
            IEntityType entity,
            string adminEditViewModelClassName)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(GeneratePageStart(entity, adminEditViewModelClassName));
            sb.Append(GenerateIfIsReady());
            sb.Append(GenerateIfNotSaved(entity));
            sb.Append(GenerateIfSaved());
            sb.Append(GenerateIfMessage());
            sb.AppendLine("</div>");

            return sb.ToString();
        }

        private string GeneratePageStart(IEntityType entity, string adminEditViewModelClassName)
        {
            var entityName = entity.ClrType.Name;
            var primaryKeyNames = GetPrimaryKeys(entity);
            var properties = entity.GetProperties();
            var pkCount = primaryKeyNames.Count();

            StringBuilder routeParametersSB = new StringBuilder();

            foreach (var primaryKey in primaryKeyNames)
            {
                var property = properties.Where(x => x.Name == primaryKey).FirstOrDefault();
                if (property == null)
                {
                    continue;
                }
                var cType = GetCType(property);
                var simpleType = ConvertToSimpleType(cType);
                var camelPK = Inflector.Camelize(primaryKey);

                routeParametersSB.Append($"{{{primaryKey}:{simpleType}}}");
                pkCount--;

                if (pkCount > 0)
                {
                    routeParametersSB.Append("/");
                }
            }

            string routeParameters = routeParametersSB.ToString();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"@page \"/admin/{entityName.ToLower()}edit/{routeParameters}\"");
            sb.AppendLine($"@inherits {adminEditViewModelClassName}");
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
            var primaryKeys = GetPrimaryKeys(entity);

            string expansionPanelLiteral = @"
            <MudExpansionPanels>
                <MudExpansionPanel Text=""Show Validation Summary"">
                    @if (Saved)
                    {
                        <MudText Color=""Color.Success"">Success</MudText>
                    }
                    else
                    {
                        <MudText Color=""@Color.Error"">
                            <ValidationSummary />
                        </MudText>
                    }
                </MudExpansionPanel>
            </MudExpansionPanels>";

            IndentingStringBuilder sb = new IndentingStringBuilder(1);

            sb.AppendLine("@if (!Saved)");
            sb.AppendLine("{");

            sb.AppendLine($"\t<section class=\"{entityName.ToLower()}\">");
            sb.AppendLine($"\t<MudText Typo=\"Typo.h6\" GutterBottom=\"true\">@{entityName}?.{primaryKeys[0]}</MudText> @* Replace with an appropriate identifying string property *@"); // \"\"
            sb.AppendLine($"\t<EditForm Model=\"@{entityName}\" OnValidSubmit=\"OnValidSubmit\">");

            sb.AppendLine("\t\t<DataAnnotationsValidator />");
            sb.AppendLine("\t\t<MudCard>");

            sb.AppendLine("\t\t\t<MudCardContent>");
            sb.AppendLine("\t\t\t\t<label for=\"\">Your Primary Key Here</label>");
            sb.AppendLine("\t\t\t\t@* Add your editor controls for editing the object here, within the MudCardContent *@");
            sb.AppendLine("\t\t\t</MudCardContent>");

            sb.AppendLine("\t\t\t<MudCardActions>");
            sb.AppendLine("\t\t\t\t<MudButton ButtonType=\"ButtonType.Submit\" Variant=\"Variant.Filled\" Color=\"Color.Primary\" Class=\"ml-auto\">Save</MudButton>");
            sb.AppendLine("\t\t\t\t<a class=\"btn btn-secondary m-2\" @onclick=\"@ReturnToList\">");
            sb.AppendLine("\t\t\t\t\tCancel");
            sb.AppendLine("\t\t\t\t</a>");
            sb.AppendLine("\t\t\t</MudCardActions>");

            sb.AppendLine("\t\t</MudCard>");

            sb.AppendLine("\t\t<MudText Typo=\"Typo.body2\" Align=\"Align.Center\" Class=\"my-4\">");
            sb.AppendLine("\t\t\tFill out the form correctly to see the success message.");
            sb.AppendLine("\t\t</MudText>");

            sb.Append(expansionPanelLiteral);
            sb.AppendLine("\t</EditForm>");
            sb.AppendLine("\t</section>");

            sb.AppendLine("}");

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

        private string GenerateIfMessage()
        {
            var ifMessageLiteral = @"
@if (!string.IsNullOrWhiteSpace(Message))
    {
        <div class=""alert @StatusClass"">
            @Message
                <MudButton @onclick=""@ClearMessage"" Variant=""Variant.Filled"" Color=""Color.Error"" Style=""height: 12px; width: 12px; min-width: 12px; padding: 1px; top: -6px;"">
                    <i class=""fas fa-times""></i>
                </MudButton>
        </div>
    }";

            return ifMessageLiteral;
        }

    }
}
