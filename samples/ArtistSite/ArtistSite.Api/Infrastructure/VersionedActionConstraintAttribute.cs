using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ArtistSite.Api.Infrastructure
{
    /// <summary>
    /// Provides an attribute route that's restricted to a specific version of the api.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class VersionedActionConstraintAttribute : Attribute, IActionConstraint // RouteFactoryAttribute
    {
        public const string VersionHeaderName = "api-version";
        //private const int DefaultVersion = 1;

        public VersionedActionConstraintAttribute(int allowedVersion, int order)
        {
            AllowedVersion = allowedVersion;
            Order = order;
        }

        public VersionedActionConstraintAttribute(int allowedVersion, int order, int defaultVersion)
        {
            AllowedVersion = allowedVersion;
            Order = order;
            DefaultVersion = defaultVersion;
        }

        public int AllowedVersion
        {
            get;
            private set;
        }

        public int? DefaultVersion
        {
            get;
            private set;
        }

        public int Order
        {
            get;
            private set;
        }

        //public override IDictionary<string, object> Constraints
        //{
        //	get
        //	{
        //		var constraints = new HttpRouteValueDictionary();
        //		constraints.Add("version", new VersionConstraint(AllowedVersion));
        //		return constraints;
        //	}
        //}

        public bool Accept(ActionConstraintContext actionConstraintContext)
        {
            var httpContext = actionConstraintContext.RouteContext.HttpContext;

            // try custom request header "api-version"
            int? version = GetVersionHeaderOrQuery(httpContext.Request);

            // not found?  Try custom content type in accept header
            if (version == null)
            {
                version = GetVersionFromCustomContentType(httpContext.Request);
            }

            // could simply permit default version here, but for now we want to ensure clients are making requests for a particular version #.
            //return ((version ?? DefaultVersion) == AllowedVersion);

            if (!version.HasValue && DefaultVersion.HasValue)
            {
                version = DefaultVersion.Value;
            }

            if (version.HasValue && version.Value == AllowedVersion)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private int? GetVersionFromCustomContentType(HttpRequest request)
        {
            string versionAsString = null;

            // get the accept header.

            // var mediaTypes = request.Headers.Accept.Select(h => h.MediaType);
            StringValues mediaTypes = request.Headers["Accept"];
            if (mediaTypes == StringValues.Empty)
                return null;

            string matchingMediaType = null;
            // find the one with the version number - match through regex
            Regex regEx = new Regex(@"application\/vnd\.api\.v([\d]+)\+json"); // "application/vnd.api.v2+json"

            foreach (var mediaType in mediaTypes)
            {
                if (regEx.IsMatch(mediaType))
                {
                    matchingMediaType = mediaType;
                    break;
                }
            }

            if (matchingMediaType == null)
                return null;

            // extract the version number
            Match m = regEx.Match(matchingMediaType);
            versionAsString = m.Groups[1].Value;

            // ... and return
            if (!string.IsNullOrWhiteSpace(versionAsString)
                && Int32.TryParse(versionAsString, out int version))
            {
                return version;
            }

            return null;
        }

        /// <summary>
        /// Check the request header, and the query string to determine if a version number has been provided
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private int? GetVersionHeaderOrQuery(HttpRequest request)
        {
            string versionAsString;

            if (request.Headers.TryGetValue(VersionHeaderName, out var headerValues)
                && headerValues.Count() == 1)
            {
                versionAsString = headerValues.First();
                if (versionAsString != null && Int32.TryParse(versionAsString, out int version))
                {
                    return version;
                }
            }
            else
            {
                if (request.Query.ContainsKey(VersionHeaderName))
                {
                    string versionStr = request.Query[VersionHeaderName];
                    int.TryParse(versionStr, out int version);

                    if (version > 0)
                        return version;
                }
            }

            return null;
        }
    }
}