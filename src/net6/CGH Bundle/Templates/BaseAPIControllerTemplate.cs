﻿using CodeGenHero.Core;
using CodeGenHero.Template.Blazor6.Generators;
using CodeGenHero.Template.Models;
using System;
using System.Collections.Generic;

namespace CodeGenHero.Template.Blazor6.Templates
{
    [Template(name: "BaseAPIController", version: "2022.06.21", uniqueTemplateIdGuid: "AF56140D-4926-4E6A-ADDB-49F3CFCD4A53",
        description: "Generates a Base API Controller class for anonymous API Controllers to inherit from.")]
    public sealed class BaseAPIControllerTemplate : BaseBlazorTemplate
    {
        public BaseAPIControllerTemplate()
        {
        }

        #region TemplateVariables

        [TemplateVariable(defaultValue: Consts.PTG_BaseAPIControllerName_DEFAULT, description: Consts.PTG_BaseAPIControllerName_DESC)]
        public string BaseAPIControllerClassName { get; set; }

        [TemplateVariable(defaultValue: "false", description: "If true, will generate an Authorized version of the API Controller instead.")]
        public bool AuthorizedController { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_APIControllerNamespace_DEFAULT, description: Consts.PTG_APIControllerNamespace_DESC)]
        public string APIControllerNamespace { get; set; }

        [TemplateVariable(defaultValue: "false", description: "If true, will include the flag [AutoInvalidateCacheOutput] in the generated code.")]
        public bool AutoInvalidateCacheOutput { get; set; }

        [TemplateVariable(defaultValue: Consts.BaseAPIControllerOutputFilepath_DEFAULT, hiddenIndicator: true)]
        public string BaseAPIControllerOutputFilepath { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_RepositoryNamespace_DEFAULT, description: Consts.PTG_RepositoryNamespace_DESC)]
        public string RepositoryNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_BaseApiControllerRelativeURL_DEFAULT, description: Consts.PTG_BaseApiControllerRelativeURL_DESC)]
        public string ApiRelativeURL { get; set; }

        #endregion TemplateVariables

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                string outputFile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                    fileName: Consts.OUT_BaseAPIControllerOutputFilepath_DEFAULT);
                string filepath = outputFile;

                var usings = new List<NamespaceItem>
                {
                    new NamespaceItem("System"),
                    new NamespaceItem("System.Collections.Generic"),
                    new NamespaceItem("System.Linq"),
                    new NamespaceItem("System.Net"),
                    new NamespaceItem("System.Runtime.CompilerServices"),
                    new NamespaceItem("Microsoft.AspNetCore.Mvc"),
                    new NamespaceItem("Microsoft.AspNetCore.Authorization"),
                    new NamespaceItem("Microsoft.Extensions.Logging"),
                    new NamespaceItem(RepositoryNamespace),
                    new NamespaceItem("Microsoft.AspNetCore.Http"),
                    new NamespaceItem("Microsoft.AspNetCore.Routing"),
                    new NamespaceItem("Microsoft.Extensions.Logging.Abstractions"),
                    new NamespaceItem("Microsoft.AspNetCore.Http.Extensions"),
                    new NamespaceItem($"{BaseNamespace}.Shared.DataService"),
                    new NamespaceItem($"Enums = {BaseNamespace}.Shared.Constants.Enums")
                };

                var entities = ProcessModel.MetadataSourceModel.GetEntityTypesByRegEx(RegexExclude, RegexInclude);

                var generator = new BaseAPIControllerGenerator(inflector: Inflector);
                var generatedCode = generator.Generate(usings, APIControllerNamespace, NamespacePostfix, AuthorizedController, AutoInvalidateCacheOutput, BaseAPIControllerClassName, ApiRelativeURL);

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