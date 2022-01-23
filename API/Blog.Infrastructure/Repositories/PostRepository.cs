using System.Linq;
using Blog.DataAccess;
using Blog.Domain.DataAccess;
using Blog.Domain.Models.Aggregates.Post;
using Blog.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(BlogDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        protected override IQueryable<Post> GetQueryWithIncludes()
        {
            return DbContext.Set<Post>()
                .Include(x => x.Comments);
        }
    }
}
