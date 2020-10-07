using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.DataAccess;
using Blog.Domain.DataAccess;
using Blog.Domain.Models.Aggregates.Post;
using Blog.Domain.Repositories;

namespace Blog.Infrastructure.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(BlogDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        protected override IQueryable<Post> GetQueryWithIncludes()
        {
            return DbContext.Set<Post>();
        }

        public async Task<IEnumerable<Post>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        {
            return GetQueryWithIncludes().Skip((pageNumber * pageSize) - pageSize)
                .Take(pageSize);
        }
    }
}
