using Microsoft.Azure.Cosmos.Table;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AzureTableStorageIndexing
{
    public class IndexedFilterHelper<TObject> where TObject : IndexedTableEntity, new()
    {
        public static string AddFilterCondition(string targetFilter, string filterToAdd, string tableOperator)
        {
            if (filterToAdd.Contains("RowKey") && targetFilter.Contains("RowKey"))
                throw new ArgumentException(nameof(filterToAdd), "Cannot add multiple index queries");
            if (string.IsNullOrWhiteSpace(targetFilter))
                return filterToAdd;
            else
                return TableQuery.CombineFilters(targetFilter, tableOperator, filterToAdd);
        }

        private static bool hasIndexedPropertyAttribute(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttribute<IndexedPropertyAttribute>() != null;
        }

        public static string GetFilterCondition<TProperty>(Expression<Func<TObject, TProperty>> expression, string queryComparison, TProperty value)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var mExp = expression as MemberExpression;
            if (mExp == null)
                throw new ArgumentOutOfRangeException(nameof(expression), $"{nameof(expression)} must be a valid {nameof(MemberExpression)}");

            var acceptableTypes = new[]
            {
                typeof(DateTimeOffset),
                typeof(double),
                typeof(Guid),
                typeof(int),
                typeof(long),
                typeof(string)
            };
            var acceptableTypeNames = string.Join(", ", acceptableTypes.Select(x => acceptableTypes.ToString()));
            if (!acceptableTypes.Contains(typeof(TProperty)))
                throw new ArgumentOutOfRangeException(nameof(TProperty), $"Type of {nameof(TProperty)} must be one of the following: {acceptableTypes}.");

            var propertyName = mExp.Member.Name;
            var propertyInfo = typeof(TObject).GetProperty(propertyName);
            var isIndexedProperty = hasIndexedPropertyAttribute(propertyInfo);

            if(isIndexedProperty)
            {

                var queryStartValue = $"{propertyName}_{value}_";
                var queryEndValue = $"{propertyName}_{value}`";

                if(queryComparison == QueryComparisons.Equal)
                {
                    return TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThanOrEqual, $"{propertyName}_{value}_"),
                        TableOperators.And,
                        TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.LessThan, $"{propertyName}_{value}`")
                    );
                }
                else if(queryComparison == QueryComparisons.LessThan || queryComparison == QueryComparisons.LessThanOrEqual)
                {
                    return TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThan, $"{propertyName}_"),
                        TableOperators.And,
                        TableQuery.GenerateFilterCondition("RowKey", queryComparison, $"{propertyName}_{value}")
                    );
                }
                else if(queryComparison == QueryComparisons.GreaterThan || queryComparison == QueryComparisons.GreaterThanOrEqual)
                {
                    return TableQuery.GenerateFilterCondition("RowKey", queryComparison, $"{propertyName}_{value}");
                }
                else if(queryComparison == QueryComparisons.NotEqual)
                {
                    var condition1 = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThan, $"{propertyName}_");
                    var condition2 = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.LessThan, $"{propertyName}_{value}");
                    var condition3 = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThan, $"{propertyName}_{value}");
                    return TableQuery.CombineFilters(TableQuery.CombineFilters(condition1, TableOperators.And, condition2), TableOperators.And, condition3);
                }
                else
                {
                    var acceptableValues = new[]
                    {
                        QueryComparisons.Equal,
                        QueryComparisons.GreaterThan,
                        QueryComparisons.GreaterThanOrEqual,
                        QueryComparisons.LessThan,
                        QueryComparisons.LessThanOrEqual,
                        QueryComparisons.NotEqual
                    };
                    throw new ArgumentOutOfRangeException(nameof(queryComparison), $"Acceptable values: {string.Join(", ", acceptableValues)}");
                }
            }
            else
            {
                var valueType = typeof(TProperty);
                if(valueType == typeof(DateTimeOffset))
                {
                    return TableQuery.GenerateFilterConditionForDate(propertyName, queryComparison, DateTimeOffset.Parse(Convert.ToString(value)));
                }
                else if(valueType == typeof(double))
                {
                    return TableQuery.GenerateFilterConditionForDouble(propertyName, queryComparison, Convert.ToDouble(value));
                }
                else if(valueType == typeof(Guid))
                {
                    return TableQuery.GenerateFilterConditionForGuid(propertyName, queryComparison, Guid.Parse(Convert.ToString(value)));
                }
                else if(valueType == typeof(int))
                {
                    return TableQuery.GenerateFilterConditionForInt(propertyName, queryComparison, Convert.ToInt32(value));
                }
                else if(valueType == typeof(long))
                {
                    return TableQuery.GenerateFilterConditionForLong(propertyName, queryComparison, Convert.ToInt64(value));
                }
                else if(valueType == typeof(string))
                {
                    return TableQuery.GenerateFilterCondition(propertyName, queryComparison, Convert.ToString(value));
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(TProperty), $"Type of {nameof(TProperty)} must be one of the following: {acceptableTypes}.");
                }
            }


            //var propertyName = new PropertyNameVisitor().Visit(expression).ToString();

            //var filterProp = isIndexedProperty
            //    ? "RowKey"
            //    : propertyInfo.Name;

            //var startValue = isIndexedProperty
            //    ? $"{propertyInfo.Name}_{value}"
            //    : default;




        }



        private class PropertyNameVisitor : ExpressionVisitor
        {
            private string propertyName;

            protected override Expression VisitMember(MemberExpression node)
            {
                propertyName = node.Member.Name;
                return base.VisitMember(node);
            }

            public override string ToString()
            {
                return propertyName;
            }
        }
    }
}
