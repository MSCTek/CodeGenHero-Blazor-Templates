using CodeGenHero.Core;
using CodeGenHero.Template.Blazor6.Generators;
using CodeGenHero.Template.Models;
using System;
using System.Collections.Generic;

namespace CodeGenHero.Template.Blazor6.Templates
{
    [Template(name: "AdminEditViewModel", version: "2021.11.12", uniqueTemplateIdGuid: "17AE856A-A589-40C0-A5BE-1579B0714385",
        description: "Generates a View Model for code-backing of a Razor page that allows an Admin to edit an Entity.")]
    public class AdminEditViewModelTemplate : BaseBlazorTemplate
    {
        public AdminEditViewModelTemplate()
        {
        }

        #region TemplateVariables

        [TemplateVariable(defaultValue: Consts.PTG_WebApiDataServiceInterfaceName_DEFAULT, description: Consts.PTG_WebApiDataServiceInterfaceName_DESC)]
        public string WebApiDataServiceInterfaceClassName { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_WebApiDataServiceClassName_DEFAULT, description: Consts.PTG_WebApiDataServiceClassName_DESC)]
        public string WebApiDataServiceClassName { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_AdminEditViewModelClassName_DEFAULT, description: Consts.PTG_AdminEditViewModelClassName_DESC)]
        public string AdminEditViewModelClassName { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_AppPageViewModelsNamespace_DEFAULT, description: Consts.PTG_AppPageViewModelsNamespace_DESC)]
        public string AppPageViewModelsNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_AdminPageNamespace_DEFAULT, description: Consts.PTG_AdminPageNamespace_DESC)]
        public string AdminPageNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_DtoNamespace_DEFAULT, description: Consts.PTG_DtoNamespace_DESC)]
        public string DtoNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_AppPageViewModelsNamespace_DEFAULT, description: Consts.PTG_AppPageViewModelsNamespace_DESC)]
        public string ApplicationProjectName { get; set; }

        [TemplateVariable(defaultValue: Consts.AdminEditViewModelOutputFilepath_DEFAULT, hiddenIndicator: true)]
        public string AdminEditViewModelOutputFilepath { get; set; }

        #endregion

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                var usings = new List<NamespaceItem>
                {
                    new NamespaceItem("Microsoft.AspNetCore.Authorization"),
                    new NamespaceItem("Microsoft.AspNetCore.Components"),
                    new NamespaceItem("Microsoft.AspNetCore.Components.Forms"),
                    new NamespaceItem("Microsoft.JSInterop"),
                    new NamespaceItem($"{BaseNamespace}.{ApplicationProjectName}.Services"),
                    new NamespaceItem($"{BaseNamespace}.{ApplicationProjectName}.Shared"),
                    new NamespaceItem($"{BaseNamespace}.Shared.Constants"),
                    new NamespaceItem($"{DtoNamespace}"),
                    new NamespaceItem("MudBlazor"),
                    new NamespaceItem("System"),
                    new NamespaceItem("System.Collections.Generic"),
                    new NamespaceItem("System.IO"),
                    new NamespaceItem("System.Net.Http"),
                    new NamespaceItem("System.Net.Http.Headers"),
                    new NamespaceItem("System.Threading.Tasks")
                };

                var entities = ProcessModel.MetadataSourceModel.GetEntityTypesByRegEx(RegexExclude, RegexInclude);

                foreach (var entity in entities)
                {
                    string outputFile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                    fileName: Consts.OUT_AdminEditViewModelOutputFilepath_DEFAULT);
                    string filepath = TokenReplacements(outputFile, entity);

                    string className = TokenReplacements(AdminEditViewModelClassName, entity);

                    var generator = new AdminEditViewModelGenerator(inflector: Inflector);
                    var generatedCode = generator.Generate(usings, AdminPageNamespace, NamespacePostfix, entity, className, WebApiDataServiceInterfaceClassName, WebApiDataServiceClassName);

                    retVal.Files.Add(new OutputFile()
                    {
                        Content = generatedCode,
                        Name = filepath
                    });
                }
            }
            catch (Exception ex)
            {
                base.AddError(ref retVal, ex, Enums.LogLevel.Error);
            }

            AddTemplateVariablesManagerErrorsToRetVal(ref retVal, Enums.LogLevel.Error);
            return retVal;
        }
    }
}
