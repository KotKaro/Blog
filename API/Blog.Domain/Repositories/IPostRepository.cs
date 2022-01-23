using Blog.Domain.DataAccess;
using Blog.Domain.Models.Aggregates.Post;

namespace Blog.Domain.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
    }
}
