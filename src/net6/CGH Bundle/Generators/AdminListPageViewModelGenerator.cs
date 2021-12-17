using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Inflector;
using CodeGenHero.Template.Helpers;
using CodeGenHero.Template.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenHero.Template.Blazor5.Generators
{
    class AdminListPageViewModelGenerator : BaseBlazorGenerator
    {
        public AdminListPageViewModelGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        public string Generate(
            List<NamespaceItem> usings,
            string classNamespace,
            string namespacePostfix,
            IEntityType entity,
            string className,
            string webApiDataServiceInterfaceClassName,
            string webApiDataServiceClassName)
        {
            var entityName = $"{entity.ClrType.Name}";
            var pluralizedEntityName = Inflector.Pluralize(entityName);
            var methodSignature = GetMethodParameterSignature(entity);
            var methodSignatureUntyped = GetMethodParametersWithoutTypes(entity);
            var primaryKeys = GetPrimaryKeys(entity);

            StringBuilder sb = new StringBuilder();

            sb.Append(GenerateHeader(usings, classNamespace));

            sb.AppendLine("\t[Authorize(Roles = Consts.ROLE_ADMIN_OR_USER)]");
            sb.AppendLine($"\tpublic partial class {className} : AdminPageBase");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tpublic {className}()");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t}");
            sb.AppendLine(string.Empty);

            sb.Append(GenerateProperties(entityName, pluralizedEntityName, webApiDataServiceInterfaceClassName, webApiDataServiceClassName));
            sb.Append(GenerateDeleteMethods(entityName, methodSignature, methodSignatureUntyped, webApiDataServiceClassName, primaryKeys));
            sb.Append(GenerateFilterFunction(entityName, primaryKeys));
            sb.Append(GenerateOnInitializedAsync(pluralizedEntityName, webApiDataServiceClassName));
            sb.Append(GenerateCommonMethods(pluralizedEntityName));

            sb.Append(GenerateFooter());

            return sb.ToString();
        }

        private string GenerateProperties(string entityName, string pluralizedEntityName, string webApiDataServiceInterfaceClassName, string webApiDataServiceClassName)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"public IList<{entityName}> {pluralizedEntityName} {{ get; set; }} = new List<{entityName}>();");
            sb.AppendLine(string.Empty);

            sb.AppendLine("[Inject]");
            sb.AppendLine($"public {webApiDataServiceInterfaceClassName} {webApiDataServiceClassName} {{ get; set; }}");
            sb.AppendLine(string.Empty);

            sb.AppendLine("protected bool Bordered { get; set; } = false;");
            sb.AppendLine("protected bool Dense { get; set; } = false;");
            sb.AppendLine("protected bool Hover { get; set; } = true;");
            sb.AppendLine("protected bool Striped { get; set; } = true;");
            sb.AppendLine(string.Empty);

            sb.AppendLine("[Inject]");
            sb.AppendLine("protected ILocalHttpClientService LocalHttpClientService { get; set; }");
            sb.AppendLine(string.Empty);

            sb.AppendLine("protected string SearchString1 { get; set; } = \"\";");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"protected {entityName} Selected{entityName} {{ get; set; }}");
            sb.AppendLine(string.Empty);

            sb.AppendLine("[Inject]");
            sb.AppendLine("private IDialogService DialogService { get; set; }");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateDeleteMethods(string entityName, string methodSignature, string methodSignatureUntyped, string webApiDataServiceClassName, List<string> primaryKeys)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(GenerateConfirmDelete(entityName, primaryKeys));
            sb.Append(GenerateDeleteEntity(entityName, methodSignature, methodSignatureUntyped, webApiDataServiceClassName));

            return sb.ToString();
        }

        #region Delete Method Generators

        private string GenerateConfirmDelete(string entityName, List<string> primaryKeys)
        {
            StringBuilder deleteSignatureSB = new StringBuilder();
            var pkCount = primaryKeys.Count();
            foreach (var pk in primaryKeys)
            {
                deleteSignatureSB.Append($"item.{pk}");
                pkCount--;
                if (pkCount > 0)
                {
                    deleteSignatureSB.Append(", ");
                }
            }
            var deleteSignature = deleteSignatureSB.ToString();

            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"protected async Task ConfirmDeleteAsync({entityName} item)");
            sb.AppendLine("{");

            sb.AppendLine("\tvar parameters = new DialogParameters();");
            sb.AppendLine("\tparameters.Add(\"ContentText\", $\"Are you sure you want to delete this?\");");
            sb.AppendLine("\tparameters.Add(\"ButtonText\", \"Yes\");");
            sb.AppendLine("\tparameters.Add(\"Color\", Color.Success);");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\tvar result = await DialogService.Show<ConfirmationDialog>(\"Confirm\", parameters).Result;");
            sb.AppendLine("\tif (!result.Cancelled)");
            sb.AppendLine("\t{");

            sb.AppendLine($"\t\tawait Delete{entityName}Async({deleteSignature});");

            sb.AppendLine("\t}");
            
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateDeleteEntity(string entityName, string methodSignature, string methodSignatureUntyped, string webApiDataServiceClassName)
        {
            string successCodeLiteral = @"
                    StatusClass = ""alert-success"";
                    Message = ""Deleted successfully"";
                    await SetSavedAsync(true);";

            string failCodeLiteral = @"
                    StatusClass = ""alert-danger"";
                    Message = ""Something went wrong deleting the item. Please try again."";
                    await SetSavedAsync(false);";

            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"protected async Task Delete{entityName}Async({methodSignature})");
            sb.AppendLine("{");

            sb.AppendLine($"\tvar result = await {webApiDataServiceClassName}.Delete{entityName}Async({methodSignatureUntyped});");
            sb.AppendLine($"\tif (result.IsSuccessStatusCode)");    // Remember to include a Commented Out reference to the WebApiDataService in the MPT's Component-Base for the end-dev to hand-populate.
            sb.AppendLine("\t{");

            sb.Append(successCodeLiteral);
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t}");
            sb.AppendLine("\telse");
            sb.AppendLine("\t{");

            sb.Append(failCodeLiteral);
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t}");

            sb.AppendLine("}");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        #endregion

        private string GenerateFilterFunction(string entityName, List<string> primaryKeys)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"protected bool Filter{entityName}1({entityName} item) => FilterFunc(item, SearchString1);");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"protected bool FilterFunc({entityName} item, string searchString)");
            sb.AppendLine("{");

            sb.AppendLine("\tif (string.IsNullOrWhiteSpace(searchString))");
            sb.AppendLine("\t\treturn true;");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\tsearchString = searchString.Trim();");
            foreach(var pk in primaryKeys)
            {
                var pkStringName = $"{pk}String";

                sb.AppendLine("\t// Replace with the property you intend a search to work against");
                sb.AppendLine($"\tvar {pkStringName} = item.{pk}.ToString();");
                sb.AppendLine($"\tif (!string.IsNullOrWhiteSpace({pkStringName}) && {pkStringName}.Contains(searchString, StringComparison.OrdinalIgnoreCase))");
                sb.AppendLine("\t\treturn true;");
                sb.AppendLine(string.Empty);
            }

            sb.AppendLine("\treturn false;");

            sb.AppendLine("}");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateOnInitializedAsync(string pluralizedEntityName, string webApiDataServiceClassName)
        {
            string lowerEntityPlural = Inflector.ToLowerFirstCharacter(pluralizedEntityName);

            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine("protected override async Task OnInitializedAsync()");
            sb.AppendLine("{");

            sb.AppendLine("\tawait base.OnInitializedAsync();");
            sb.AppendLine("\tIsReady = false;");
            sb.AppendLine("\tawait SetSavedAsync(false);");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\ttry");
            sb.AppendLine("\t{");

            sb.AppendLine("\t\tList<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>()");
            sb.AppendLine("\t\t{");

            sb.AppendLine("\t\t\tnew BreadcrumbItem(\"Home\", \"/\"),");
            sb.AppendLine($"\t\t\tnew BreadcrumbItem(\"List of {pluralizedEntityName}\", \"/admin/{lowerEntityPlural}\", disabled: true)");

            sb.AppendLine("\t\t};");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\t\tNavigationService.SetBreadcrumbs(breadcrumbs);");

            sb.AppendLine($"\t\tif ({pluralizedEntityName} == null || !{pluralizedEntityName}.Any())");
            sb.AppendLine("\t\t{");

            sb.AppendLine("\t\t\tif (User != null && User.Identity.IsAuthenticated)");
            sb.AppendLine("\t\t\t{");

            sb.AppendLine("\t\t\t\t// Add your filtering logic in here, by adding FilterCriterion to the filterCriteria list");
            sb.AppendLine("\t\t\t\tvar filterCriteria = new List<IFilterCriterion>();");

            sb.AppendLine($"\t\t\t\t{pluralizedEntityName} = await {webApiDataServiceClassName}.GetAllPages{pluralizedEntityName}Async();");

            sb.AppendLine("\t\t\t}");

            sb.AppendLine("\t\t}");

            sb.AppendLine("\t}");
            sb.AppendLine("\tfinally");
            sb.AppendLine("\t{");

            sb.AppendLine("\t\tIsReady = true;");

            sb.AppendLine("\t}");

            sb.AppendLine("}");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateCommonMethods(string pluralizedEntityName)
        {
            var lowerPlural = Inflector.ToLowerFirstCharacter(pluralizedEntityName);

            var setSavedLiteral = @"
        protected async Task SetSavedAsync(bool value)
        {
            Saved = value;
            if (value == true)
            {
                await JsRuntime.InvokeVoidAsync(""blazorExtensions.ScrollToTop"");
            }
        }";

            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine("protected void ReturnToList()");
            sb.AppendLine("{");

            sb.AppendLine($"\tNavigationManager.NavigateTo(\"/admin/{lowerPlural}\");");

            sb.AppendLine("}");

            sb.AppendLine(setSavedLiteral);
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }
    }
}
