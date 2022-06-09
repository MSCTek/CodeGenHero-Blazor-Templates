using CodeGenHero.Core;
using CodeGenHero.Template.Blazor6.Generators;
using CodeGenHero.Template.Models;
using System;
using System.Collections.Generic;

namespace CodeGenHero.Template.Blazor6.Templates
{
    [Template(name: "APIControllerCustom", version: "2022.06.09", uniqueTemplateIdGuid: "44D5B085-471A-4F79-9440-4254C967F282",
        description: "Creates partial API Controllers to perform custom Controller Logic separate from generated logic.")]
    public class ApiControllerCustomTemplate : BaseBlazorTemplate
    {
        public ApiControllerCustomTemplate()
        {
        }

        #region TemplateVariables

        [TemplateVariable(Consts.APIControllerCustomFilepath_DEFAULTVALUE, hiddenIndicator: true)]
        public string APIControllerCustomOutputFilepath { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_APIControllerName_DEFAULT, description: Consts.PTG_APIControllerName_DESC)]
        public string APIControllerClassName { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_APIControllerNamespace_DEFAULT, description: Consts.PTG_APIControllerNamespace_DESC)]
        public string APIControllerNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_EntitiesNamespace_DEFAULT, description: Consts.PTG_EntitiesNamespace_DESC)]
        public string EntitiesNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_RepositoryInterfaceClassName_DEFAULT, description: Consts.PTG_RepositoryInterfaceClassName_DESC)]
        public string RepositoryInterfaceClassName { get; set; }

        #endregion

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                var filteredEntityTypes = ProcessModel.MetadataSourceModel.GetEntityTypesByRegEx(RegexExclude, RegexInclude);

                var usings = new List<NamespaceItem>
                {
                    new NamespaceItem($"{BaseNamespace}.Repository.Infrastructure"),
                    new NamespaceItem($"{BaseNamespace}.Repository.Repositories"),
                    new NamespaceItem($"ent{NamespacePostfix} = {EntitiesNamespace}"),
                    new NamespaceItem($"Enums = {BaseNamespace}.Shared.Constants.Enums")
                };

                var generator = new APIControllerCustomGenerator(inflector: Inflector);

                foreach (var entity in filteredEntityTypes)
                {
                    string outputfile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                    fileName: Consts.OUT_APIControllerCustomFilePath_DEFAULTVALUE);
                    outputfile = TokenReplacements(outputfile, entity);
                    string filepath = outputfile;

                    // Individualize the Class Name
                    string className = TokenReplacements(APIControllerClassName, entity);

                    string generatedCode = generator.Generate(usings, APIControllerNamespace, NamespacePostfix, entity, className, RepositoryInterfaceClassName);

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
