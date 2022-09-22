using CodeGenHero.Core;
using CodeGenHero.Template.Blazor6.Generators;
using CodeGenHero.Template.Models;
using System;
using System.Collections.Generic;

namespace CodeGenHero.Template.Blazor6.Templates
{
    [Template(name: "AutoMapperProfile", version: "2021.9.14", uniqueTemplateIdGuid: "7B0AA8DE-D2FB-4EFA-98E1-75FEB116A153",
        description: "Generates an Automapper Profile based off provided Metadata, which converts Repository EF Entities to untracked DTOs. Requires AutoMapper.Extensions.Microsoft.DependencyInjection NuGet package.")]
    public sealed class AutoMapperProfileTemplate : BaseBlazorTemplate
    {
        public AutoMapperProfileTemplate()
        {
        }

        #region TemplateVariables

        [TemplateVariable(defaultValue: "false", description: "Whether or not the template should generate entity foreign-key navigations")]
        public bool IncludeEntityNavigations { get; set; }

        [TemplateVariable(defaultValue: "", description: "Regex Pattern to exclude Entity Navigations based off of.")]
        public string ExcludedNavigationRegex { get; set; }

        [TemplateVariable(defaultValue: "", description: "Regex Pattern to include Entity Navigations based off of.")]
        public string IncludedNavigationRegex { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_AutoMapperName_DEFAULT, description: Consts.PTG_AutoMapperName_DESC)]
        public string AutoMapperProfileClassName { get; set; }

        [TemplateVariable(defaultValue: Consts.AutoMapperProfileOutputFilepath_DEFAULT, hiddenIndicator: true)]
        public string AutoMapperProfileOutputFilepath { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_DtoNamespace_DEFAULT, description: Consts.PTG_DtoNamespace_DESC)]
        public string DtoNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_EntitiesNamespace_DEFAULT, description: Consts.PTG_EntitiesNamespace_DESC)]
        public string EntitiesNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_MappersNamespace_DEFAULT, description: Consts.PTG_MappersNamespace_DESC)]
        public string MappersNamespace { get; set; }

        #endregion TemplateVariables

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                string outputFile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                    fileName: Consts.OUT_AutoMapperProfileOutputFilepath_DEFAULT);
                string filepath = outputFile;

                var usings = new List<NamespaceItem>
                {
                    new NamespaceItem("AutoMapper"),
                    new NamespaceItem($"xDTO = {DtoNamespace}"),
                    new NamespaceItem($"xENT = {EntitiesNamespace}")
                };

                var entities = ProcessModel.MetadataSourceModel.GetEntityTypesByRegEx(RegexExclude, RegexInclude);
                var excludedEntityNavigations = ProcessModel.GetAllExcludedEntityNavigations(ExcludedNavigationRegex, IncludedNavigationRegex);

                var generator = new AutoMapperProfileGenerator(inflector: Inflector);
                var generatedCode = generator.Generate(usings, MappersNamespace, NamespacePostfix, entities, IncludeEntityNavigations, excludedEntityNavigations, AutoMapperProfileClassName);

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