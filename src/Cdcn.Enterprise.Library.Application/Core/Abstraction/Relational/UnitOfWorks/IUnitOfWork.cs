namespace Cdcn.Enterprise.Library.Application.Core.Abstraction.Relational.UnitOfWorks
{
    /// <summary>
    /// Represents a unit of work that encapsulates a set of operations to be committed as a single transaction.
    /// Notes : <a href="https://chatgpt.com/share/679a3e65-eb44-800b-8867-62e00917d4fb" />
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Asynchronously saves all changes made in the unit of work.
        /// </summary>
        /// <param name="saveBy">The identifier of the user or process that is saving the changes.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries written to the database.</returns>
        Task<int> SaveChangesAsync(Guid saveBy, CancellationToken cancellationToken = default);
    }
}
