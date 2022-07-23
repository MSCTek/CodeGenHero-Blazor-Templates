using System;

namespace ArtistSite.Repository.Infrastructure
{
    public class RepositoryPageDataResponse<T> : RepositoryPageDataResponse
    {
        public RepositoryPageDataResponse() : base()
        {
        }

        public RepositoryPageDataResponse(RepositoryPageDataRequest request)
            : base(request)
        {
        }

        public RepositoryPageDataResponse(T data) : base()
        {
            Data = data;
        }

        public T Data { get; set; }
    }

    public class RepositoryPageDataResponse : RepositoryPageDataRequest
    {
        public RepositoryPageDataResponse()
        {
        }

        public RepositoryPageDataResponse(RepositoryPageDataRequest request)
        {
            FieldList = request.FieldList;
            FilterList = request.FilterList;
            Page = request.Page;
            PageSize = request.PageSize;
            Sort = request.Sort;
            RelatedEntitiesType = request.RelatedEntitiesType;
        }

        public int TotalCount { get; set; }

        public int TotalPages
        {
            get
            {
                var retVal = 0;

                if (PageSize > 0)
                {
                    retVal = (int)Math.Ceiling((double)TotalCount / PageSize);
                }

                return retVal;
            }
        }
    }
}