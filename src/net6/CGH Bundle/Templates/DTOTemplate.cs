using CodeGenHero.Template.Blazor5.Generators;
using System;
using System.Collections.Generic;
using CodeGenHero.Template.Models;
using CodeGenHero.Core;

namespace CodeGenHero.Template.Blazor5.Templates
{
    [Template(name: "DTO", version: "2021.11.12", uniqueTemplateIdGuid: "C97FAB8D-DB03-4F94-9C85-14D1F9B41AA7",
        description: "Generates Data Transfer Objects based off provided Metadata Entities.")]
    public class DTOTemplate : BaseBlazorTemplate
    {
        public DTOTemplate()
        {

        }

        #region TemplateVariables

        [TemplateVariable(defaultValue: Consts.DTOFilePath_DEFAULTVALUE, hiddenIndicator: true)]
        public string DTOOutputFilepath { get; set; }

        [TemplateVariable(defaultValue: "true", description: "Determines whether to include related objects.")]
        public bool IncludeRelatedObjects { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_DtoNamespace_DEFAULT, description: Consts.PTG_DtoNamespace_DESC)]
        public string DtoNamespace { get; set; }

        #endregion

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                var excludedEntityNavigations = ProcessModel.GetAllExcludedEntityNavigations(
                    excludeRegExPattern: RegexExclude, includeRegExPattern: RegexInclude);
                var filteredEntityTypes = ProcessModel.MetadataSourceModel.GetEntityTypesByRegEx(RegexExclude, RegexInclude);

                var usings = new List<NamespaceItem>
                {
                    new NamespaceItem("System"),
                    new NamespaceItem("System.Collections.Generic")
                };
                var generator = new DTOGenerator(inflector: Inflector);

                foreach (var entityType in filteredEntityTypes)
                {
                    var className = Inflector.Pascalize(entityType.ClrType.Name);
                    string outputFile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                    fileName: Consts.OUT_DTOFilePath_DEFAULTVALUE);
                    string filepath = TokenReplacements(outputFile, entityType);
                    
                    var generatedCode = generator.Generate(usings, DtoNamespace, NamespacePostfix, entityType, excludedEntityNavigations, IncludeRelatedObjects, className);

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
