using System;
using Blog.Application.DTO;
using MediatR;

namespace Blog.Application.Commands.GetPostById
{
    public class GetPostByIdCommand : IRequest<PostDTO>
    {
        public Guid Id { get; set; }
    }
}
