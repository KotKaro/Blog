using System.Threading.Tasks;

namespace Blog.Domain.DataAccess
{
    public interface IUnitOfWork
    {
        ITransaction CurrentTransaction { get; }

        Task<ITransaction> BeginTransactionAsync();
        Task CommitTransactionAsync(ITransaction transaction);
        bool HasActiveTransaction { get; }
        Task<int> SaveEntitiesAsync();
    }
}
