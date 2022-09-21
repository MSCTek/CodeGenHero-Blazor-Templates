namespace $safeprojectname$.Infrastructure
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;
    using System;
    using enums = $ext_safeprojectname$.Shared.Constants.Enums;

    public class RelatedEntitiesTypeRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            // retrieve the candidate value
            var candidate = values[routeKey]?.ToString();

            // attempt to parse the candidate to the required Enum type, and return the result
            var retVal = (candidate == (enums.RelatedEntitiesType.None).ToString())
                || Enum.TryParse(candidate, out enums.RelatedEntitiesType result);
            return retVal;
        }
    }
}