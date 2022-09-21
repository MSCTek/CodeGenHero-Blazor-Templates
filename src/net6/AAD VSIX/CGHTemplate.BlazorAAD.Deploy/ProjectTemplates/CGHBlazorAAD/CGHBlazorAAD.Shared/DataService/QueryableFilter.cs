namespace $safeprojectname$.DataService
{
    using enums = $safeprojectname$.Constants.Enums;

    public class QueryableFilter
    {
        public QueryableFilter(string member, enums.CriterionCondition condition, string value)
        {
            Member = member;
            Condition = condition;
            Value = value;
        }

        public enums.CriterionCondition Condition { get; set; }
        public string Member { get; set; }
        public string Value { get; set; }
    }
}