namespace $safeprojectname$.DataService
{
    using static $safeprojectname$.Constants.Enums;

    public interface IFilterCriterion
    {
        CriterionCondition Condition { get; set; }
        string FieldName { get; set; }
        object Value { get; set; }
    }
}