using CodeGenHero.Template.Models;
using System.Runtime.InteropServices;

// In SDK-style projects such as this one, several assembly attributes that were historically
// defined in this file are now automatically added during build and populated with
// values defined in project properties. For details of which attributes are included
// and how to customise this process see: https://aka.ms/assembly-info-properties

// Setting ComVisible to false makes the types in this assembly not visible to COM
// components.  If you need to access a type in this assembly from COM, set the ComVisible
// attribute to true on that type.

[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM.

[assembly: Guid("6713c0c5-a5d4-4391-b8e0-26e5c3fa1e54")]
[assembly: TemplateAssembly(name: "CodeGenHero.Template.Blazor",
    uniqueAssemblyIdGuid: "{E0FECEBA-AA48-4FDB-9EB5-2177307BB238}",
    description: "Templates for solutions using the ASP.NET Core Blazor.",
    author: "Micro Support Center, Inc.",
    version: "2021.12.7",
    requiredMetadataSource: "EFCore")]