using Blog.Auth.Abstractions;
using MediatR;

namespace Blog.Application.Queries.RefreshToken
{
    public class RefreshTokenQuery : IRequest<JwtToken>
    {
        public string Token { get; set; }
    }
}
