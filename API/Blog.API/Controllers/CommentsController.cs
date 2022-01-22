using System;
using System.Net;
using System.Threading.Tasks;
using Blog.Application.Commands.CreateComment;
using Blog.Application.Commands.DeleteComment;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Blog.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentsController : BlogControllerBase
    {
        public CommentsController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Creates comment for post specified in command
        /// </summary>
        /// <param name="command">Command details about comment to create</param>
        /// <returns>PostDTO objects</returns>
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateAsync(CreateComment command)
        {
            command.Id = Guid.NewGuid();
            await mediator.Send(command);

            return StatusCode(201);
        }

        /// <summary>
        /// Deletes comment with specified id
        /// </summary>
        /// <param name="id">ID of comment to delete</param>
        /// <returns>PostDTO objects</returns>
        [HttpDelete]
        [Route("{id:guid}")]
        [SwaggerResponse((int)HttpStatusCode.Accepted)]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await CheckTokenAsync();
            await mediator.Send(new DeleteCommentCommand
            {
                Id = id
            });

            return StatusCode(202);
        }
    }
}