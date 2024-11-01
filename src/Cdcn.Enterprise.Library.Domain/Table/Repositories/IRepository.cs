using Cdcn.Enterprise.Library.Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Domain.Table.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : Entity
    {
        Task<TEntity> GetByIdAsync(Guid id);
        Task Insert(TEntity entity);
        Task Update(TEntity entity);
        Task Remove(TEntity entity);
        Task Delete(Guid id);
        Task<IEnumerable<TEntity>> QueryAsync(QueryFilter<TEntity> filter);

    }
}
