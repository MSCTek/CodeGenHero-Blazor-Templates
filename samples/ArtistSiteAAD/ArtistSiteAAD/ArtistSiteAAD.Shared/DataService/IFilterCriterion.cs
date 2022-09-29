using static ArtistSiteAAD.Shared.Constants.Enums;

namespace ArtistSiteAAD.Shared.DataService
{
    public interface IFilterCriterion
    {
        CriterionCondition Condition { get; set; }
        string FieldName { get; set; }
        object Value { get; set; }
    }
}