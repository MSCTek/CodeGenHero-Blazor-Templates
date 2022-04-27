using $ext_safeprojectname$.Shared.DataService;
using $ext_safeprojectname$.Shared.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using enums = $ext_safeprojectname$.Shared.Constants.Enums;

namespace $safeprojectname$.Infrastructure
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> source, QueryableFilter filter)
        {
            var propertyInfoMatch = typeof(T).GetProperty(filter.Member, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            if (propertyInfoMatch == null)
                throw new ArgumentException($"Unexpected filter field of {filter.Member} does not match a property on object {typeof(T).ToString()}");

            bool isCollection = propertyInfoMatch.PropertyType != typeof(string) &&
                typeof(IEnumerable).IsAssignableFrom(propertyInfoMatch.PropertyType);
            string fullName = propertyInfoMatch.PropertyType.FullName;
            bool isDateTime = fullName.Contains("System.DateTime");  // Contains handles nullable fields.
            bool isGuid = fullName.Contains("System.Guid");  // Contains handles nullable fields.
            bool isBoolean = fullName.Contains("System.Boolean");  // Contains handles nullable fields.
            bool isShort = fullName.Contains("System.Int16");
            bool isInt = fullName.Contains("System.Int32");
            bool isFilterAnArray = filter.Value.Contains("|"); // multiple criteria come in comma-delimited, so the pipe character is used to separate multiple values within a filter value (i.e. for a contains clause).
            Func<string, string, string> condition;

            if (isCollection)
                condition = (format, member) => string.Format("{0}.Any({1})", member, string.Format(format, "it")); // System.Linq.Dynamic Collection:  {PropertyName}.Any(it{condition})
            else
                condition = (format, member) => string.Format(format, member); // System.Linq.Dynamic Object:  {PropertyName}{condition}

            object typedFilterValue = filter.Value;
            
            if (isDateTime)
            {
                if (DateTime.TryParse(filter.Value, result: out var parseOutput))
                    typedFilterValue = parseOutput;
            }
            else if (isGuid)
            {
                if (Guid.TryParse(filter.Value, result: out var parseOutput))
                    typedFilterValue = parseOutput;
            }
            else if (isBoolean)
            {
                if (bool.TryParse(filter.Value, result: out var parseOutput))
                    typedFilterValue = parseOutput;
            }
            else if (isShort)
            {
                if (Int16.TryParse(filter.Value, result: out var parseOutput))
                    typedFilterValue = parseOutput;
            }
            else if (isInt)
            {
                if (!isFilterAnArray)
                {
                    if (Int32.TryParse(filter.Value, result: out var parseOutput))
                        typedFilterValue = parseOutput;
                }
            }

            switch (filter.Condition)
            {
                case enums.CriterionCondition.IsContainedIn:
                    {
                        var type = propertyInfoMatch.PropertyType;
                        var filterArray = ConvertFilterValueToArray(filter.Value, type);

                        source = source.Where(String.Format("x => @0.Contains(x.{0})", filter.Member), filterArray);
                        break;
                    }

                case enums.CriterionCondition.IsNotContainedIn:
                    {
                        var type = propertyInfoMatch.PropertyType;
                        var filterArray = ConvertFilterValueToArray(filter.Value, type);

                        source = source.Where(String.Format("x => !@0.Contains(x.{0})", filter.Member), filterArray);
                        break;
                    }

                case enums.CriterionCondition.IsGreaterThanOrEqual:
                    source = source.Where(condition("{0} >= (@0)", filter.Member), typedFilterValue);
                    break;

                case enums.CriterionCondition.IsLessThanOrEqual:
                    source = source.Where(condition("{0} <= (@0)", filter.Member), typedFilterValue);
                    break;

                case enums.CriterionCondition.IsGreaterThan:
                    source = source.Where(condition("{0} > (@0)", filter.Member), typedFilterValue);
                    break;

                case enums.CriterionCondition.IsLessThan:
                    source = source.Where(condition("{0} < (@0)", filter.Member), typedFilterValue);
                    break;

                case enums.CriterionCondition.Contains:
                    if (isInt && isFilterAnArray)
                    {
                        var containsArray = filter.Value.Split("|").Select(x => Int32.Parse(x)).ToArray();
                        source = source.Where(String.Format("x => @0.Contains(x.{0})", filter.Member), containsArray);
                        //source = source.Where(conditionIntArray("@0.Contains(outerIt.{0})", filterValuesArray), filter.Member); outerIt
                    }
                    else
                    {
                        source = source.Where(condition("{0}.Contains(@0)", filter.Member), typedFilterValue);
                    }
                    break;

                case enums.CriterionCondition.DoesNotContain:
                    source = source.Where(condition("!{0}.Contains(@0)", filter.Member), typedFilterValue);
                    break;

                case enums.CriterionCondition.IsEqualTo:
                    source = source.Where(condition("{0} = @0", filter.Member), typedFilterValue);
                    break;

                case enums.CriterionCondition.IsNotEqualTo:
                    source = source.Where(condition("{0} != @0", filter.Member), typedFilterValue);
                    break;

                case enums.CriterionCondition.StartsWith:
                    source = source.Where(condition("{0}.StartsWith(@0)", filter.Member), typedFilterValue);
                    break;

                case enums.CriterionCondition.EndsWith:
                    source = source.Where(condition("{0}.EndsWith(@0)", filter.Member), typedFilterValue);
                    break;

                case enums.CriterionCondition.IsNull:
                    source = source.Where(condition("{0} = NULL", filter.Member));
                    break;

                case enums.CriterionCondition.IsNotNull:
                    source = source.Where(condition("{0} != NULL", filter.Member));
                    break;

                case enums.CriterionCondition.IsEmpty:
                    source = source.Where(condition("string.IsNullOrEmpty({0})", filter.Member));
                    break;

                case enums.CriterionCondition.IsNotEmpty:
                    source = source.Where(condition("!string.IsNullOrEmpty({0})", filter.Member));
                    break;
            }

            return source;
        }

        public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> source, IList<string> filterList)
        {
            for (int i = 0; i < filterList.Count(); i++)
            {
                var filter = filterList[i];
                var indexOfFirstDelimiter = filter.IndexOf("~");
                string filterField = filter.Substring(0, indexOfFirstDelimiter);
                var indexOfSecondDelimiter = filter.IndexOf("~", indexOfFirstDelimiter + 1);
                string operandString = filter.Substring(indexOfFirstDelimiter + 1, indexOfSecondDelimiter - indexOfFirstDelimiter - 1);
                string filterValue = filter.Substring(indexOfSecondDelimiter + 1, filter.Length - indexOfSecondDelimiter - 1);
                var operand = (enums.CriterionCondition)Enum.Parse(typeof(enums.CriterionCondition), operandString, true);

                source = source.ApplyFilter(new QueryableFilter(filterField, operand, filterValue)); //.Where(x => x.UpdatedDate >= updatedDate);
            }

            return source;
        }

        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string sort)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (sort == null)
            {
                return source;
            }

            // split the sort string
            var lstSort = sort.Split(',');

            // run through the sorting options and create a sort expression string from them

            string completeSortExpression = "";
            for (int i = 0; i < lstSort.Length; i++)
            {
                var sortOption = lstSort[i].Trim();
                // if the sort option starts with "-", we order
                // descending, otherwise ascending

                if (sortOption.StartsWith("-"))
                {
                    completeSortExpression = completeSortExpression + sortOption.Remove(0, 1) + " descending,";
                }
                else
                {
                    completeSortExpression = completeSortExpression + sortOption + ",";
                }
            }

            if (!string.IsNullOrWhiteSpace(completeSortExpression))
            {
                source = source.OrderBy(completeSortExpression.Remove(completeSortExpression.Count() - 1));
            }

            return source;
        }

        private static Array ConvertFilterValueToArray(string filterValue, Type type)
        {
            var filterStrings = filterValue.Split("|");
            var convertMethod = typeof(ObjectExtensions).GetMethod(nameof(ObjectExtensions.ConvertTo)).MakeGenericMethod(type);
            var filterArray = Array.CreateInstance(type, filterStrings.Length);
            for (var i = 0; i < filterStrings.Length; i++)
            {
                var parameters = new object[] { filterStrings[i] };
                filterArray.SetValue(convertMethod.Invoke(null, parameters: parameters), i);
            }

            return filterArray;
        }
    }
}