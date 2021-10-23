using System.Threading;
using System.Threading.Tasks;
using Blog.Application.Mappers.Exceptions;
using Blog.Auth.Abstractions;
using MediatR;

namespace Blog.Application.Commands.CheckToken
{
    public class CheckTokenHandler : IRequestHandler<CheckToken, Unit>
    {
        private readonly IJwtService _jwtService;

        public CheckTokenHandler(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        public Task<Unit> Handle(CheckToken request, CancellationToken cancellationToken)
        {
            if (!_jwtService.IsTokenValid(new JwtToken(request?.Token)))
            {
                throw new TokenInvalidException();
            }

            return Unit.Task;
        }
    }
}
