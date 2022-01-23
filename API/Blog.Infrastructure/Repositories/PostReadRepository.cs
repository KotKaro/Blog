using System.Linq;
using System.Threading.Tasks;
using Blog.Domain.Models.Aggregates.Post;
using Blog.Domain.Repositories.PostReadRepository;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories;

public class PostReadRepository : IPostReadRepository
{
    private readonly BlogDbContext _dbContext;

    public PostReadRepository(BlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }
        
    public async Task<PostDTO[]> GetAllAsync(int pageNumber = 1, int pageSize = 10)
    {
        return await _dbContext.Set<Post>()
            .Include(x => x.Comments)
            .OrderByDescending(x => x.CreationDate.Value)
            .Skip(pageNumber * pageSize - pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .Select(x => new PostDTO
            {
                Id = x.Id,
                Content = x.Content.Value,
                Title = x.Title.Value,
                CreationDate = x.CreationDate.Value,
                Comments = x.Comments.Select(comment => new CommentDTO
                {
                    Id = comment.Id,
                    Content = comment.Content.Value,
                    Creator = comment.Creator.Value
                }).ToArray()
            })
            .ToArrayAsync();
    }
}