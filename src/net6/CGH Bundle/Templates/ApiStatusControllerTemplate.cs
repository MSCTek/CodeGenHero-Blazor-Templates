using CodeGenHero.Core;
using CodeGenHero.Template.Blazor5.Generators;
using CodeGenHero.Template.Models;
using System;
using System.Collections.Generic;

namespace CodeGenHero.Template.Blazor5.Templates
{
    [Template(name: "APIStatusController", version: "2021.11.12", uniqueTemplateIdGuid: "021B5127-262B-45FB-A3A7-7388B2EDFCA9",
        description: "Creates the API Status Controller class")]
    public class ApiStatusControllerTemplate : BaseBlazorTemplate
    {
        public ApiStatusControllerTemplate()
        {

        }

        #region TemplateVariables

        [TemplateVariable(defaultValue: Consts.PTG_ApiStatusControllerClassName_DEFAULT, description: Consts.PTG_ApiStatusControllerClassName_DESC)]
        public string ApiStatusControllerClassName { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_BaseAPIControllerName_DEFAULT, description: Consts.PTG_BaseAPIControllerName_DESC)]
        public string BaseAPIControllerClassName { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_RepositoryInterfaceClassName_DEFAULT, description: Consts.PTG_RepositoryInterfaceClassName_DESC)]
        public string RepositoryInterfaceClassName { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_APIControllerNamespace_DEFAULT, description: Consts.PTG_APIControllerNamespace_DESC)]
        public string APIControllerNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_RepositoryNamespace_DEFAULT, description: Consts.PTG_RepositoryNamespace_DESC)]
        public string RepositoryNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.ApiStatusControllerOutputFilepath_DEFAULT, hiddenIndicator: true)]
        public string ApiStatusControllerOutputFilepath { get; set; }

        #endregion

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                string outputFile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                    fileName: Consts.OUT_ApiStatusControllerOutputFilepath_DEFAULT);
                string filepath = outputFile;

                var usings = new List<NamespaceItem>
                {
                    new NamespaceItem("System"),
                    new NamespaceItem("System.Threading.Tasks"),
                    new NamespaceItem("Microsoft.AspNetCore.Mvc"),
                    new NamespaceItem(RepositoryNamespace),
                    new NamespaceItem("Microsoft.Extensions.Logging"),
                    new NamespaceItem("Microsoft.AspNetCore.Http"),
                    new NamespaceItem("Microsoft.AspNetCore.Routing"),
                    new NamespaceItem($"{BaseNamespace}.Api.Infrastructure")
                };

                var generator = new ApiStatusControllerGenerator(inflector: Inflector);
                string generatedCode = generator.Generate(usings, APIControllerNamespace, NamespacePostfix, ApiStatusControllerClassName, BaseAPIControllerClassName, RepositoryInterfaceClassName);

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

            AddTemplateVariablesManagerErrorsToRetVal(ref retVal, Enums.LogLevel.Error);
            return retVal;
        }
    }
}
