﻿using CodeGenHero.Core;
using CodeGenHero.Template.Blazor6.Generators;
using CodeGenHero.Template.Models;
using System;
using System.Collections.Generic;

namespace CodeGenHero.Template.Blazor6.Templates
{
    [Template(name: "APIController", version: "2021.9.14", uniqueTemplateIdGuid: "70A21A48-7EE1-42F5-B1EB-4891E290A17D",
        description: "Creates standard API Controllers to perform CRUD operations on Metadata-provided Entities.")]
    public sealed class APIControllerTemplate : BaseBlazorTemplate
    {
        public APIControllerTemplate()
        {
        }

        #region TemplateVariables

        [TemplateVariable(defaultValue: Consts.PTG_APIControllerName_DEFAULT, description: Consts.PTG_APIControllerName_DESC)]
        public string APIControllerClassName { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_APIControllerNamespace_DEFAULT, description: Consts.PTG_APIControllerNamespace_DESC)]
        public string APIControllerNamespace { get; set; }

        [TemplateVariable(Consts.APIControllerFilePath_DEFAULTVALUE, hiddenIndicator: true)]
        public string APIControllerOutputFilepath { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_BaseAPIControllerName_DEFAULT, description: Consts.PTG_BaseAPIControllerName_DESC)]
        public string BaseAPIControllerClassName { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_DtoNamespace_DEFAULT, description: Consts.PTG_DtoNamespace_DESC)]
        public string DtoNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_EntitiesNamespace_DEFAULT, description: Consts.PTG_EntitiesNamespace_DESC)]
        public string EntitiesNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_GenericFactoryInterfaceName_DEFAULT, description: Consts.PTG_GenericFactoryInterfaceName_DESC)]
        public string GenericFactoryInterfaceClassName { get; set; }

        [TemplateVariable(defaultValue: null,
            description: "A list of MSC.CodeGenHero.DTO.NameValue items serialized as JSON that correspond to table names and integer values for the maximum number of rows to return for a single request for a page of data.")]
        public string MaxRequestPerPageOverrideByTableName { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_RepositoryInterfaceClassName_DEFAULT, description: Consts.PTG_RepositoryInterfaceClassName_DESC)]
        public string RepositoryInterfaceClassName { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_RepositoryNamespace_DEFAULT, description: Consts.PTG_RepositoryNamespace_DESC)]
        public string RepositoryNamespace { get; set; }

        #endregion TemplateVariables

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            var maxRequestPerPageOverrides =
                    DeserializeJsonObject<List<NameValue>>(MaxRequestPerPageOverrideByTableName);

            if (maxRequestPerPageOverrides == null)
            {
                maxRequestPerPageOverrides = new List<NameValue>();
            }

            try
            {
                var filteredEntityTypes = ProcessModel.MetadataSourceModel.GetEntityTypesByRegEx(RegexExclude, RegexInclude);

                var usings = new List<NamespaceItem>
                {
                    new NamespaceItem("Marvin.JsonPatch"),
                    new NamespaceItem("Microsoft.AspNetCore.Http"),
                    new NamespaceItem("Microsoft.AspNetCore.Mvc"),
                    new NamespaceItem("Microsoft.AspNetCore.Routing"),
                    new NamespaceItem("Microsoft.Extensions.Logging"),
                    new NamespaceItem($"{BaseNamespace}.Api.Infrastructure"),
                    new NamespaceItem($"{BaseNamespace}.Repository.Infrastructure"),
                    new NamespaceItem($"{BaseNamespace}.Repository.Mappers"),
                    new NamespaceItem($"{BaseNamespace}.Repository.Repositories"),
                    new NamespaceItem($"{BaseNamespace}.Shared.DataService"),
                    new NamespaceItem("System"),
                    new NamespaceItem("System.Collections.Generic"),
                    new NamespaceItem("System.Linq"),
                    new NamespaceItem("System.Threading.Tasks"),
                    new NamespaceItem($"static {BaseNamespace}.Repository.Infrastructure.Enums"),
                    new NamespaceItem($"dto{NamespacePostfix} = {DtoNamespace}"),
                    new NamespaceItem($"ent{NamespacePostfix} = {EntitiesNamespace}"),
                    new NamespaceItem($"Enums = {BaseNamespace}.Shared.Constants.Enums")
                };

                var generator = new APIControllerGenerator(inflector: Inflector);

                foreach (var entity in filteredEntityTypes)
                {
                    string outputfile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                    fileName: Consts.OUT_APIControllerFilePath_DEFAULTVALUE);
                    outputfile = TokenReplacements(outputfile, entity);
                    string filepath = outputfile;

                    // Individualize the Class Name
                    string className = TokenReplacements(APIControllerClassName, entity);

                    string generatedCode = generator.Generate(usings, APIControllerNamespace, NamespacePostfix, entity, maxRequestPerPageOverrides, className, BaseAPIControllerClassName, RepositoryInterfaceClassName, GenericFactoryInterfaceClassName);

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