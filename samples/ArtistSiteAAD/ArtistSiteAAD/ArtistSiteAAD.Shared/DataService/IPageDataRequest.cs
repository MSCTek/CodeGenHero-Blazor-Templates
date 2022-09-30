using System.Collections.Generic;
using enums = ArtistSiteAAD.Shared.Constants.Enums;

namespace ArtistSiteAAD.Shared.DataService
{
    public interface IPageDataRequest
    {
        IList<IFilterCriterion> FilterCriteria { get; set; }
        int Page { get; set; }
        int PageSize { get; set; }
        enums.RelatedEntitiesType RelatedEntitiesType { get; set; }
        string Sort { get; set; }
    }
}