using System.Threading;
using System.Threading.Tasks;
using Blog.Application.Mappers.Exceptions;
using Blog.Auth.Abstractions;
using MediatR;

namespace Blog.Application.Commands.CheckToken
{
    public class CheckTokenCommandHandler : IRequestHandler<CheckTokenCommand, Unit>
    {
        private readonly IJwtService _jwtService;

        public CheckTokenCommandHandler(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        public Task<Unit> Handle(CheckTokenCommand request, CancellationToken cancellationToken)
        {
            if (!_jwtService.IsTokenValid(new JwtToken(request?.Token)))
            {
                throw new TokenInvalidException();
            }

            return Unit.Task;
        }
    }
}
