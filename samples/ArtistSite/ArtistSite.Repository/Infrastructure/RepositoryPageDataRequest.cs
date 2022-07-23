using System.Collections.Generic;
using enums = ArtistSite.Shared.Constants.Enums;

namespace ArtistSite.Repository.Infrastructure
{
    public class RepositoryPageDataRequest
    {
        public RepositoryPageDataRequest()
        {
        }

        public RepositoryPageDataRequest(IList<string> fieldList, IList<string> filterList,
            int page, int pageSize, string sort, enums.RelatedEntitiesType relatedEntitiesType)
        {
            FieldList = fieldList;
            FilterList = filterList;
            Page = page;
            PageSize = pageSize;
            Sort = sort;
            RelatedEntitiesType = relatedEntitiesType;
        }

        public IList<string> FieldList { get; set; } = new List<string>();

        public IList<string> FilterList { get; set; } = new List<string>();

        public int Page { get; set; }

        public int PageSize { get; set; }

        public enums.RelatedEntitiesType RelatedEntitiesType { get; set; }
        public string Sort { get; set; }
    }
}