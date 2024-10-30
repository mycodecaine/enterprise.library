using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Domain.Data
{
    public class QueryFilter<T>
    {
        private List<Expression<Func<T, bool>>> Items { get; }

        public QueryFilter()
        {
            Items = new List<Expression<Func<T, bool>>>();
        }

        public QueryFilter(Expression<Func<T, bool>> criteria)
        {
            Items = new List<Expression<Func<T, bool>>>();

            Add(criteria);
        }

        /// <summary>
        /// Adds new criteria to the filter collection. 
        /// </summary>
        /// <returns>Returns index of added criteria.</returns>
        /// <param name="criteria">Criteria.</param>
        public int Add(Expression<Func<T, bool>> criteria)
        {
            Items.Add(criteria);
            return Items.Count - 1;
        }

        public void Insert(int index, Expression<Func<T, bool>> criteria)
        {
            Items.Insert(index, criteria);
        }

        public void RemoveAt(int index)
        {
            Items.RemoveAt(index);
        }

        public void Replace(int index, Expression<Func<T, bool>> criteria)
        {
            RemoveAt(index);
            Insert(index, criteria);
        }

        public void Clear()
        {
            Items.Clear();
        }

        /// <summary>
        /// Combines criteria list collection into a single Linq expression
        /// </summary>
        /// <returns>The combine.</returns>
        public Expression<Func<T, bool>> Combine()
        {
            return Items.CombineAnd();
        }

        /// <summary>
        /// Combines criteria list collection into a single Linq expression
        /// </summary>
        /// <returns>The combine.</returns>
        public Expression<Func<T, bool>> CombineAnd()
        {
            return Items.CombineAnd();
        }

        /// <summary>
        /// Combines criteria list collection into a single Linq expression
        /// </summary>
        /// <returns>The combine.</returns>
        public Expression<Func<T, bool>> CombineOr()
        {
            return Items.CombineOr();
        }
        public override string ToString()
        {
            return $"QueryFilter<{typeof(T).Name}>[{Items.Count}]";
        }
    }
}
