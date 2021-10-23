using Blog.Application.DTO;
using MediatR;

namespace Blog.Application.Queries.GetPosts
{
    public class GetPostsQuery : IRequest<PostDTO[]>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
