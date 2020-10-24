using System;
using MediatR;

namespace Blog.Application.Commands.DeleteComment
{
    public class DeleteCommentCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
