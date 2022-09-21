namespace $safeprojectname$.DataService
{
    using enums = $safeprojectname$.Constants.Enums;

    public class FilterCriterion : IFilterCriterion
    {
        public FilterCriterion(string fieldName, enums.CriterionCondition condition, object value)
        {
            FieldName = fieldName;
            Condition = condition;
            Value = value;
        }

        private FilterCriterion()
        {
        }

        public enums.CriterionCondition Condition { get; set; }
        public string FieldName { get; set; }
        public object Value { get; set; }
    }
}