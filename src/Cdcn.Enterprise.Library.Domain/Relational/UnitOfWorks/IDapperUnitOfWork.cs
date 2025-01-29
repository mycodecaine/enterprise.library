using Cdcn.Enterprise.Library.Domain.Relational.Repositories;

namespace Cdcn.Enterprise.Library.Domain.Relational.UnitOfWorks
{
    /// <summary>
    /// Interface for a unit of work that manages Dapper repositories and transactions.
    /// </summary>
    public interface IDapperUnitOfWork
    {
        /// <summary>
        /// Gets the repository for the specified entity type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>The repository for the specified entity type.</returns>
        IDapperRepository<TEntity> Repository<TEntity>() where TEntity : Entity;

        /// <summary>
        /// Commits the current transaction asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task CommitAsync();

        /// <summary>
        /// Rolls back the current transaction.
        /// </summary>
        void Rollback();

        /// <summary>
        /// Begins a new transaction.
        /// </summary>
        void BeginTransaction();
    }
}
