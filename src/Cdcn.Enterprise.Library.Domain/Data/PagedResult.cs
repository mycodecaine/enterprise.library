using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Domain.Data
{
    [ExcludeFromCodeCoverage]
    public abstract class PagedResultBase
    {
        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets the total number of pages.
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// Gets or sets the number of items per page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the total number of rows/items in the dataset.
        /// </summary>
        public int RowCount { get; set; }

        /// <summary>
        /// Gets the index of the first row on the current page.
        /// </summary>
        public int FirstRowOnPage
        {
            get { return (CurrentPage - 1) * PageSize + 1; }
        }

        /// <summary>
        /// Gets the index of the last row on the current page.
        /// </summary>
        public int LastRowOnPage
        {
            get { return Math.Min(CurrentPage * PageSize, RowCount); }
        }
    }

    /// <summary>
    /// Represents a paginated result for a generic type T.
    /// </summary>
    /// <typeparam name="T">The type of the results in the list.</typeparam>
    public class PagedResult<T> : PagedResultBase where T : class
    {
        /// <summary>
        /// Gets or sets the list of results for the current page.
        /// </summary>
        public IList<T> Results { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedResult{T}"/> class.
        /// </summary>
        public PagedResult()
        {
            Results = new List<T>();
        }
    }

    [ExcludeFromCodeCoverage]
    public static class PagedResultExtension
    {
        /// <summary>
        /// Paginates an IQueryable collection based on the specified page and page size.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the query.</typeparam>
        /// <param name="query">The IQueryable collection to paginate.</param>
        /// <param name="page">The page number to retrieve.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A <see cref="PagedResult{T}"/> object containing the paginated results and pagination details.</returns>
        public static PagedResult<T> GetPaged<T>(this IQueryable<T> query, int page, int pageSize) where T : class
        {
            var result = new PagedResult<T>();
            result.CurrentPage = page;
            result.PageSize = pageSize;
            result.RowCount = query.Count();

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results = query.Skip(skip).Take(pageSize).ToList();

            return result;
        }

        /// <summary>
        /// Paginates an IList collection based on the specified page and page size.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the list.</typeparam>
        /// <param name="query">The IList collection to paginate.</param>
        /// <param name="page">The page number to retrieve.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A <see cref="PagedResult{T}"/> object containing the paginated results and pagination details.</returns>
        public static PagedResult<T> GetPaged<T>(this IList<T> query, int page, int pageSize) where T : class
        {
            var result = new PagedResult<T>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = query.Count()
            };

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results = query.Skip(skip).Take(pageSize).ToList();

            return result;
        }

        /// <summary>
        /// Orders an IQueryable collection by a specified member path (property or field).
        /// </summary>
        /// <typeparam name="T">The type of the elements in the query.</typeparam>
        /// <param name="source">The IQueryable collection to order.</param>
        /// <param name="memberPath">The path to the member (property or field) to order by.</param>
        /// <param name="descending">Whether to sort in descending order (true) or ascending order (false).</param>
        /// <returns>An <see cref="IOrderedQueryable{T}"/> for further chaining or execution.</returns>
        public static IOrderedQueryable<T> OrderByMember<T>(this IQueryable<T> source, string memberPath, bool descending)
        {
            var parameter = Expression.Parameter(typeof(T), "item");
            var member = memberPath.Split('.').Aggregate((Expression)parameter, Expression.PropertyOrField);
            var keySelector = Expression.Lambda(member, parameter);

            var methodCall = Expression.Call(
                typeof(Queryable),
                descending ? "OrderByDescending" : "OrderBy",
                new[] { parameter.Type, member.Type },
                source.Expression,
                Expression.Quote(keySelector));

            return (IOrderedQueryable<T>)source.Provider.CreateQuery(methodCall);
        }
    }
}
