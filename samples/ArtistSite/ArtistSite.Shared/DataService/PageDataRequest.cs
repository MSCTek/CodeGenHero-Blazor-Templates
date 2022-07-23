using System.Collections.Generic;
using enums = ArtistSite.Shared.Constants.Enums;

namespace ArtistSite.Shared.DataService
{
    public class PageDataRequest : IPageDataRequest
    {
        public PageDataRequest()
            : this(filterCriteria: new List<IFilterCriterion>())
        {
        }

        public PageDataRequest(IList<IFilterCriterion> filterCriteria,
            string sort = null, int page = 1, int pageSize = 100,
            enums.RelatedEntitiesType relatedEntitiesType = enums.RelatedEntitiesType.None)
        {
            this.FilterCriteria = filterCriteria;
            this.Sort = sort;
            this.Page = page;
            this.PageSize = pageSize;
            this.RelatedEntitiesType = relatedEntitiesType;
        }

        public IList<IFilterCriterion> FilterCriteria { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public enums.RelatedEntitiesType RelatedEntitiesType { get; set; }
        public string Sort { get; set; }
    }
}