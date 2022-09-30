using ArtistSiteAAD.Shared.DataService;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using consts = ArtistSiteAAD.Shared.Constants.Consts;
using enums = ArtistSiteAAD.Shared.Constants.Enums;

namespace ArtistSiteAAD.Client.Services
{
    public abstract class WebApiDataServiceBase : IWebApiDataServiceBase
    {
        #region Constructor and Context

        public WebApiDataServiceBase(ILogger log,
            ISerializationHelper serializationHelper,
            IHttpClientFactory httpClientFactory,
            AuthenticationStateProvider authenticationStateProvider,
            string httpClientName = consts.HTTPCLIENTNAME_AUTHORIZED,
            string isServiceOnlineRelativeUrl = "APIStatus/")
        {
            Log = log;
            HttpClientFactory = httpClientFactory;
            SerializationHelper = serializationHelper;
            if (!string.IsNullOrWhiteSpace(httpClientName))
            {
                HttpClientName = httpClientName;
            }
            IsServiceOnlineRelativeUrl = isServiceOnlineRelativeUrl;
            AuthenticationStateProvider = authenticationStateProvider;
        }

        public IHttpClientFactory HttpClientFactory { get; set; }
        public virtual string IsServiceOnlineRelativeUrl { get; set; } = "";
        public virtual ILogger Log { get; set; }
        public virtual ISerializationHelper SerializationHelper { get; set; }
        protected string HttpClientName { get; set; } = default!;
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        #endregion Constructor and Context

        #region Convenience Methods

        protected virtual List<string> BuildFilter(IList<IFilterCriterion> filterCriteria)
        {
            var retVal = new List<string>();

            if (filterCriteria != null)
            {
                foreach (IFilterCriterion filterCriterion in filterCriteria)
                {
                    retVal.Add($"{filterCriterion.FieldName}{consts.API_FILTER_DELIMITER}{filterCriterion.Condition}{consts.API_FILTER_DELIMITER}{filterCriterion.Value}");
                }
            }

            return retVal;
        }

        protected virtual async Task<List<T>> GetAllPageDataResultsAsync<T>(IPageDataRequest pageDataRequest, Func<IPageDataRequest,
            Task<IHttpCallResultCGHT<IPageDataT<IList<T>>>>> getMethodToRun, bool throwExceptionOnFailureStatusCode = false)
        {
            List<T> retVal = new List<T>();
            IHttpCallResultCGHT<IPageDataT<IList<T>>> response = null;
            IPageDataRequest currentPageDataRequest = new PageDataRequest(pageDataRequest.FilterCriteria, pageDataRequest.Sort, pageDataRequest.Page, pageDataRequest.PageSize, pageDataRequest.RelatedEntitiesType);

            while (response == null
                || (response.IsSuccessStatusCode == true && currentPageDataRequest.Page <= response.Data.TotalPages))
            {
                response = await getMethodToRun(currentPageDataRequest);
                if (response.IsSuccessStatusCode && response.Data != null)
                {
                    retVal.AddRange(response.Data.Data);
                }
                else
                {
                    string serializedCurrentPageDataRequest = JsonConvert.SerializeObject(currentPageDataRequest);
                    string msg = $"{nameof(GetAllPageDataResultsAsync)} call resulted in error - status code: {response?.StatusCode}; reason: {response?.ReasonPhrase}. CurrentPageDataRequest: {serializedCurrentPageDataRequest}";

                    Log.LogWarning(eventId: (int)enums.EventId.Warn_WebApiClient,
                        exception: response?.Exception,
                        message: "Failure during WebApiClient to Web API GetAllPage operation with {ReturnTypeName}. ReasonPhrase: {ReasonPhrase}  HttpResponseStatusCode: {HttpResponseStatusCode} using CurrentPageDataRequest: {CurrentPageDataRequest}",
                        retVal?.GetType().Name, response?.ReasonPhrase, (int)response.StatusCode, serializedCurrentPageDataRequest);

                    if (throwExceptionOnFailureStatusCode == true)
                    {
                        throw new ApplicationException(msg, response?.Exception);
                    }
                }

                currentPageDataRequest.Page += 1;
            }

            return retVal;
        }

        #endregion Convenience Methods

        public async Task<HttpClient> GetHttpClientAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

            var clientName = (authState?.User?.Identity?.IsAuthenticated ?? false) ? HttpClientName : consts.HTTPCLIENTNAME_ANONYMOUS;

            var retVal = HttpClientFactory.CreateClient(clientName);
            return retVal;
        }

        public virtual async Task<bool> IsServiceOnlineAsync()
        {
            bool retVal = false;
            HttpResponseMessage response = null;

            try
            {
                var httpClient = await GetHttpClientAsync();
                response = await httpClient.GetAsync(IsServiceOnlineRelativeUrl);
                string content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    retVal = true;
                }
            }
            catch (Exception ex)
            {
                Log.LogError(eventId: (int)enums.EventId.Exception_WebApiClient,
                    exception: ex,
                    message: "Failure during WebApiClient to Web API {HttpVerb} operation with {ReturnTypeName}. HttpResponseStatusCode: {HttpResponseStatusCode} from RequestUri {RequestUri}",
                    Enum.GetName(typeof(enums.HttpVerb), enums.HttpVerb.Get),
                    retVal.GetType().Name,
                    response == null ? 0 : (int)response.StatusCode,
                    IsServiceOnlineRelativeUrl);
            }

            return retVal;
        }
    }
}