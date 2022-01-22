using System.Threading.Tasks;
using Blog.Application.Queries.Login;
using Blog.Application.Queries.RefreshToken;
using Blog.Auth.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Blog.API.Controllers
{
    /// <summary>
    /// Controller for login and refreshing token.
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Constructor with mediator which delegates all work to specific handlers.
        /// </summary>
        /// <param name="mediator"></param>
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Login in user with provided credentials.
        /// </summary>
        /// <param name="query">Query with user credentials</param>
        /// <returns>New token</returns>
        [HttpPost]
        [Route("/auth/login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status200OK, "Logged in")]
        public async Task<JwtToken> Login(LoginQuery query)
        {
            return await _mediator
                .Send(query);
        }

        /// <summary>
        /// Refreshes user token.
        /// </summary>
        /// <param name="query">Object with existing token</param>
        /// <returns>New token</returns>
        [HttpPost]
        [Route("/auth/refreshtoken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status200OK, "Token refreshed")]
        public async Task<JwtToken> RefreshToken(RefreshTokenQuery query)
        {
            return await _mediator
                .Send(query);
        }
    }
}
