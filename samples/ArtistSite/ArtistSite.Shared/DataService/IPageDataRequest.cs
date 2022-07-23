using System.Collections.Generic;
using enums = ArtistSite.Shared.Constants.Enums;

namespace ArtistSite.Shared.DataService
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