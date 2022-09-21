using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Collections.Generic;

namespace CGHTemplate.BlazorAAD.Wizard
{
    public class BlazorAADTemplateWizard : IWizard
    {
        private static MainWindow _mainWindow;
        private static string _templateName;

        public static bool? GetConnectionStringUserInput(string templateName)
        {
            _templateName = templateName;

            _mainWindow = new MainWindow();
            _mainWindow.TemplateParametersSet += TemplateParametersSet;
            var retVal = _mainWindow.ShowDialog();

            return retVal;
        }

        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(Project project)
        {
            Console.WriteLine($"Reached {nameof(ProjectFinishedGenerating)}");
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }

        public void RunFinished()
        {
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {

            if (RootWizard.RootDictionary.ContainsKey(Consts.DictionaryEntries.SafeRootProjectName))
            {
                replacementsDictionary[Consts.DictionaryEntries.SafeRootProjectName] = RootWizard.RootDictionary[Consts.DictionaryEntries.SafeRootProjectName];
            }

            if (RootWizard.RootDictionary.ContainsKey(Consts.DictionaryEntries.GlobalGuid1))
            {
                replacementsDictionary[Consts.DictionaryEntries.GlobalGuid1] = RootWizard.RootDictionary[Consts.DictionaryEntries.GlobalGuid1];
            }

            if (RootWizard.RootDictionary.ContainsKey(Consts.DictionaryEntries.GlobalGuid2))
            {
                replacementsDictionary[Consts.DictionaryEntries.GlobalGuid2] = RootWizard.RootDictionary[Consts.DictionaryEntries.GlobalGuid2];
            }

            if (RootWizard.RootDictionary.ContainsKey(Consts.DictionaryEntries.GlobalGuid3))
            {
                replacementsDictionary[Consts.DictionaryEntries.GlobalGuid3] = RootWizard.RootDictionary[Consts.DictionaryEntries.GlobalGuid3];
            }

            if (RootWizard.RootDictionary.ContainsKey(Consts.DictionaryEntries.GlobalGuid3))
            {
                replacementsDictionary[Consts.DictionaryEntries.GlobalGuid3] = RootWizard.RootDictionary[Consts.DictionaryEntries.GlobalGuid3];
            }

            if (RootWizard.RootDictionary.ContainsKey(Consts.DictionaryEntries.GlobalGuid4))
            {
                replacementsDictionary[Consts.DictionaryEntries.GlobalGuid4] = RootWizard.RootDictionary[Consts.DictionaryEntries.GlobalGuid4];
            }

            if (RootWizard.RootDictionary.ContainsKey(Consts.DictionaryEntries.GlobalGuid5))
            {
                replacementsDictionary[Consts.DictionaryEntries.GlobalGuid5] = RootWizard.RootDictionary[Consts.DictionaryEntries.GlobalGuid5];
            }


            // Obtain connection string value from RootDictionary and place it in the replacementsDictionary.
            if (RootWizard.RootDictionary.ContainsKey(Consts.DictionaryEntries.ConnectionString))
            {
                replacementsDictionary[Consts.DictionaryEntries.ConnectionString] = RootWizard.RootDictionary[Consts.DictionaryEntries.ConnectionString];
            }

            if (RootWizard.RootDictionary.ContainsKey(Consts.DictionaryEntries.AadSiteDomain))
            {
                replacementsDictionary[Consts.DictionaryEntries.AadSiteDomain] = RootWizard.RootDictionary[Consts.DictionaryEntries.AadSiteDomain];
            }

            if (RootWizard.RootDictionary.ContainsKey(Consts.DictionaryEntries.AadApiTenantId))
            {
                replacementsDictionary[Consts.DictionaryEntries.AadApiTenantId] = RootWizard.RootDictionary[Consts.DictionaryEntries.AadApiTenantId];
            }

            if (RootWizard.RootDictionary.ContainsKey(Consts.DictionaryEntries.AadApiClientId))
            {
                replacementsDictionary[Consts.DictionaryEntries.AadApiClientId] = RootWizard.RootDictionary[Consts.DictionaryEntries.AadApiClientId];
            }

            if (RootWizard.RootDictionary.ContainsKey(Consts.DictionaryEntries.AadAppClientId))
            {
                replacementsDictionary[Consts.DictionaryEntries.AadAppClientId] = RootWizard.RootDictionary[Consts.DictionaryEntries.AadAppClientId];
            }

            if (RootWizard.RootDictionary.ContainsKey(Consts.DictionaryEntries.AadHostClientId))
            {
                replacementsDictionary[Consts.DictionaryEntries.AadHostClientId] = RootWizard.RootDictionary[Consts.DictionaryEntries.AadHostClientId];
            }

        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }

        /// Event Subscribers
        private static void TemplateParametersSet(object sender, MainWindow.TemplateParametersSetEventArgs e)
        {
            // Place connection string input by user into the root dictionary
            RootWizard.RootDictionary[Consts.DictionaryEntries.ConnectionString] = e.ConnectionString;
            RootWizard.RootDictionary[Consts.DictionaryEntries.AadSiteDomain] = e.AadSiteDomain;
            RootWizard.RootDictionary[Consts.DictionaryEntries.AadApiTenantId] = e.AadApiTenantId;
            RootWizard.RootDictionary[Consts.DictionaryEntries.AadApiClientId] = e.AadApiClientId;
            RootWizard.RootDictionary[Consts.DictionaryEntries.AadAppClientId] = e.AadAppClientId;
            RootWizard.RootDictionary[Consts.DictionaryEntries.AadHostClientId] = e.AadHostClientId;

            _mainWindow.Close();
        }
    }
}
