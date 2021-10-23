using Blog.Auth.Abstractions;
using MediatR;

namespace Blog.Application.Queries.Login
{
    public class LoginQuery : IRequest<JwtToken>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
