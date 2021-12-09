using static $safeprojectname$.Constants.Enums;

namespace $safeprojectname$.DataService
{
    public interface IFilterCriterion
    {
        CriterionCondition Condition { get; set; }
        string FieldName { get; set; }
        object Value { get; set; }
    }
}