using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using enums = ArtistSite.Shared.Constants.Enums;

namespace ArtistSite.Shared.DataService
{
    public interface ISerializationHelper
    {
        Task<IHttpCallResultCGHT<T>> MakeWebApiBSONCall<T>(enums.HttpVerb httpVerb, ILogger log, HttpClient client, string requestUri, T item) where T : class;

        Task<IHttpCallResultCGHT<T>> MakeWebApiFromBodyCall<T>(enums.HttpVerb httpVerb, ILogger log, HttpClient client, string requestUri, T item) where T : class;

        Task<IHttpCallResultCGHT<T>> SerializeCallResultsDelete<T>(ILogger log, HttpClient client, string webApiRequestUrl) where T : class;

        Task<IHttpCallResultCGHT<T>> SerializeCallResultsGet<T>(ILogger log, HttpClient client, string webApiRequestUrl) where T : class;

        Task<IHttpCallResultCGHT<IPageDataT<T>>> SerializeCallResultsGet<T>(ILogger log, HttpClient client,
            string webApiParameterlessPath, IList<string> filter, string sort, int page, int pageSize) where T : class;

        Task<IHttpCallResultCGHT<IPageDataT<T>>> SerializeCallResultsGet<T>(ILogger log, HttpClient client,
            string webApiParameterlessPath, IList<string> fields, IList<string> filter, string sort, int page, int pageSize) where T : class;

        Task<IHttpCallResultCGHT<T>> SerializeCallResultsPost<T>(ILogger log, HttpClient client, string requestUri, T item) where T : class;

        Task<IHttpCallResultCGHT<T>> SerializeCallResultsPut<T>(ILogger log, HttpClient client, string requestUri, T item) where T : class;
    }
}