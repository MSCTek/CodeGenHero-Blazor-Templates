﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGHTemplate.BlazorAAD.Wizard
{
    internal static class Consts
    {
        public static class DictionaryEntries
        {
            public const string ConnectionString = "$dbconnectionstring$";
            public const string AadSiteDomain = "$AadSiteDomain$";
            public const string AadApiTenantId = "$AadApiTenantId$";
            public const string AadApiClientId = "$AadApiClientId$";
            public const string AadAppClientId = "$AadAppClientId$";
            public const string AadHostClientId = "$AadHostClientId$";
            public const string DestinationDirectory = "$destinationdirectory$";
            public const string OriginalDestinationDirectory = "$origdestinationdirectory$";
            public const string ParentWizardName = "$parentwizardname$";
            public const string SafeProjectName = "$safeprojectname$";
            public const string SafeRootProjectName = "$saferootprojectname$";
            public const string SolutionDirectory = "$solutiondirectory$";
            public const string GlobalGuid1 = "$globalguid1$";
            public const string GlobalGuid2 = "$globalguid2$";
            public const string GlobalGuid3 = "$globalguid3$";
            public const string GlobalGuid4 = "$globalguid4$";
            public const string GlobalGuid5 = "$globalguid5$";
        }

        public static class ParentWizards
        {
            public const string RootWizard = "RootWizard";
        }

        public static class ProjectTemplates
        {
            public const string Root = "CGHBlazorAAD";
        }

        public static class ReadMeFiles
        {
            public const string Root = "ReadMe.md";
        }

        public const string GitIgnoreFileName = ".gitignore";
    }
}
