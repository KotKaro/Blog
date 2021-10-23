using MediatR;

namespace Blog.Application.Commands.CheckToken
{
    public class CheckToken : IRequest
    {
        public string Token { get; set; }
    }
}
