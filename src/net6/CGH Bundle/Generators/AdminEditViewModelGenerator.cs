using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Inflector;
using CodeGenHero.Template.Helpers;
using CodeGenHero.Template.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenHero.Template.Blazor6.Generators
{
    public sealed class AdminEditViewModelGenerator : BaseBlazorGenerator
    {
        public AdminEditViewModelGenerator(ICodeGenHeroInflector inflector) : base(inflector)
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
            StringBuilder sb = new StringBuilder();

            sb.Append(GenerateHeader(usings, classNamespace));

            sb.AppendLine("\t[Authorize(Roles = Consts.ROLE_ADMIN_OR_USER)]");
            sb.AppendLine($"\tpublic partial class {className} : AdminPageBase");
            sb.AppendLine("\t{");

            sb.Append(GenerateProperties(entity, webApiDataServiceInterfaceClassName, webApiDataServiceClassName));
            sb.Append(GenerateOnInitialized(entity));
            sb.Append(GenerateOnParametersSet(entity, webApiDataServiceClassName));
            sb.Append(GenerateOnValidSubmit(entity, webApiDataServiceClassName));
            sb.Append(GenerateSmallMethods(entity));

            sb.AppendLine(GenerateFooter());
            return sb.ToString();
        }

        private string GenerateProperties(IEntityType entity, string webApiDataServiceInterfaceClassName, string webApiDataServiceClassName)
        {
            var entityName = entity.ClrType.Name;
            var primaryKeys = GetPrimaryKeys(entity);
            var properties = entity.GetProperties();

            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"public {entityName} {entityName} {{ get; set; }} = new {entityName}();");
            sb.AppendLine(string.Empty);

            sb.AppendLine("[Inject]");
            sb.AppendLine($"public {webApiDataServiceInterfaceClassName} {webApiDataServiceClassName} {{ get; set; }}");
            sb.AppendLine(string.Empty);

            foreach (var property in properties)
            {
                string propertyName = property.Name;
                bool isPrimaryKey = primaryKeys.Any(x => x.Equals(propertyName));

                if (isPrimaryKey)
                {
                    string cType = GetCType(property);
                    var simpleType = ConvertToSimpleType(cType);

                    sb.AppendLine("[Parameter]");
                    sb.AppendLine($"public {simpleType} {Inflector.Pascalize(propertyName)} {{ get; set; }}");
                    sb.AppendLine(string.Empty);
                }
            }

            return sb.ToString();
        }

        private string GenerateOnInitialized(IEntityType entity)
        {
            var entityName = entity.ClrType.Name;
            var entityNamePlural = Inflector.Pluralize(entityName);
            var entityNamePluralLower = Inflector.ToLowerFirstCharacter(entityNamePlural);
            var primaryKeys = GetPrimaryKeys(entity);

            StringBuilder pageParametersSB = new StringBuilder();
            var pkCount = primaryKeys.Count();
            foreach (var pk in primaryKeys)
            {
                pageParametersSB.Append($"{{{pk}}}");
                pkCount--;
                if (pkCount > 0)
                {
                    pageParametersSB.Append("/");
                }
            }
            var pageParameters = pageParametersSB.ToString();

            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine("protected override async Task OnInitializedAsync()");
            sb.AppendLine("{");

            sb.AppendLine("\tawait base.OnInitializedAsync();");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\tList<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>()");
            sb.AppendLine("\t{");

            sb.AppendLine("\t\tnew BreadcrumbItem(\"Home\", \"/\"),");
            sb.AppendLine($"\t\tnew BreadcrumbItem(\"List of {entityNamePlural}\", \"/admin/{entityNamePluralLower}\"),");
            sb.AppendLine($"\t\tnew BreadcrumbItem(\"Edit {entityName}\", $\"/admin/{entityName.ToLower()}edit/{pageParameters}\", disabled: true)");

            sb.AppendLine("\t};");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\tNavigationService.SetBreadcrumbs(breadcrumbs);");

            sb.AppendLine("}");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateOnParametersSet(IEntityType entity, string webApiDataServiceClassName)
        {
            var entityName = entity.ClrType.Name;
            var entityNameLower = Inflector.ToLowerFirstCharacter(entityName);
            var entityParameterSignatureUntyped = GetMethodParametersWithoutTypes(entity, camelize: false);
            var primaryKeys = GetPrimaryKeys(entity);

            StringBuilder pkMismatchSB = new StringBuilder();
            foreach (var pk in primaryKeys)
            {
                pkMismatchSB.Append($" || {entityName}.{pk} != {pk}");
            }
            var pkMismatch = pkMismatchSB.ToString();

            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine("protected async override Task OnParametersSetAsync()");
            sb.AppendLine("{");

            sb.AppendLine("\tIsReady = false;");
            sb.AppendLine("\tawait SetSavedAsync(false);");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\ttry");
            sb.AppendLine("\t{");

            var firstPrimaryKeyName = primaryKeys.FirstOrDefault();
            IProperty firstPrimaryKey = null;
            if (firstPrimaryKeyName != null)
            {
                foreach (KeyValuePair<IList<IProperty>, IKey> key in entity.Keys)
                {
                    foreach (IProperty item in key.Key)
                    {
                        if (item.Name == firstPrimaryKeyName)
                        {
                            firstPrimaryKey = item;
                        }
                    }
                }
            }

            if (firstPrimaryKey != null)
            {
                if (firstPrimaryKey.ClrType.FullName.Equals("System.Int32"))
                {
                    sb.AppendLine($"\t\tif ({firstPrimaryKeyName} == 0) // A new item is being created - opportunity to populate initial/default state");
                }
                else if (firstPrimaryKey.ClrType.FullName.Equals("System.Guid"))
                {
                    sb.AppendLine($"\t\tif ({firstPrimaryKeyName} == Guid.Empty) // A new item is being created - opportunity to populate initial/default state");
                }
                else
                {
                    sb.AppendLine($"\t\tif (false) // A new item is being created - opportunity to populate initial/default state");
                }

                sb.AppendLine("\t\t{");

                sb.AppendLine("\t\t\t// Define entity defaults");
                sb.AppendLine($"\t\t\t{entityName} = new {entityName} {{ }};");

                sb.AppendLine("\t\t}");
                sb.AppendLine("\t\telse");
                sb.AppendLine("\t\t{");
            }

            sb.AppendLine($"\t\t\tif ({entityName} == null{pkMismatch})");
            sb.AppendLine("\t\t\t{");

            sb.AppendLine($"\t\t\t\tvar result = await {webApiDataServiceClassName}.Get{entityName}Async({entityParameterSignatureUntyped});");
            sb.AppendLine("\t\t\t\tif (result.IsSuccessStatusCode)");
            sb.AppendLine("\t\t\t\t{");

            sb.AppendLine($"\t\t\t\t\tvar {entityNameLower} = result.Data;");
            sb.AppendLine("\t\t\t\t\t// Admins and other approved user claims (Add below) only");
            sb.AppendLine("\t\t\t\t\tif (!User.IsInRole(Consts.ROLE_ADMIN))");
            sb.AppendLine("\t\t\t\t\t{");

            sb.AppendLine("\t\t\t\t\t\tNavigationManager.NavigateTo($\"/Authorization/AccessDenied\");");

            sb.AppendLine("\t\t\t\t\t}");
            sb.AppendLine("\t\t\t\t\telse");
            sb.AppendLine("\t\t\t\t\t{");

            sb.AppendLine($"\t\t\t\t\t\t{entityName} = {entityNameLower};");

            sb.AppendLine("\t\t\t\t\t}");

            sb.AppendLine("\t\t\t\t}");

            sb.AppendLine("\t\t\t}");

            if (firstPrimaryKey != null)
            {
                sb.AppendLine("\t\t}");
            }

            sb.AppendLine("\t}");
            sb.AppendLine("\tfinally");
            sb.AppendLine("\t{");

            sb.AppendLine("\t\tIsReady = true;");

            sb.AppendLine("\t}");

            sb.AppendLine("}");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateOnValidSubmit(IEntityType entity, string webApiDataServiceClassName)
        {
            var entityName = entity.ClrType.Name;
            var primaryKeys = GetPrimaryKeys(entity);

            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine("protected async Task OnValidSubmit()");
            sb.AppendLine("{");

            sb.AppendLine("\tawait SetSavedAsync(false);");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\tClearNoneValues();");
            sb.AppendLine(string.Empty);

            var firstPrimaryKeyName = primaryKeys.FirstOrDefault();
            IProperty firstPrimaryKey = null;
            if (firstPrimaryKeyName != null)
            {
                foreach (KeyValuePair<IList<IProperty>, IKey> key in entity.Keys)
                {
                    foreach (IProperty item in key.Key)
                    {
                        if (item.Name == firstPrimaryKeyName)
                        {
                            firstPrimaryKey = item;
                        }
                    }
                }
            }

            if (firstPrimaryKey != null)
            {
                if (firstPrimaryKey.ClrType.FullName.Equals("System.Int32"))
                {
                    sb.AppendLine($"\tif ({firstPrimaryKeyName} == 0) // A new item is being created - opportunity to populate initial/default state");
                }
                else if (firstPrimaryKey.ClrType.FullName.Equals("System.Guid"))
                {
                    sb.AppendLine($"\tif ({firstPrimaryKeyName} == Guid.Empty) // A new item is being created - opportunity to populate initial/default state");
                }
                else
                {
                    sb.AppendLine($"\tif (false) // A new item is being created - opportunity to populate initial/default state");
                }

                sb.AppendLine("\t{");

                sb.AppendLine($"\t\tvar result = await {webApiDataServiceClassName}.Create{entityName}Async({entityName});");
                sb.AppendLine("\t\tif (result.IsSuccessStatusCode)");
                sb.AppendLine("\t\t{");

                sb.AppendLine($"\t\t\t{entityName} = result.Data;");
                sb.AppendLine("\t\t\tStatusClass = \"alert-success\";");
                sb.AppendLine("\t\t\tMessage = \"New item added successfully.\";");
                sb.AppendLine("\t\t\tawait SetSavedAsync(true);");

                sb.AppendLine("\t\t}");
                sb.AppendLine("\t\telse");
                sb.AppendLine("\t\t{");

                sb.AppendLine("\t\t\tStatusClass = \"alert-danger\";");
                sb.AppendLine("\t\t\tMessage = \"Something went wrong adding the new item. Please try again.\";");
                sb.AppendLine("\t\t\tawait SetSavedAsync(false);");

                sb.AppendLine("\t\t}");

                sb.AppendLine("\t}");
                sb.AppendLine("\telse");
                sb.AppendLine("\t{");

                sb.AppendLine($"\t\tvar result = await {webApiDataServiceClassName}.Update{entityName}Async({entityName});");
                sb.AppendLine("\t\tif (result.IsSuccessStatusCode)");
                sb.AppendLine("\t\t{");

                sb.AppendLine("\t\t\tStatusClass = \"alert-success\";");
                sb.AppendLine($"\t\t\tMessage = \"{entityName} updated successfully.\";");
                sb.AppendLine("\t\t\tawait SetSavedAsync(true);");

                sb.AppendLine("\t\t}");
                sb.AppendLine("\t\telse");
                sb.AppendLine("\t\t{");

                sb.AppendLine("\t\t\tStatusClass = \"alert-danger\";");
                sb.AppendLine("\t\t\tMessage = \"Something went wrong updating the new item. Please try again.\";");
                sb.AppendLine("\t\t\tawait SetSavedAsync(false);");

                sb.AppendLine("\t\t}");

                sb.AppendLine("\t}");
            }

            sb.AppendLine("}");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateSmallMethods(IEntityType entity)
        {
            var setSavedLiteral = @"
        protected async Task SetSavedAsync(bool value)
        {
            Saved = value;
            if (value == true)
            {
                await JsRuntime.InvokeVoidAsync(""blazorExtensions.ScrollToTop"");
            }
        }
";

            var clearNullValuesLiteral = @"
        private void ClearNoneValues()
        {
            // Add handling for null values here
        }";

            StringBuilder sb = new StringBuilder();

            sb.Append(GenerateReturnToList(entity));
            sb.Append(setSavedLiteral);
            sb.Append(clearNullValuesLiteral);
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateReturnToList(IEntityType entity)
        {
            var entityName = entity.ClrType.Name;
            var entityNamePlural = Inflector.Pluralize(entityName).ToLower();

            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine("protected void ReturnToList()");
            sb.AppendLine("{");

            sb.AppendLine($"\tNavigationManager.NavigateTo(\"/admin/{entityNamePlural}\");");

            sb.AppendLine("}");

            return sb.ToString();
        }

    }
}
