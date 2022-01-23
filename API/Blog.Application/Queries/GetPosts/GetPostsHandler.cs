using System;
using System.Threading;
using System.Threading.Tasks;
using Blog.Domain.Repositories.PostReadRepository;
using MediatR;

namespace Blog.Application.Queries.GetPosts
{
    public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, PostDTO[]>
    {
        private readonly IPostReadRepository _postRepository;

        public GetPostsQueryHandler(IPostReadRepository postRepository)
        {
            _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        }

        public async Task<PostDTO[]> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            return await _postRepository.GetAllAsync(request.PageNumber, request.PageSize);
        }
    }
}
