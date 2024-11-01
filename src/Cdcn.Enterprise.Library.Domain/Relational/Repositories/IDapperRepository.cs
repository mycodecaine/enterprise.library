using Cdcn.Enterprise.Library.Domain.Data;
using Cdcn.Enterprise.Library.Domain.Primitives.Maybe;
using System.Data;

namespace Cdcn.Enterprise.Library.Domain.Relational.Repositories
{
    public interface IDapperRepository<TEntity> where TEntity : Entity
    {
        Task<Maybe<TEntity>> GetByIdAsync(Guid id);
        Task Insert(TEntity entity);
        Task Delete(Guid id);
        Task Remove(TEntity entity);
        Task<IQueryable<TEntity>> GetQueryable(SqlQueryFilter<TEntity> filter);
        Task<PagedResult<TEntity>> Pagination(int pageNumber, int itemsPerPage, string orderBy, bool isDecending, SqlQueryFilter<TEntity> filter);
        Task ExecuteInTransactionAsync(Func<IDbTransaction, Task> action);

    }
}
