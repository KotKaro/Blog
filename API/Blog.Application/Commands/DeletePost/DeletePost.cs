using System;
using MediatR;

namespace Blog.Application.Commands.DeletePost
{
    public class DeleteCommentCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
