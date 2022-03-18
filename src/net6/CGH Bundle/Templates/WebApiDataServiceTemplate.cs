using CodeGenHero.Core;
using CodeGenHero.Template.Blazor6.Generators;
using CodeGenHero.Template.Models;
using System;
using System.Collections.Generic;

namespace CodeGenHero.Template.Blazor6.Templates
{
    [Template(name: "WebApiDataService", version: "2021.11.9", uniqueTemplateIdGuid: "DA87D00B-525F-487A-934A-0925A3F99DB9",
        description: "Generates the WebApiDataService implementation.")]
    public class WebApiDataServiceTemplate : BaseBlazorTemplate
    {
        public WebApiDataServiceTemplate()
        {
        }

        #region TemplateVariables

        [TemplateVariable(defaultValue: Consts.PTG_WebApiDataServiceNamespace_DEFAULT, description: Consts.PTG_WebApiDataServiceNamespace_DESC)]
        public string WebApiDataServiceNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_DtoNamespace_DEFAULT, description: Consts.PTG_DtoNamespace_DESC)]
        public string DtoNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_WebApiDataServiceInterfaceName_DEFAULT, description: Consts.PTG_WebApiDataServiceInterfaceName_DESC)]
        public string WebApiDataServiceInterfaceClassName { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_WebApiDataServiceClassName_DEFAULT, description: Consts.PTG_WebApiDataServiceClassName_DESC)]
        public string WebApiDataServiceClassName { get; set; }

        [TemplateVariable(defaultValue: Consts.WebApiDataServiceOutputFilepath_DEFAULT, hiddenIndicator: true)]
        public string WebApiDataServiceOutputFilepath { get; set; }

        [TemplateVariable(defaultValue: Consts.WebApiDataServiceApiRelativeURL_DEFAULT, description: Consts.WebApiDataServiceApiRelativeURL_DESC)]
        public string ApiRelativeURL { get; set; }

        [TemplateVariable(defaultValue: "", description: Consts.WebApiDataServiceCheckForIsActiveRegex)]
        public string CheckForIsActiveRegex { get; set; }

        #endregion

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                string outputFile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                    fileName: Consts.OUT_WebApiDataServiceOutputFilepath_DEFAULT);
                string filepath = outputFile;

                var usings = new List<NamespaceItem>
                {
                    new NamespaceItem("System"),
                    new NamespaceItem("System.Collections.Generic"),
                    new NamespaceItem("System.Net.Http"),
                    new NamespaceItem("System.Threading.Tasks"),
                    //new NamespaceItem("System.Text.Json")
                    new NamespaceItem("Microsoft.Extensions.Logging"),
                    new NamespaceItem($"{BaseNamespace}.Shared.DataService"),
                    new NamespaceItem($"Enums = {BaseNamespace}.Shared.Constants.Enums"),
                    new NamespaceItem($"xDTO = {DtoNamespace}")
                };

                var entities = ProcessModel.MetadataSourceModel.GetEntityTypesByRegEx(RegexExclude, RegexInclude);
                var entitiesToCheckIsActive = ProcessModel.MetadataSourceModel.GetEntityTypesMatchingRegEx(CheckForIsActiveRegex);

                var generator = new WebApiDataServiceGenerator(inflector: Inflector);
                string generatedCode = generator.Generate(usings, WebApiDataServiceNamespace, NamespacePostfix, entities, WebApiDataServiceClassName, WebApiDataServiceInterfaceClassName, ApiRelativeURL, entitiesToCheckIsActive);

                retVal.Files.Add(new OutputFile()
                {
                    Content = generatedCode,
                    Name = filepath
                });
            }
            catch (Exception ex)
            {
                base.AddError(ref retVal, ex, Enums.LogLevel.Error);
            }

            return retVal;
        }
    }
}
