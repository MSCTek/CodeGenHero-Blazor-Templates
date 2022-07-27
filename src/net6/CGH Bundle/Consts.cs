namespace CodeGenHero.Template.Blazor6
{
    public static class Consts
    {
        // Old

        #region Old Templates

        public const string BlazorAPIBaseControllerClassName_DEFAULTVALUE = "{NamespacePostfix}BaseApiController";
        public const string BlazorAPIController_Base_OutFileVariableName = "BlazorAPIControllerBaseOutputFilePath";
        public const string BlazorAPIControllerClassNamespace_DEFAULTVALUE = "{baseNamespace}.Api.Controllers";
        public const string BlazorRepository_Base_OutFileVariableName = "BlazorRepositoryBaseOutputFilePath";

        // Output Variable Names
        public const string BlazorRepository_Implementation_OutFileVariableName = "BlazorRepositoryImplementationOutputFilePath";

        public const string BlazorRepository_Interface_OutFileVariableName = "BlazorRepositoryInterfaceOutputFilePath";
        public const string BlazorRepositoryChildIncludesByTableName_DEFAULTDESCRIPTION = "A list of NameValue items (JSON) to be used as .Includes for all of a repository class's DB Get methods. Child Include names should be comma-delimited.";
        public const string BlazorRepositoryClassNamespace_DEFAULTDESCRIPTION = "The namespace of the Repository classes. Should be promoted to Global.";
        public const string BlazorRepositoryClassNamespace_DEFAULTVALUE = "{BaseNamespace}.Api.Database";
        public const string BlazorRepositoryDbContextClassName_DEFAULTDESCRIPTION = "The name of the DbContext class for your Metadata.";
        public const string BlazorRepositoryDbContextClassName_DEFAULTVALUE = "{BaseNamespace}DbContext";
        public const string BlazorRepositoryEntitiesNamespace_DEFAULTDESCRIPTION = "The namespace of the DBContext and Entity Model classes.";
        public const string BlazorRepositoryEntitiesNamespace_DEFAULTVALUE = "{BaseNamespace}.Repository.Entities";
//        public const string Entities_Namespace_DEFAULTVALUE = "{baseNamespace}.DataAccess.Entities.{namespacePostfix}";
        public const string Model_Namespace_DEFAULTVALUE = "{baseNamespace}.Model";

        // Output Paths/Filenames
        public const string OUT_BlazorAPIControllerFilePath_DEFAULTVALUE = "Controllers\\{NamespacePostfix}[tablename]Controller.cs";

        public const string OUT_BlazorRepositoryBaseFilePath_DEFAULTVALUE = "Repository\\{NamespacePostfix}BaseRepository.cs";
        public const string OUT_BlazorRepositoryImplementationFilePath_DEFAULTVALUE = "Repository\\{NamespacePostfix}[tablename]Repository.cs";
        public const string OUT_BlazorRepositoryInterfaceFilePath_DEFAULTVALUE = "Repository\\I{NamespacePostfix}[tablename]Repository.cs";
        public const string OUT_ToDataMapperFilePath_DEFAULTVALUE = "Blazor\\DataMappers\\ToDataMappers.cs";
        public const string OUT_ToModelMapperFilePath_DEFAULTVALUE = "Blazor\\ModelMappers\\ToModelMappers.cs";

        // Template Variable Defaults
        public const string OutFilepath_DEFAULTDESCRIPTION = "Path and name of the outputted file.";

        public const string TEMPLATE_ToDataMapper = "ToDataMapper";
        public const string TEMPLATE_ToModelMapper = "ToModelMapper";

        public const string ToDataMapper_OutFileVariableName = "ToDataMapperOutputFilePath";
        public const string ToDataMapperClassNamespace_DEFAULTVALUE = "{baseNamespace}.Service";
        public const string ToModelMapper_OutFileVariableName = "ToModelMapperOutputFilePath";
        public const string ToModelMapperClassNamespace_DEFAULTVALUE = "{baseNamespace}.Service";

        #endregion Old Templates

        // Global Recommended

        #region Namespaces

        public const string PTG_APIControllerNamespace_DEFAULT = "{BaseNamespace}.Api.Controllers";
        public const string PTG_APIControllerNamespace_DESC = "The namespace of the API Controller classes this template will use. Should be promoted to Global.";
        public const string PTG_DtoNamespace_DEFAULT = "{BaseNamespace}.Shared.DTO";
        public const string PTG_DtoNamespace_DESC = "The namespace of the DTO classes this template will use. Should be promoted to Global.";
        public const string PTG_EntitiesNamespace_DEFAULT = "{BaseNamespace}.Repository.Entities";
        public const string PTG_EntitiesNamespace_DESC = "The namespace of the Db Entities that this Repository will use. Should be promoted to Global.";

        public const string PTG_MappersNamespace_DEFAULT = "{BaseNamespace}.Repository.Mappers";
        public const string PTG_MappersNamespace_DESC = "The namespace of Entity to DTO Mapper classes. Should be promoted to Global.";
        public const string PTG_RepositoryNamespace_DEFAULT = "{BaseNamespace}.Repository.Repositories";
        public const string PTG_RepositoryNamespace_DESC = "The namespace of the Repository classes this template will use. Should be promoted to Global.";

        public const string PTG_WebApiDataServiceNamespace_DEFAULT = "{BaseNamespace}.App.Services";
        public const string PTG_WebApiDataServiceNamespace_DESC = "The namespace of the Web API Data Service classes this template will use. Should be promoted to Global.";

        public const string PTG_AppPageViewModelsNamespace_DEFAULT = "{BaseNamespace}.App.Pages";
        public const string PTG_AppPageViewModelsNamespace_DESC = "The namespace of Razor Page viewmodels this template will use. Should be promoted to Global.";

        #endregion Namespaces

        #region Class Names

        public const string PTG_APIControllerName_DEFAULT = "[tablename]Controller";
        public const string PTG_APIControllerName_DESC = "The name of the API Controller classes. Use '[tablename]' token to substitute-in the name of the associated Entity Table. Should be promoted to Global.";
        public const string PTG_AutoMapperName_DEFAULT = "{namespacePostfix}AutoMapperProfile";
        public const string PTG_AutoMapperName_DESC = "The name of the AutoMapper Profile class. Should be promoted to Global.";
        public const string PTG_BaseAPIControllerName_DEFAULT = "{namespacePostfix}BaseApiController";
        public const string PTG_BaseAPIControllerName_DESC = "The name of the Base API Controller class. Should be promoted to Global.";
        public const string PTG_BaseApiControllerRelativeURL_DEFAULT = "api/{namespacePostfix}";
        public const string PTG_BaseApiControllerRelativeURL_DESC = "Relative URL of the API's route. Should be promoted to Global.";
        public const string PTG_DbContextName_DEFAULT = "{baseNamespace}DbContext";
        public const string PTG_DbContextName_DESC = "The name of the DbContext class this Repository is expected to access. Should be promoted to Global.";

        public const string PTG_GenericFactoryInterfaceName_DEFAULT = "I{namespacePostfix}GenericFactory";
        public const string PTG_GenericFactoryInterfaceName_DESC = "The name of the Generic Factory Interface class. Should be promoted to Global.";
        public const string PTG_GenericFactoryName_DEFAULT = "{namespacePostfix}GenericFactory";
        public const string PTG_GenericFactoryName_DESC = "The name of the Generic Factory class. Should be promoted to Global.";
        public const string PTG_RepositoryCrudInterfaceName_DEFAULT = "I{namespacePostfix}RepositoryCrud";
        public const string PTG_RepositoryCrudInterfaceName_DESC = "The name of the Repository CRUD Interface class. Should be promoted to Global.";

        public const string PTG_RepositoryInterfaceClassName_DEFAULT = "I{namespacePostfix}Repository";
        public const string PTG_RepositoryInterfaceClassName_DESC = "The name of the Repository Interface class. Should be promoted to Global.";

        public const string PTG_RepositoryName_DEFAULT = "{namespacePostfix}Repository";
        public const string PTG_RepositoryName_DESC = "The name of the Repository class. Should be promoted to Global.";

        public const string PTG_WebApiDataServiceInterfaceName_DEFAULT = "IWebApiDataService{namespacePostfix}";
        public const string PTG_WebApiDataServiceInterfaceName_DESC = "The name of the Web API Data Service interface. Should be promoted to Global.";

        public const string PTG_WebApiDataServiceClassName_DEFAULT = "WebApiDataService{namespacePostfix}";
        public const string PTG_WebApiDataServiceClassName_DESC = "The name of the Web API Data Service class. Should be promoted to Global.";

        public const string PTG_ApiStatusControllerClassName_DEFAULT = "{namespacePostfix}ApiStatusController";
        public const string PTG_ApiStatusControllerClassName_DESC = "The name of the API Status Controller class. Should be promoted to Global.";

        public const string PTG_AdminListPageViewModelClassName_DEFAULT = "{namespacePostfix}[tablepluralname]ListViewModel";
        public const string PTG_AdminListPageViewModelClassName_DESC = "The name of the Admin List Page Viewmodel class. Should be promoted to Global.";

        public const string PTG_AdminEditViewModelClassName_DEFAULT = "{namespacePostfix}[tablename]EditViewModel";
        public const string PTG_AdminEditViewModelClassName_DESC = "The name of the Admin Edit Page Viewmodel class. Should be promoted to Global.";

        #endregion Class Names

        #region Output Filepaths
        /// Output Filepath Defaults
        public const string APIControllerFilePath_DEFAULTVALUE = "Blazor\\Controllers\\[tablename]Controller.cs";

        public const string APIControllerCustomFilepath_DEFAULTVALUE = "Blazor\\Controllers\\Custom\\[tablename]ControllerCustom.cs";

        public const string AutoMapperProfileOutputFilepath_DEFAULT = "Blazor\\Repository\\{NamespacePostfix}AutoMapperProfile.cs";

        public const string BaseAPIControllerOutputFilepath_DEFAULT = "Blazor\\API\\{NamespacePostfix}BaseApiController.cs";

        public const string GenericFactoryInterfaceOutputFilepath_DEFAULT = "Blazor\\Repository\\I{NamespacePostfix}GenericFactory.cs";

        public const string GenericFactoryOutputFilepath_DEFAULT = "Blazor\\Repository\\{NamespacePostfix}GenericFactory.cs";

        public const string RepositoryCrudInterfaceOutputFilepath_DEFAULT = "Blazor\\Repository\\I{NamespacePostfix}RepositoryCrud.cs";

        public const string RepositoryInterfaceOutputFilepath_DEFAULT = "Blazor\\Repository\\I{NamespacePostfix}Repository.cs";

        public const string RepositoryOutputFilepath_DEFAULT = "Blazor\\Repository\\{NamespacePostfix}Repository.cs";

        public const string DTOFilePath_DEFAULTVALUE = "Blazor\\DTO\\{NamespacePostfix}[tablename].cs";

        public const string WebApiDataServiceInterfaceOutputFilepath_DEFAULT = "Blazor\\Services\\IWebApiDataService{NamespacePostfix}.cs";

        public const string WebApiDataServiceOutputFilepath_DEFAULT = "Blazor\\Services\\WebApiDataService{NamespacePostfix}.cs";

        public const string ApiStatusControllerOutputFilepath_DEFAULT = "Blazor\\API\\{NamespacePostfix}ApiStatusController.cs";

        public const string AdminListPageOutputFilepath_DEFAULT = "Blazor\\App\\Pages\\{NamespacePostfix}[tablepluralname]List.razor";

        public const string AdminListPageViewModelOutputFilepath_DEFAULT = "Blazor\\App\\Pages\\{NamespacePostfix}[tablepluralname]List.razor.cs";

        public const string AdminEditViewModelOutputFilepath_DEFAULT = "Blazor\\App\\Pages\\{NamespacePostfix}[tablename]Edit.razor.cs";

        public const string AdminEditPageOutputFilepath_DEFAULT = "Blazor\\App\\Pages\\{NamespacePostfix}[tablename]Edit.razor";

        #endregion

        #region Output Variable Names
        /// Output Filepath Variable Names
        public const string OUT_APIControllerFilePath_DEFAULTVALUE = "APIControllerOutputFilepath";

        public const string OUT_APIControllerCustomFilePath_DEFAULTVALUE = "APIControllerCustomOutputFilepath";

        public const string OUT_AutoMapperProfileOutputFilepath_DEFAULT = "AutoMapperProfileOutputFilepath";

        public const string OUT_BaseAPIControllerOutputFilepath_DEFAULT = "BaseAPIControllerOutputFilepath";

        public const string OUT_GenericFactoryInterfaceOutputFilepath_DEFAULT = "GenericFactoryInterfaceOutputFilepath";

        public const string OUT_GenericFactoryOutputFilepath_DEFAULT = "GenericFactoryOutputFilepath";

        public const string OUT_RepositoryCrudInterfaceOutputFilepath_DEFAULT = "RepositoryCrudInterfaceOutputFilepath";

        public const string OUT_RepositoryInterfaceOutputFilepath_DEFAULT = "RepositoryInterfaceOutputFilepath";

        public const string OUT_RepositoryOutputFilepath_DEFAULT = "RepositoryOutputFilepath";

        public const string OUT_DTOFilePath_DEFAULTVALUE = "DTOOutputFilepath";

        public const string OUT_WebApiDataServiceInterfaceOuputFilepath_DEFAULT = "WebApiDataServiceInterfaceOuputFilepath";

        public const string OUT_WebApiDataServiceOutputFilepath_DEFAULT = "WebApiDataServiceOutputFilepath";

        public const string OUT_ApiStatusControllerOutputFilepath_DEFAULT = "ApiStatusControllerOutputFilepath";

        public const string OUT_AdminListPageOutputFilepath_DEFAULT = "AdminListPageOutputFilepath";

        public const string OUT_AdminListPageViewModelOutputFilepath_DEFAULT = "AdminListPageViewModelOutputFilepath";

        public const string OUT_AdminEditViewModelOutputFilepath_DEFAULT = "AdminEditViewModelOutputFilepath";

        public const string OUT_AdminEditPageOutputFilepath_DEFAULT = "AdminEditPageOutputFilepath";

        #endregion

        /// Class-specific variables
        // Web API Data Service
        public const string WebApiDataServiceCheckForIsActiveRegex = "Regular Expression to determine which Entities should have an \"IsActive\" check in their Get Methods. Only include entities that have a boolean property of this name.";
    }
}