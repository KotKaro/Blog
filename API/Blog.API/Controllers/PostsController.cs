using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Blog.Application.Commands.CheckToken;
using Blog.Application.Commands.CreatePost;
using Blog.Application.Commands.DeletePost;
using Blog.Application.Commands.UpdatePost;
using Blog.Application.DTO;
using Blog.Application.Queries.GetPostById;
using Blog.Application.Queries.GetPosts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;

namespace Blog.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Returns specific amount of posts described in query.
        /// </summary>
        /// <param name="query">Query details, about page number and size</param>
        /// <returns>PostDTO objects</returns>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK)]
        public async Task<PostDTO[]> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
            => await _mediator.Send(new GetPostsQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            });

        /// <summary>
        /// Creates post with specific details.
        /// </summary>
        /// <param name="createCreatePostCommand">Command with all required data</param>
        /// <returns>Created object</returns>
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status201Created, "Created post")]
        public async Task<PostDTO> Create(CreatePostCommand createCreatePostCommand)
        {
            await _mediator.Send(new CheckTokenCommand { Token = GetTokenHeaderValue() });

            var id = Guid.NewGuid();
            createCreatePostCommand.Id = id;
            await _mediator.Send(createCreatePostCommand);

            return await GetById(id);
        }

        /// <summary>
        /// Retrieve post with specific ID
        /// </summary>
        /// <param name="id">ID of post to retrieve</param>
        /// <returns>PostDTO object</returns>
        [HttpGet]
        [Route("{id:guid}")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        public async Task<PostDTO> GetById([FromRoute] Guid id)
        {
            return await _mediator.Send(new GetPostByIdQuery
            {
                Id = id
            });
        }

        /// <summary>
        /// Updates post specified by command ID
        /// </summary>
        /// <param name="request">Data of post to update</param>
        /// <returns>Information about completed action</returns>
        [HttpPatch]
        [SwaggerResponse(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(UpdatePostCommand request)
        {
            await _mediator.Send(new CheckTokenCommand { Token = GetTokenHeaderValue() });
            await _mediator.Send(request);

            return Ok();
        }

        /// <summary>
        /// Deletes post with specified ID
        /// </summary>
        /// <param name="id">ID of post to remove</param>
        /// <returns>Information about completed action</returns>
        [HttpDelete]
        [Route("{id:guid}")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await _mediator.Send(new CheckTokenCommand {Token = GetTokenHeaderValue()});
            await _mediator.Send(new DeleteCommentCommand
            {
                Id = id
            });

            return NoContent();
        }

        private string GetTokenHeaderValue()
        {
            return HttpContext.Request.Headers["jwt-token"].FirstOrDefault() ?? "no-token";
        }
    }
}