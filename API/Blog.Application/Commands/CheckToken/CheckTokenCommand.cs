using MediatR;

namespace Blog.Application.Commands.CheckToken
{
    public class CheckTokenCommand : IRequest
    {
        public string Token { get; set; }
    }
}
