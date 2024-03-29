﻿using System;
using System.Net;
using System.Threading.Tasks;
using Blog.Application.Commands.CreatePost;
using Blog.Application.Commands.DeletePost;
using Blog.Application.Commands.UpdatePost;
using Blog.Application.Queries.GetPostById;
using Blog.Application.Queries.GetPosts;
using Blog.Domain.Repositories.PostReadRepository;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Blog.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : BlogControllerBase
    {
        public PostsController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Returns specific amount of posts described in query.
        /// </summary>
        /// <param name="query">Query details, about page number and size</param>
        /// <returns>PostDTO objects</returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        public async Task<PostDTO[]> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
            => await mediator.Send(new GetPostsQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            });

        /// <summary>
        /// Creates post with specific details.
        /// </summary>
        /// <param name="createCreatePost">Command with all required data</param>
        /// <returns>Created object</returns>
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status201Created, "Created post")]
        public async Task<PostDTO> Create(CreatePost createCreatePost)
        {
            await CheckTokenAsync();

            var id = Guid.NewGuid();
            createCreatePost.Id = id;
            await mediator.Send(createCreatePost);

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
            return await mediator.Send(new GetPostByIdQuery
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
            await CheckTokenAsync();
            await mediator.Send(request);

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
            await CheckTokenAsync();
            await mediator.Send(new DeleteCommentCommand
            {
                Id = id
            });

            return NoContent();
        }
    }
}