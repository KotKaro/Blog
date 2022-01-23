using System;
using Blog.Domain.Repositories.PostReadRepository;
using MediatR;

namespace Blog.Application.Queries.GetPostById
{
    public class GetPostByIdQuery : IRequest<PostDTO>
    {
        public Guid Id { get; set; }
    }
}
