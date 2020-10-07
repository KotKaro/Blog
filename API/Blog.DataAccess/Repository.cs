using System;
using System.Linq;
using System.Threading.Tasks;
using Blog.Domain.DataAccess;
using Blog.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.DataAccess
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, IAggregateRoot
    {
        protected readonly DbContext DbContext;

        public Repository(DbContext dbContext, IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public IUnitOfWork UnitOfWork { get; }

        public Task<TEntity[]> GetAllAsync()
        {
            return GetQueryWithIncludes()
                .ToArrayAsync();
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await GetQueryWithIncludes()
                .Where(entity => entity.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await DbContext
                .Set<TEntity>()
                .AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            DbContext
                .Set<TEntity>()
                .Remove(entity);
        }

        protected abstract IQueryable<TEntity> GetQueryWithIncludes();
    }
}
