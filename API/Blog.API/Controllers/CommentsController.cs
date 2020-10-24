using System;
using System.Threading.Tasks;
using Blog.Application.Commands.CreateComment;
using Blog.Application.Commands.DeleteComment;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CommentsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task CreateAsync(CreateCommentCommand command)
        {
            command.Id = Guid.NewGuid();
            await _mediator.Send(command);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _mediator.Send(new DeleteCommentCommand
            {
                Id = id
            });
        }
    }
}