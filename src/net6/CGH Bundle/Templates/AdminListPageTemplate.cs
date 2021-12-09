﻿using CodeGenHero.Core;
using CodeGenHero.Template.Blazor5.Generators;
using CodeGenHero.Template.Models;
using System;
using System.Collections.Generic;

namespace CodeGenHero.Template.Blazor5.Templates
{
    [Template(name: "AdminListPage", version: "2021.11.12", uniqueTemplateIdGuid: "414C3369-6F09-4341-B755-A133EAB5E775",
        description: "Generates a basic Razor page visible to Admin users that lists all of a certain metadata entity.")]
    public class AdminListPageTemplate : BaseBlazorTemplate
    {
        public AdminListPageTemplate()
        {
        }

        #region TemplateVariables

        [TemplateVariable(defaultValue: Consts.PTG_AdminListPageViewModelClassName_DEFAULT, description: Consts.PTG_AdminListPageViewModelClassName_DESC)]
        public string AdminListPageViewModelClassName { get; set; }

        [TemplateVariable(defaultValue: Consts.AdminListPageOutputFilepath_DEFAULT)]
        public string AdminListPageOutputFilepath { get; set; }

        #endregion

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                var entities = ProcessModel.MetadataSourceModel.GetEntityTypesByRegEx(RegexExclude, RegexInclude);

                foreach (var entity in entities)
                {
                    string outputFile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                        fileName: Consts.OUT_AdminListPageOutputFilepath_DEFAULT);
                    string filepath = TokenReplacements(outputFile, entity);

                    var viewModelClassName = TokenReplacements(AdminListPageViewModelClassName, entity);

                    var generator = new AdminListPageGenerator(inflector: Inflector);
                    var generatedCode = generator.Generate(entity, viewModelClassName);

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
