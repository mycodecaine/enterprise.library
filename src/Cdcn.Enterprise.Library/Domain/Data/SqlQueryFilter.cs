using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Domain.Data
{
    public class SqlQueryFilter<TEntity> where TEntity : class
    {
        private readonly List<string> _conditions = new List<string>();
        private readonly Dictionary<string, object> _parameters = new Dictionary<string, object>();
        public int ConditionCount { get; internal set; } = 0;
        public void AddCondition(string columnName, object value)
        {
            string parameterName = $"@{columnName}";
            _conditions.Add($"{columnName} ={parameterName}");
            _parameters.Add(parameterName, value);
            ConditionCount = _conditions.Count;
        }

        public void AddWildcardCondition(string columnName, string value)
        {
            string parameterName = $"@{columnName}";
            _conditions.Add($"{columnName} LIKE {parameterName}");
            _parameters.Add(parameterName, $"%{value}%");
            ConditionCount = _conditions.Count;
        }

        // Add method to handle date range filtering
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

        public string ToSql()
        {
            return string.Join(" AND ", _conditions);
        }

        public Dictionary<string, object> GetParameters()
        {
            return _parameters;
        }
    }
}
