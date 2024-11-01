using Cdcn.Enterprise.Library.Domain.Data;
using Cdcn.Enterprise.Library.Domain.Primitives.Maybe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Domain.Relational.Repositories
{
    public interface IRepository<TEntity>
       where TEntity : Entity
    {
        Task<Maybe<TEntity>> GetByIdAsync(Guid id);
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(Guid id);
        void Remove(TEntity entity);
        Task<IQueryable<TEntity>> GetQueryable(QueryFilter<TEntity> filter);
        Task<PagedResult<TEntity>> Pagination(int pageNumber, int itemsPerPage, string orderBy, bool isDecending, QueryFilter<TEntity> filter);

    }
}
