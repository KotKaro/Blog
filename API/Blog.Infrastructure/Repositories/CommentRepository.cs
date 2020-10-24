using System.Linq;
using Blog.DataAccess;
using Blog.Domain.DataAccess;
using Blog.Domain.Models.Aggregates.Post;
using Blog.Domain.Repositories;

namespace Blog.Infrastructure.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(BlogDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        protected override IQueryable<Comment> GetQueryWithIncludes()
        {
            return DbContext.Set<Comment>();
        }
    }
}
