using System.Threading.Tasks;

namespace Blog.Domain.Repositories.PostReadRepository;

public interface IPostReadRepository
{
    Task<PostDTO[]> GetAllAsync(int pageNumber = 1, int pageSize = 10);
}