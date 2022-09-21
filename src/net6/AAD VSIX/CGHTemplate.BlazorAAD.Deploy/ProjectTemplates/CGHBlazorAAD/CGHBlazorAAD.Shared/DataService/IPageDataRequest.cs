namespace $safeprojectname$.DataService
{
    using System.Collections.Generic;
    using enums = $safeprojectname$.Constants.Enums;

    public interface IPageDataRequest
    {
        IList<IFilterCriterion> FilterCriteria { get; set; }
        int Page { get; set; }
        int PageSize { get; set; }
        enums.RelatedEntitiesType RelatedEntitiesType { get; set; }
        string Sort { get; set; }
    }
}