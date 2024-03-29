﻿using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Domain.Exceptions;
using Blog.Domain.Models.Aggregates.Post;
using Blog.Domain.Repositories;
using Blog.Domain.Repositories.PostReadRepository;
using MediatR;

namespace Blog.Application.Queries.GetPostById
{
    public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, PostDTO>
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public GetPostByIdQueryHandler(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<PostDTO> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
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
