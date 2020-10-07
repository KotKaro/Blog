using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Application.DTO;
using Blog.Domain.Exceptions;
using Blog.Domain.Models.Aggregates.Post;
using Blog.Domain.Repositories;
using MediatR;

namespace Blog.Application.Commands.GetPostById
{
    public class GetPostByIdCommandHandler : IRequestHandler<GetPostByIdCommand, PostDTO>
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public GetPostByIdCommandHandler(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<PostDTO> Handle(GetPostByIdCommand request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetByIdAsync(request.Id);

            if (post is null)
            {
                throw new RecordNotFoundException(typeof(Post), request.Id);
            }

            return _mapper.Map<PostDTO>(post);
        }
    }
}
