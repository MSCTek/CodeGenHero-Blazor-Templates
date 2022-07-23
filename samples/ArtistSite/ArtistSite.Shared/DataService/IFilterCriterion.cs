using static ArtistSite.Shared.Constants.Enums;

namespace ArtistSite.Shared.DataService
{
    public interface IFilterCriterion
    {
        CriterionCondition Condition { get; set; }
        string FieldName { get; set; }
        object Value { get; set; }
    }
}