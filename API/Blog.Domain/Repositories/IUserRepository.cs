using System.Threading.Tasks;
using Blog.Domain.DataAccess;
using Blog.Domain.Models.Aggregates.User;

namespace Blog.Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
    }
}
