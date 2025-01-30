using Cdcn.Enterprise.Library.Domain.Data;
using Cdcn.Enterprise.Library.Domain.Primitives;
using Cdcn.Enterprise.Library.Domain.Primitives.Maybe;
using System.Data;

namespace Cdcn.Enterprise.Library.Domain.Relational.Repositories
{
    /// <summary>
    /// Interface for a Dapper repository that provides basic CRUD operations and query capabilities for entities.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IDapperRepository<TEntity> where TEntity : Entity
    {
        /// <summary>
        /// Retrieves an entity by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a Maybe instance with the entity if found, otherwise None.</returns>
        Task<Maybe<TEntity>> GetByIdAsync(Guid id);

        /// <summary>
        /// Inserts a new entity into the repository.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task Insert(TEntity entity);

        /// <summary>
        /// Deletes an entity by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task Delete(Guid id);

        /// <summary>
        /// Removes an entity from the repository.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task Remove(TEntity entity);

        /// <summary>
        /// Retrieves a queryable collection of entities based on the specified filter.
        /// </summary>
        /// <param name="filter">The filter to apply to the query.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IQueryable of entities.</returns>
        Task<IQueryable<TEntity>> GetQueryable(SqlQueryFilter<TEntity> filter);

        /// <summary>
        /// Retrieves a paginated result of entities based on the specified parameters.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="itemsPerPage">The number of items per page.</param>
        /// <param name="orderBy">The column to order by.</param>
        /// <param name="isDecending">Indicates whether the order should be descending.</param>
        /// <param name="filter">The filter to apply to the query.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a PagedResult of entities.</returns>
        Task<PagedResult<TEntity>> Pagination(int pageNumber, int itemsPerPage, string orderBy, bool isDecending, SqlQueryFilter<TEntity> filter);

        /// <summary>
        /// Executes a specified action within a database transaction.
        /// </summary>
        /// <param name="action">The action to execute within the transaction.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ExecuteInTransactionAsync(Func<IDbTransaction, Task> action);
    }
}
