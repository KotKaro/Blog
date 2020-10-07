using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Application.DTO;
using Blog.Domain.Repositories;
using MediatR;

namespace Blog.Application.Queries.GetPosts
{
    public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, PostDTO[]>
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public GetPostsQueryHandler(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<PostDTO[]> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            return (await _postRepository.GetAllAsync(request.PageNumber, request.PageSize))
                .Select(p => _mapper.Map<PostDTO>(p))
                .ToArray();
        }
    }
}
