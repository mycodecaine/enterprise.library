namespace Cdcn.Enterprise.Library.Domain.Data
{
    /// <summary>
    /// Represents a collection of SQL filter conditions for querying data of type <typeparamref name="TEntity"/>.
    /// This class allows adding conditions, wildcard conditions, and date range conditions, and generates SQL query strings and parameters.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to filter. Must be a class.</typeparam>
    public class SqlQueryFilter<TEntity> where TEntity : class
    {
        /// <summary>
        /// Stores the list of SQL conditions as strings.
        /// </summary>
        private readonly List<string> _conditions = new List<string>();

        /// <summary>
        /// Stores the parameters for the SQL conditions as key-value pairs.
        /// </summary>
        private readonly Dictionary<string, object> _parameters = new Dictionary<string, object>();

        /// <summary>
        /// Gets the number of conditions added to the filter.
        /// </summary>
        public int ConditionCount { get; internal set; } = 0;

        /// <summary>
        /// Adds a condition to filter by a specific column and value.
        /// </summary>
        /// <param name="columnName">The name of the column to filter.</param>
        /// <param name="value">The value to compare the column against.</param>
        public void AddCondition(string columnName, object value)
        {
            string parameterName = $"@{columnName}";
            _conditions.Add($"{columnName} = {parameterName}");
            _parameters.Add(parameterName, value);
            ConditionCount = _conditions.Count;
        }

        /// <summary>
        /// Adds a wildcard condition to filter a column using the LIKE operator.
        /// </summary>
        /// <param name="columnName">The name of the column to filter.</param>
        /// <param name="value">The value to use for the wildcard search. The value will be wrapped with '%' to perform a partial match.</param>
        public void AddWildcardCondition(string columnName, string value)
        {
            string parameterName = $"@{columnName}";
            _conditions.Add($"{columnName} LIKE {parameterName}");
            _parameters.Add(parameterName, $"%{value}%");
            ConditionCount = _conditions.Count;
        }

        /// <summary>
        /// Adds a date range condition to filter a column between two dates.
        /// </summary>
        /// <param name="columnName">The name of the column to filter.</param>
        /// <param name="startDate">The start date of the range. If null, only the end date will be used.</param>
        /// <param name="endDate">The end date of the range. If null, only the start date will be used.</param>
        public void AddDateRangeCondition(string columnName, DateTime? startDate, DateTime? endDate)
        {
            if (startDate.HasValue && endDate.HasValue)
            {
                _conditions.Add($"{columnName} BETWEEN @StartDate AND @EndDate");
                _parameters.Add("@StartDate", startDate.Value);
                _parameters.Add("@EndDate", endDate.Value);
            }
            else if (startDate.HasValue)
            {
                _conditions.Add($"{columnName} >= @StartDate");
                _parameters.Add("@StartDate", startDate.Value);
            }
            else if (endDate.HasValue)
            {
                _conditions.Add($"{columnName} <= @EndDate");
                _parameters.Add("@EndDate", endDate.Value);
            }
            ConditionCount = _conditions.Count;
        }

        /// <summary>
        /// Generates a SQL WHERE clause by combining all added conditions with the AND operator.
        /// </summary>
        /// <returns>A SQL WHERE clause as a string.</returns>
        public string ToSql()
        {
            return string.Join(" AND ", _conditions);
        }

        /// <summary>
        /// Retrieves the parameters for the SQL conditions.
        /// </summary>
        /// <returns>A dictionary of parameter names and their corresponding values.</returns>
        public Dictionary<string, object> GetParameters()
        {
            return _parameters;
        }
    }
}
