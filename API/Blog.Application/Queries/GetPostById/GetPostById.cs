using System;
using Blog.Application.DTO;
using MediatR;

namespace Blog.Application.Queries.GetPostById
{
    public class GetPostByIdQuery : IRequest<PostDTO>
    {
        public Guid Id { get; set; }
    }
}
