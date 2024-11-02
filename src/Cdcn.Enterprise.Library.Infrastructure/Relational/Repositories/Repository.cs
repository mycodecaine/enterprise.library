using Cdcn.Enterprise.Library.Domain.Data;
using Cdcn.Enterprise.Library.Domain.Primitives.Maybe;
using Cdcn.Enterprise.Library.Domain.Relational;
using Cdcn.Enterprise.Library.Domain.Relational.Repositories;
using Cdcn.Enterprise.Library.Infrastructure.Relational.Abstractions;
using Microsoft.EntityFrameworkCore;
using Swq.Infrastructure.DataAccess.Specifications;

namespace Cdcn.Enterprise.Library.Infrastructure.Relational
{
    public  class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected Repository(IDbContext dbContext) => DbContext = dbContext;

        protected IDbContext DbContext { get; }
        public async void Delete(Guid id)
        {
            var entity = await DbContext.GetBydIdAsync<TEntity>(id);
            var data = entity.Value;
            Remove(data);

        }

        public virtual async Task<Maybe<TEntity>> GetByIdAsync(Guid id) => await DbContext.GetBydIdAsync<TEntity>(id);

        public async  Task<IQueryable<TEntity>> GetQueryable(QueryFilter<TEntity> filter)
        {
            var dataQ = DbContext.Set<TEntity>().AsQueryable();
            var data = await Task.Run(() => dataQ.Where(filter.Combine()));
            return data;
        }

        public void Insert(TEntity entity) => DbContext.Insert(entity);

        public async Task<PagedResult<TEntity>> Pagination(int pageNumber, int itemsPerPage, string orderBy, bool isDecending, QueryFilter<TEntity> filter)
        {
            var data = DbContext.Set<TEntity>().AsQueryable();
            var query = await Task.Run(() => data.Where(filter.Combine()).OrderByMember(orderBy, isDecending).GetPaged(pageNumber, itemsPerPage));

            return query;
        }

        public void Update(TEntity entity) => DbContext.Set<TEntity>().Update(entity);

        public void Remove(TEntity entity) => DbContext.Remove(entity);

        /// <summary>
        /// Checks if any entity meets the specified specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>True if any entity meets the specified specification, otherwise false.</returns>
        protected async Task<bool> AnyAsync(Specification<TEntity> specification) =>
            await DbContext.Set<TEntity>().AnyAsync(specification);

        /// <summary>
        /// Gets the first entity that meets the specified specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>The maybe instance that may contain the first entity that meets the specified specification.</returns>
        protected async Task<Maybe<TEntity?>> FirstOrDefaultAsync(Specification<TEntity> specification)
        {
            return await DbContext.Set<TEntity>().FirstOrDefaultAsync(specification);
        }
    }
}
