using Cdcn.Enterprise.Library.Domain.Relational.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Domain.Relational.UnitOfWorks
{
    public interface IDapperUnitOfWork
    {
        IDapperRepository<TEntity> Repository<TEntity>() where TEntity : Entity;
        Task CommitAsync();
        void Rollback();
        void BeginTransaction();
    }
}
