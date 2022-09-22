using CodeGenHero.Core;
using CodeGenHero.Template.Blazor6.Generators;
using CodeGenHero.Template.Models;
using System;
using System.Collections.Generic;

namespace CodeGenHero.Template.Blazor6.Templates
{
    [Template(name: "RepositoryInterface", version: "2021.9.14", uniqueTemplateIdGuid: "FF160397-8584-4518-8EC7-A9549B37515A",
        description: "Generates the Interface for a Repository Class to implement.")]
    public sealed class RepositoryInterfaceTemplate : BaseBlazorTemplate
    {
        public RepositoryInterfaceTemplate()
        {
        }

        #region TemplateVariables

        [TemplateVariable(defaultValue: Consts.PTG_DbContextName_DEFAULT, description: Consts.PTG_DbContextName_DESC)]
        public string DbContextName { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_EntitiesNamespace_DEFAULT, description: Consts.PTG_EntitiesNamespace_DESC)]
        public string EntitiesNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_RepositoryCrudInterfaceName_DEFAULT, description: Consts.PTG_RepositoryCrudInterfaceName_DESC)]
        public string RepositoryCrudInterfaceClassName { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_RepositoryInterfaceClassName_DEFAULT, description: Consts.PTG_RepositoryInterfaceClassName_DESC)]
        public string RepositoryInterfaceClassName { get; set; }

        [TemplateVariable(defaultValue: Consts.RepositoryInterfaceOutputFilepath_DEFAULT, hiddenIndicator: true)]
        public string RepositoryInterfaceOutputFilepath { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_RepositoryNamespace_DEFAULT, description: Consts.PTG_RepositoryNamespace_DESC)]
        public string RepositoryNamespace { get; set; }

        #endregion TemplateVariables

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                string outputFile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                    fileName: Consts.OUT_RepositoryInterfaceOutputFilepath_DEFAULT);
                string filepath = outputFile;

                var usings = new List<NamespaceItem>
                {
                    new NamespaceItem(EntitiesNamespace),
                    new NamespaceItem($"{BaseNamespace}.Repository.Infrastructure"),
                    new NamespaceItem("System"),
                    new NamespaceItem("System.Collections.Generic"),
                    new NamespaceItem("System.Threading.Tasks"),
                    new NamespaceItem($"ent{NamespacePostfix} = {EntitiesNamespace}"),
                    new NamespaceItem($"Enums = {BaseNamespace}.Shared.Constants.Enums")
                };

                var entities = ProcessModel.MetadataSourceModel.GetEntityTypesByRegEx(RegexExclude, RegexInclude);

                var generator = new RepositoryInterfaceGenerator(inflector: Inflector);
                string generatedCode = generator.Generate(usings, RepositoryNamespace, NamespacePostfix, entities, RepositoryInterfaceClassName, RepositoryCrudInterfaceClassName, DbContextName);

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