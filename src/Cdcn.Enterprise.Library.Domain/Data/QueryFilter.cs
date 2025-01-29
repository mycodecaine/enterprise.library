using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Domain.Data
{
    /// <summary>
    /// Represents a collection of filter criteria for querying data of type <typeparamref name="T"/>.
    /// This class allows adding, inserting, removing, replacing, and combining filter expressions.
    /// </summary>
    /// <typeparam name="T">The type of data to filter.</typeparam>
    public class QueryFilter<T>
    {
        /// <summary>
        /// Gets the list of filter expressions.
        /// </summary>
        private List<Expression<Func<T, bool>>> Items { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryFilter{T}"/> class with an empty filter collection.
        /// </summary>
        public QueryFilter()
        {
            Items = new List<Expression<Func<T, bool>>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryFilter{T}"/> class with a single filter criteria.
        /// </summary>
        /// <param name="criteria">The initial filter criteria to add.</param>
        public QueryFilter(Expression<Func<T, bool>> criteria)
        {
            Items = new List<Expression<Func<T, bool>>>();
            Add(criteria);
        }

        /// <summary>
        /// Adds a new filter criteria to the collection.
        /// </summary>
        /// <param name="criteria">The filter criteria to add.</param>
        /// <returns>The index of the added criteria in the collection.</returns>
        public int Add(Expression<Func<T, bool>> criteria)
        {
            Items.Add(criteria);
            return Items.Count - 1;
        }

        /// <summary>
        /// Inserts a filter criteria at the specified index in the collection.
        /// </summary>
        /// <param name="index">The index at which to insert the criteria.</param>
        /// <param name="criteria">The filter criteria to insert.</param>
        public void Insert(int index, Expression<Func<T, bool>> criteria)
        {
            Items.Insert(index, criteria);
        }

        /// <summary>
        /// Removes the filter criteria at the specified index from the collection.
        /// </summary>
        /// <param name="index">The index of the criteria to remove.</param>
        public void RemoveAt(int index)
        {
            Items.RemoveAt(index);
        }

        /// <summary>
        /// Replaces the filter criteria at the specified index with a new criteria.
        /// </summary>
        /// <param name="index">The index of the criteria to replace.</param>
        /// <param name="criteria">The new filter criteria to use.</param>
        public void Replace(int index, Expression<Func<T, bool>> criteria)
        {
            RemoveAt(index);
            Insert(index, criteria);
        }

        /// <summary>
        /// Clears all filter criteria from the collection.
        /// </summary>
        public void Clear()
        {
            Items.Clear();
        }

        /// <summary>
        /// Combines all filter criteria in the collection into a single LINQ expression using the AND operator.
        /// </summary>
        /// <returns>A combined LINQ expression representing all filter criteria.</returns>
        public Expression<Func<T, bool>> Combine()
        {
            return Items.CombineAnd();
        }

        /// <summary>
        /// Combines all filter criteria in the collection into a single LINQ expression using the AND operator.
        /// </summary>
        /// <returns>A combined LINQ expression representing all filter criteria.</returns>
        public Expression<Func<T, bool>> CombineAnd()
        {
            return Items.CombineAnd();
        }

        /// <summary>
        /// Combines all filter criteria in the collection into a single LINQ expression using the OR operator.
        /// </summary>
        /// <returns>A combined LINQ expression representing all filter criteria.</returns>
        public Expression<Func<T, bool>> CombineOr()
        {
            return Items.CombineOr();
        }

        /// <summary>
        /// Returns a string representation of the <see cref="QueryFilter{T}"/> object.
        /// </summary>
        /// <returns>A string in the format "QueryFilter&lt;TypeName&gt;[Count]", where TypeName is the name of the type T and Count is the number of filter criteria.</returns>
        public override string ToString()
        {
            return $"QueryFilter<{typeof(T).Name}>[{Items.Count}]";
        }
    }
}
