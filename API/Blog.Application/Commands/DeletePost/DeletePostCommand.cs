using System;
using MediatR;

namespace Blog.Application.Commands.DeletePost
{
    public class DeletePostCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
