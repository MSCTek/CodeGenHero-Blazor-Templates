namespace $safeprojectname$.DataService
{
    using System;
    using System.Net;

    public interface IHttpCallResultCGH
    {
        long? ContentLength { get; set; }
        string ContentType { get; set; }
        Exception Exception { get; set; }
        string FileName { get; set; }
        bool IsSuccessStatusCode { get; set; }
        string MediaType { get; set; }
        string ReasonPhrase { get; set; }
        string RequestUri { get; set; }
        HttpStatusCode StatusCode { get; set; }
    }
}