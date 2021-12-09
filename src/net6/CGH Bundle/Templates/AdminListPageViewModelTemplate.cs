using CodeGenHero.Core;
using CodeGenHero.Template.Blazor5.Generators;
using CodeGenHero.Template.Models;
using System;
using System.Collections.Generic;

namespace CodeGenHero.Template.Blazor5.Templates
{
    [Template(name: "AdminListPageViewModel", version: "2021.11.12", uniqueTemplateIdGuid: "B76E62EC-FE5B-47C6-BB58-FB58ED7399E5",
        description: "Generates a View Model for code-backing of a Razor page that lists all of a certain Metadata entity.")]
    public class AdminListPageViewModelTemplate : BaseBlazorTemplate
    {
        public AdminListPageViewModelTemplate()
        {

        }

        #region TemplateVariables

        [TemplateVariable(defaultValue: Consts.PTG_AdminListPageViewModelClassName_DEFAULT, description: Consts.PTG_AdminListPageViewModelClassName_DESC)]
        public string AdminListPageViewModelClassName { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_WebApiDataServiceClassName_DEFAULT, description: Consts.PTG_WebApiDataServiceClassName_DESC)]
        public string WebApiDataServiceClassName { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_AppPageViewModelsNamespace_DEFAULT, description: Consts.PTG_AppPageViewModelsNamespace_DESC)]
        public string AppPageViewModelsNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_DtoNamespace_DEFAULT, description: Consts.PTG_DtoNamespace_DESC)]
        public string DtoNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.AdminListPageViewModelOutputFilepath_DEFAULT, hiddenIndicator: true)]
        public string AdminListPageViewModelOutputFilepath { get; set; }

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
                    new NamespaceItem("Microsoft.JSInterop"),
                    new NamespaceItem($"{BaseNamespace}.App.Components"),
                    new NamespaceItem($"{BaseNamespace}.App.Services"),
                    new NamespaceItem($"{BaseNamespace}.App.Shared"),
                    new NamespaceItem($"{BaseNamespace}.Shared.Constants"),
                    new NamespaceItem($"{BaseNamespace}.Shared.DataService"),
                    new NamespaceItem($"{DtoNamespace}"),
                    new NamespaceItem("MudBlazor"),
                    new NamespaceItem("System"),
                    new NamespaceItem("System.Collections.Generic"),
                    new NamespaceItem("System.Linq"),
                    new NamespaceItem("System.Threading.Tasks"),
                    new NamespaceItem($"Enums = {BaseNamespace}.Shared.Constants.Enums")
                };

                var entities = ProcessModel.MetadataSourceModel.GetEntityTypesByRegEx(RegexExclude, RegexInclude);

                foreach (var entity in entities)
                {
                    string outputFile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                    fileName: Consts.OUT_AdminListPageViewModelOutputFilepath_DEFAULT);
                    string filepath = TokenReplacements(outputFile, entity);

                    string className = TokenReplacements(AdminListPageViewModelClassName, entity);

                    var generator = new AdminListPageViewModelGenerator(inflector: Inflector);
                    var generatedCode = generator.Generate(usings, AppPageViewModelsNamespace, NamespacePostfix, entity, className, WebApiDataServiceClassName);

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
