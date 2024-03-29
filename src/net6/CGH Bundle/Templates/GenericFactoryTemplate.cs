﻿using CodeGenHero.Core;
using CodeGenHero.Template.Blazor6.Generators;
using CodeGenHero.Template.Models;
using System;
using System.Collections.Generic;

namespace CodeGenHero.Template.Blazor6.Templates
{
    [Template(name: "GenericFactory", version: "2021.9.14", uniqueTemplateIdGuid: "6696EDA9-AD96-46B4-AA58-1842BC9C2BBD",
        description: "Generates a Factory class that maps Entities to DTOs. Implies use of AutoMapper template and associated prerequisites, and should be placed alongside an Automapper.")]
    public sealed class GenericFactoryTemplate : BaseBlazorTemplate
    {
        public GenericFactoryTemplate()
        {
        }

        #region TemplateVariables

        [TemplateVariable(defaultValue: Consts.PTG_GenericFactoryName_DEFAULT, description: Consts.PTG_GenericFactoryName_DESC)]
        public string GenericFactoryClassName { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_GenericFactoryInterfaceName_DEFAULT, description: Consts.PTG_GenericFactoryInterfaceName_DESC)]
        public string GenericFactoryInterfaceClassName { get; set; }

        [TemplateVariable(defaultValue: Consts.GenericFactoryOutputFilepath_DEFAULT, hiddenIndicator: true)]
        public string GenericFactoryOutputFilepath { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_MappersNamespace_DEFAULT, description: Consts.PTG_MappersNamespace_DESC)]
        public string MappersNamespace { get; set; }

        #endregion TemplateVariables

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                string outputFile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                    fileName: Consts.OUT_GenericFactoryOutputFilepath_DEFAULT);
                string filepath = outputFile;

                var usings = new List<NamespaceItem>
                {
                    new NamespaceItem("AutoMapper"),
                    new NamespaceItem("System"),
                    new NamespaceItem("System.Collections.Generic"),
                    new NamespaceItem("System.Dynamic"),
                    new NamespaceItem("System.Linq"),
                    new NamespaceItem("System.Reflection")
                };

                var entities = ProcessModel.MetadataSourceModel.GetEntityTypesByRegEx(RegexExclude, RegexInclude);

                var generator = new GenericFactoryGenerator(inflector: Inflector);
                var generatedCode = generator.Generate(usings, MappersNamespace, NamespacePostfix, GenericFactoryClassName, GenericFactoryInterfaceClassName);

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