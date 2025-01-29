using Cdcn.Enterprise.Library.Domain.Data;
using Cdcn.Enterprise.Library.Domain.Primitives.Maybe;

namespace Cdcn.Enterprise.Library.Domain.Relational.Repositories
{
    /// <summary>
    /// Defines a repository interface for managing entities of type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IRepository<TEntity>
       where TEntity : Entity
    {
        /// <summary>
        /// Retrieves an entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Maybe{TEntity}"/>.</returns>
        Task<Maybe<TEntity>> GetByIdAsync(Guid id);

        /// <summary>
        /// Inserts a new entity into the repository.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        void Insert(TEntity entity);

        /// <summary>
        /// Updates an existing entity in the repository.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        void Update(TEntity entity);

        /// <summary>
        /// Deletes an entity from the repository by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to delete.</param>
        void Delete(Guid id);

        /// <summary>
        /// Removes an entity from the repository.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        void Remove(TEntity entity);

        /// <summary>
        /// Retrieves a queryable collection of entities that match the specified filter.
        /// </summary>
        /// <param name="filter">The filter criteria for querying entities.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IQueryable{TEntity}"/>.</returns>
        Task<IQueryable<TEntity>> GetQueryable(QueryFilter<TEntity> filter);

        /// <summary>
        /// Retrieves a paginated result of entities that match the specified filter and sorting criteria.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="itemsPerPage">The number of items per page.</param>
        /// <param name="orderBy">The property to order by.</param>
        /// <param name="isDecending">Indicates whether the sorting should be in descending order.</param>
        /// <param name="filter">The filter criteria for querying entities.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="PagedResult{TEntity}"/>.</returns>
        Task<PagedResult<TEntity>> Pagination(int pageNumber, int itemsPerPage, string orderBy, bool isDecending, QueryFilter<TEntity> filter);
    }
}
