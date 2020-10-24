using System;
using System.Threading.Tasks;
using Blog.Domain.Models;

namespace Blog.Domain.DataAccess
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        IUnitOfWork UnitOfWork { get; }
        Task<TEntity[]> GetAllAsync();
        Task<TEntity> GetByIdAsync(Guid id);
        Task AddAsync(TEntity entity);
        void Delete(TEntity entity);
    }
}
