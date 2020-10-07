using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Domain.DataAccess;
using Blog.Domain.Models.Aggregates.Post;

namespace Blog.Domain.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<IEnumerable<Post>> GetAllAsync(int pageNumber = 1, int pageSize = 10);
    }
}
