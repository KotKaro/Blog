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
    public class CommentsController : BlogControllerBase
    {
        public CommentsController(IMediator mediator) : base(mediator)
        {
        }

        public async Task CreateAsync(CreateCommentCommand command)
        {
            command.Id = Guid.NewGuid();
            await mediator.Send(command);
        }

        public async Task DeleteAsync(Guid id)
        {
            await CheckTokenAsync();
            await mediator.Send(new DeleteCommentCommand
            {
                Id = id
            });
        }
    }
}