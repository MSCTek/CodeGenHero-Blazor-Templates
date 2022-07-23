using System;

namespace ArtistSite.Shared.Constants
{
    public static class Enums
    {
        public enum CriterionCondition
        {
            Contains,
            DoesNotContain,
            IsContainedIn,
            IsNotContainedIn,
            IsGreaterThan,
            IsGreaterThanOrEqual,
            IsLessThan,
            IsLessThanOrEqual,
            IsEqualTo,
            IsNotEqualTo,
            StartsWith,
            EndsWith,
            IsNull,
            IsNotNull,
            IsEmpty,
            IsNotEmpty
        }

        public enum EventId
        {
            Unknown = 100,
            Exception_Application = 101,
            Exception_Database = 102,
            Exception_General = 103,
            Exception_Unhandled = 104,
            Exception_WebApi = 105,
            Exception_WebApiClient = 108,
            Exception_Synchronization = 110,
            Exception_Authenticate = 111,
            Warn_WebApi = 205,
            Warn_Mobile = 206,
            Warn_Web = 207,
            Warn_WebApiClient = 208,
            Warn_Synchronization = 210,
            Info_General = 300,
            Info_Diagnostics = 301,
            Info_Synchronization = 310,
            Unauthorized = 401,
            Authentication_Info = 500,
            Authentication_Fail = 501,
            Authentication_Success = 502,
            TemplateGenerationError = 1001,
            TemplateInitializeError = 1002,
            TemplateMetadataError = 1003,
            TemplateSettingError = 1004,
            FileTargetConflictError = 1005,
            FileTemplateOutputError = 1006,
            FileNotFound = 4004,
            DbContextLoad = 5001,
        }

        public enum HttpVerb
        {
            Delete, //Requests that a specified URI be deleted.
            Get, //Retrieves the information or entity that is identified by the URI of the request.
            Head, //Retrieves the message headers for the information or entity that is identified by the URI of the request.
            Options, //Represents a request for information about the communication options available on the request/response chain identified by the Request-URI.
            Patch, //Requests that a set of changes described in the request entity be applied to the resource identified by the Request- URI.
            Post, //Posts a new entity as an addition to a URI.
            Put //Replaces an entity that is identified by a URI.
        }

        [Flags]
        public enum PageContentElement
        {
            Title = 1,
            Meta = 2,
            H1 = 4,
            H2 = 8,
            H3 = 16,
            H4 = 32,
            H5 = 64,
            H6 = 128,
            Div = 256,
            Span = 512,
            Label = 1024,
            P = 2048,
            Img = 4096,
        }

        public enum RelatedEntitiesType
        {
            None = 0,
            ImmediateChildren = 1,
            SomeSpecialRule = 99
        }

        [Flags]
        public enum SearchIndexServiceUpdateMode
        {
            Append = 1,
            Replace = 2,
            UseLast = 4
        }

        [Flags]
        public enum SiteIndexStatusType
        {
            New = 1,
            Running = 2,
            Completed = 4,
            Failed = 8
        }
    }
}