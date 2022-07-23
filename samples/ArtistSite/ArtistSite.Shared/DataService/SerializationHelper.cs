using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using enums = ArtistSite.Shared.Constants.Enums;

namespace ArtistSite.Shared.DataService
{
    public partial class SerializationHelper : ISerializationHelper
    {
        public const string MEDIATYPEHEADERVALUE_BSON = "application/bson";
        //private static readonly Lazy<SerializationHelper> _lazyInstance = new Lazy<SerializationHelper>(() => new SerializationHelper());

        public SerializationHelper()
        {
        }

        //public static SerializationHelper Instance { get { return _lazyInstance.Value; } }

        public static byte[] SerializeBson<T>(T obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BsonDataWriter writer = new BsonDataWriter(ms))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(writer, obj);
                }

                //string data = Convert.ToBase64String(ms.ToArray());
                return ms.ToArray();
            }
        }

        public virtual async Task<IHttpCallResultCGHT<T>> MakeWebApiBSONCall<T>(
                    enums.HttpVerb httpVerb, ILogger log, HttpClient client, string requestUri, T item) where T : class
        {
            T retValData = default(T);
            HttpCallResult<T> retVal = new HttpCallResult<T>();

            try
            {
                HttpResponseMessage response = null;
                if (httpVerb == enums.HttpVerb.Post)
                {
                    response = await PostBsonAsync<T>(client, requestUri, item);
                }
                else
                {
                    throw new NotImplementedException();
                }

                if (response.IsSuccessStatusCode)
                {
                    using (BsonDataReader reader = new BsonDataReader(await response.Content.ReadAsStreamAsync()))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        retValData = serializer.Deserialize<T>(reader);
                    }
                }
                else
                {
                    log.LogWarning(eventId: (int)enums.EventId.Warn_WebApiClient,
                        message: "Failure during WebApiClient to Web API {HttpVerb} operation with {ReturnTypeName}. HttpResponseStatusCode: {HttpResponseStatusCode} from RequestUri {RequestUri}",
                        Enum.GetName(typeof(enums.HttpVerb), httpVerb),
                        retVal?.GetType().Name,
                        (int)response.StatusCode,
                        requestUri);
                }

                retVal = new HttpCallResult<T>(
                    retValData, requestUri,
                    response.IsSuccessStatusCode, response.StatusCode, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                retVal.Exception = ex;
            }

            return retVal;
        }

        public virtual async Task<IHttpCallResultCGHT<T>> MakeWebApiFromBodyCall<T>(
            enums.HttpVerb httpVerb, ILogger log, HttpClient client, string requestUri, T item) where T : class
        {
            T retValData = default(T);
            System.Net.Http.HttpResponseMessage response = null;
            HttpCallResult<T> retVal = new HttpCallResult<T>();

            try
            {
                string serializedItem = JsonConvert.SerializeObject(item);
                var inputMessage = new System.Net.Http.HttpRequestMessage
                {
                    Content = new System.Net.Http.StringContent(serializedItem, System.Text.Encoding.UTF8, "application/json")
                };

                inputMessage.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpVerb == enums.HttpVerb.Post)
                {
                    response = await client.PostAsync(requestUri, inputMessage.Content);
                }
                else if (httpVerb == enums.HttpVerb.Put)
                {
                    response = await client.PutAsync(requestUri, inputMessage.Content);
                }
                else
                {
                    throw new NotImplementedException();
                }

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content.ReadAsStringAsync();
                    retValData = JsonConvert.DeserializeObject<T>(responseContent.Result);
                }
                else
                {
                    log.LogWarning(eventId: (int)enums.EventId.Warn_WebApiClient,
                        message: "Failure during WebApiClient to Web API {HttpVerb} operation with {ReturnTypeName}. HttpResponseStatusCode: {HttpResponseStatusCode} from RequestUri {RequestUri}",
                        Enum.GetName(typeof(enums.HttpVerb), httpVerb),
                        retVal?.GetType().Name,
                        (int)response.StatusCode,
                        requestUri);
                }

                retVal = new HttpCallResult<T>(
                    retValData, requestUri,
                    response.IsSuccessStatusCode, response.StatusCode, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                log.LogError(eventId: (int)enums.EventId.Exception_WebApiClient,
                    exception: ex,
                    message: "Failure during WebApiClient to Web API {HttpVerb} operation with {ReturnTypeName}. HttpResponseStatusCode: {HttpResponseStatusCode} from RequestUri {RequestUri}",
                    Enum.GetName(typeof(enums.HttpVerb), httpVerb),
                    retVal?.GetType().Name,
                    response == null ? 0 : (int)response.StatusCode,
                    requestUri);

                retVal = new HttpCallResult<T>(data: null, requestUri: requestUri, isSuccessStatusCode: response != null ? response.IsSuccessStatusCode : false,
                    statusCode: response != null ? response.StatusCode : System.Net.HttpStatusCode.InternalServerError,
                    reasonPhrase: response != null ? response.ReasonPhrase : ex.Message, exception: ex);
            }

            return retVal;
        }

        public virtual async Task<IHttpCallResultCGHT<T>> SerializeCallResultsDelete<T>(
            ILogger log, HttpClient client, string url) where T : class
        {
            var retVal = new HttpCallResult<T>
            {
                IsSuccessStatusCode = false
            };

            HttpResponseMessage response = null;

            try
            {
                response = await client.DeleteAsync(url);
                // A successful response is essentially "No Content" so there is nothing to deserialize here.
                if (!response.IsSuccessStatusCode)
                {
                    log.LogWarning(eventId: (int)enums.EventId.Warn_WebApiClient,
                        message: "Failure during WebApiClient to Web API delete operation with {ReturnTypeName}. ReasonPhrase: {ReasonPhrase} HttpResponseStatusCode: {HttpResponseStatusCode} from Url {Url}",
                        retVal?.GetType().Name,
                        response.ReasonPhrase,
                        (int)response.StatusCode,
                        url);
                }

                retVal = new HttpCallResult<T>(
                    null, url,
                    response.IsSuccessStatusCode, response.StatusCode, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                log.LogError(eventId: (int)enums.EventId.Exception_WebApiClient,
                    exception: ex,
                    message: "Failure during WebApiClient to Web API delete operation with {ReturnTypeName}. ReasonPhrase: {ReasonPhrase}  HttpResponseStatusCode: {HttpResponseStatusCode} from Url {Url}",
                    retVal?.GetType().Name,
                    response?.ReasonPhrase,
                    response == null ? 0 : (int)response.StatusCode,
                    url);

                retVal = new HttpCallResult<T>(data: null, requestUri: url, isSuccessStatusCode: response != null ? response.IsSuccessStatusCode : false,
                    statusCode: response != null ? response.StatusCode : System.Net.HttpStatusCode.InternalServerError,
                    reasonPhrase: response != null ? response.ReasonPhrase : ex.Message, exception: ex);
            }

            return retVal;
        }

        public virtual async Task<IHttpCallResultCGHT<T>> SerializeCallResultsGet<T>(
            ILogger log, HttpClient client, string url) where T : class
        {
            var retVal = new HttpCallResult<T>();
            T retValData = default(T);
            retVal.IsSuccessStatusCode = false;

            HttpResponseMessage response = null;

            try
            {
                response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    retValData = JsonConvert.DeserializeObject<T>(content); // new GuidBoolJsonConverter());
                }
                else
                {
                    log.LogWarning(eventId: (int)enums.EventId.Warn_WebApiClient,
                        message: "Failure during WebApiClient to Web API get operation with {ReturnTypeName}. ReasonPhrase: {ReasonPhrase}  HttpResponseStatusCode: {HttpResponseStatusCode} from Url {Url}",
                        retVal?.GetType().Name,
                        response?.ReasonPhrase,
                        (int)response.StatusCode,
                        url);
                }

                retVal = new HttpCallResult<T>(
                    retValData, url,
                    response.IsSuccessStatusCode, response.StatusCode, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                log.LogError(eventId: (int)enums.EventId.Exception_WebApiClient,
                    exception: ex,
                    message: "Failure during WebApiClient to Web API get operation with {ReturnTypeName}. ReasonPhrase: {ReasonPhrase}  HttpResponseStatusCode: {HttpResponseStatusCode} from Url {Url}",
                    retVal?.GetType().Name,
                    response?.ReasonPhrase,
                    response == null ? 0 : (int)response.StatusCode,
                    url);

                retVal = new HttpCallResult<T>(data: null, requestUri: url, isSuccessStatusCode: response != null ? response.IsSuccessStatusCode : false,
                    statusCode: response != null ? response.StatusCode : System.Net.HttpStatusCode.InternalServerError,
                    reasonPhrase: response != null ? response.ReasonPhrase : ex.Message, exception: ex);
            }

            return retVal;
        }

        public virtual async Task<IHttpCallResultCGHT<IPageDataT<T>>> SerializeCallResultsGet<T>(
            ILogger log, HttpClient client, string webApiParameterlessPath,
            IList<string> filter, string sort, int page, int pageSize) where T : class
        {
            var retVal = new HttpCallResult<IPageDataT<T>>();
            T retValData = default(T);
            retVal.IsSuccessStatusCode = false;

            HttpResponseMessage response = null;
            string requestUri = null;
            PageData<T> pageData = null;

            try
            {
                requestUri = AddFilterToPath(webApiParameterlessPath, filter);
                requestUri = AddSortToPath(requestUri, sort);
                requestUri = AddPagingToPath(requestUri, page, pageSize);
                // client.DefaultRequestHeaders.Add("api-version", "1");
                System.Diagnostics.Debug.WriteLine($"Http Get for: {client.BaseAddress.AbsoluteUri}{requestUri}");
                response = await client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead); // Performance optimization: https://www.stevejgordon.co.uk/using-httpcompletionoption-responseheadersread-to-improve-httpclient-performance-dotnet

                response.EnsureSuccessStatusCode();
                if (response.Content is object)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    using (StreamReader sr = new StreamReader(responseStream))
                    using (JsonReader reader = new JsonTextReader(sr))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        retValData = serializer.Deserialize<T>(reader);
                    }
                }

                pageData = new PageData<T>(retValData);
                var headers = response.Headers;
                headers.TryGetValues("X-Pagination", out IEnumerable<string> values);

                var e = values.GetEnumerator();
                if (e.MoveNext())
                {
                    var x = JsonConvert.DeserializeObject<PageData>(e.Current);
                    pageData.CurrentPage = x.CurrentPage;
                    pageData.NextPageLink = x.NextPageLink;
                    pageData.PageSize = x.PageSize;
                    pageData.PreviousPageLink = x.PreviousPageLink;
                    pageData.TotalCount = x.TotalCount;
                    pageData.TotalPages = x.TotalPages;
                }

                retVal = new HttpCallResult<IPageDataT<T>>(data: pageData, requestUri: requestUri, isSuccessStatusCode: response.IsSuccessStatusCode,
                    statusCode: response.StatusCode, reasonPhrase: response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                log.LogError(eventId: (int)enums.EventId.Exception_WebApiClient,
                    exception: ex,
                    message: "Failure during WebApiClient to Web API get operation with {ReturnTypeName}. ReasonPhrase: {ReasonPhrase}  HttpResponseStatusCode: {HttpResponseStatusCode} from RequestUri {RequestUri}",
                    retVal?.GetType().Name,
                    response?.ReasonPhrase,
                    response == null ? 0 : (int)response.StatusCode,
                    requestUri);

                retVal = new HttpCallResult<IPageDataT<T>>(data: null, requestUri: requestUri, isSuccessStatusCode: response != null ? response.IsSuccessStatusCode : false,
                    statusCode: response != null ? response.StatusCode : System.Net.HttpStatusCode.InternalServerError,
                    reasonPhrase: response != null ? response.ReasonPhrase : ex.Message, exception: ex);
            }

            return retVal;
        }

        public virtual async Task<IHttpCallResultCGHT<IPageDataT<T>>> SerializeCallResultsGet<T>(
            ILogger log, HttpClient client, string webApiParameterlessPath, IList<string> fields,
            IList<string> filter, string sort, int page, int pageSize) where T : class
        {
            var retVal = new HttpCallResult<IPageDataT<T>>();
            T retValData = default(T);
            retVal.IsSuccessStatusCode = false;

            HttpResponseMessage response = null;
            string requestUri = null;
            PageData<T> pageData = null;

            try
            {
                requestUri = AddFilterToPath(webApiParameterlessPath, filter);
                requestUri = AddSortToPath(requestUri, sort);
                requestUri = AddPagingToPath(requestUri, page, pageSize);
                requestUri = AddFieldsToPath(requestUri, fields);
                Stream responseStream = await client.GetStreamAsync(requestUri);
                using (StreamReader sr = new StreamReader(responseStream))
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    retValData = serializer.Deserialize<T>(reader);
                }

                pageData = new PageData<T>(retValData);

                var headers = response.Headers;
                headers.TryGetValues("X-Pagination", out IEnumerable<string> values);

                var e = values.GetEnumerator();
                if (e.MoveNext())
                {
                    var x = JsonConvert.DeserializeObject<PageData>(e.Current);
                    pageData.CurrentPage = x.CurrentPage;
                    pageData.NextPageLink = x.NextPageLink;
                    pageData.PageSize = x.PageSize;
                    pageData.PreviousPageLink = x.PreviousPageLink;
                    pageData.TotalCount = x.TotalCount;
                    pageData.TotalPages = x.TotalPages;
                }

                retVal = new HttpCallResult<IPageDataT<T>>(data: pageData, requestUri: requestUri, isSuccessStatusCode: response.IsSuccessStatusCode,
                    statusCode: response.StatusCode, reasonPhrase: response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                log.LogError(eventId: (int)enums.EventId.Exception_WebApiClient,
                    exception: ex,
                    message: "Failure during WebApiClient to Web API get operation with {ReturnTypeName}. ReasonPhrase: {ReasonPhrase}  HttpResponseStatusCode: {HttpResponseStatusCode} from RequestUri {RequestUri}",
                    retVal?.GetType().Name,
                    response?.ReasonPhrase,
                    response == null ? 0 : (int)response.StatusCode,
                    requestUri);

                retVal = new HttpCallResult<IPageDataT<T>>(data: null, requestUri: requestUri, isSuccessStatusCode: response != null ? response.IsSuccessStatusCode : false,
                    statusCode: response != null ? response.StatusCode : System.Net.HttpStatusCode.InternalServerError,
                    reasonPhrase: response != null ? response.ReasonPhrase : ex.Message, exception: ex);
            }

            return retVal;
        }

        public virtual async Task<IHttpCallResultCGHT<T>> SerializeCallResultsPost<T>(
            ILogger log, HttpClient client, string requestUri, T item) where T : class
        {
            return await MakeWebApiFromBodyCall<T>(enums.HttpVerb.Post, log, client, requestUri, item);
        }

        public virtual async Task<IHttpCallResultCGHT<T>> SerializeCallResultsPut<T>(
            ILogger log, HttpClient client, string requestUri, T item) where T : class
        {
            return await MakeWebApiFromBodyCall<T>(enums.HttpVerb.Put, log, client, requestUri, item);
        }

        private string AddFieldsToPath(string requestURL, IList<string> fields)
        {
            string retVal = requestURL;
            if (fields.Count == 0)
            {
                return retVal;
            }

            if (!retVal.Contains("?"))
            {
                retVal = $"{retVal}?";
            }
            else
            {
                retVal = $"{retVal}&";
            }

            return $"{retVal}fields={string.Join(",", fields)}";
        }

        private string AddFilterToPath(string requestURL, IList<string> filter)
        {
            string retVal = requestURL;
            if (filter == null || filter.Count == 0)
                return retVal;

            if (!retVal.Contains("?"))
                retVal = $"{retVal}?";
            else
                retVal = $"{retVal}&";

            retVal = $"{retVal}filter=";

            for (int i = 0; i < filter.Count; i++)
            {
                if (i > 0)
                    retVal = $"{retVal},";

                retVal = $"{retVal}{filter[i]}";
            }

            return retVal;
        }

        private string AddPagingToPath(
            string requestURL, int page, int pageSize)
        {
            string retVal = requestURL;

            if (!retVal.Contains("?"))
                retVal = $"{retVal}?";
            else
                retVal = $"{retVal}&";

            retVal = $"{retVal}page={page}&pageSize={pageSize}";

            return retVal;
        }

        private string AddSortToPath(string requestURL, string sort)
        {
            string retVal = requestURL;
            if (string.IsNullOrWhiteSpace(sort))
                return retVal;

            if (!retVal.Contains("?"))
                retVal = $"{retVal}?";
            else
                retVal = $"{retVal}&";

            retVal = $"{retVal}sort={sort}";

            return retVal;
        }

        private async Task<HttpResponseMessage> PostBsonAsync<T>(
            HttpClient client, string url, T data)
        {
            //Specifiy 'Accept' header As BSON: to ask server to return data as BSON format
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue(MEDIATYPEHEADERVALUE_BSON));

            //Specify 'Content-Type' header: to tell server which format of the data will be posted
            //Post data will be as Bson format
            var bsonData = SerializeBson<T>(data);
            var byteArrayContent = new ByteArrayContent(bsonData);
            byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue(MEDIATYPEHEADERVALUE_BSON);

            var response = await client.PostAsync(url, byteArrayContent);
            return response;
        }
    }
}