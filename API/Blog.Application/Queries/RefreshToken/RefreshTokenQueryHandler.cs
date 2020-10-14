using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blog.Application.Exceptions;
using Blog.Auth.Abstractions;
using MediatR;

namespace Blog.Application.Queries.RefreshToken
{
    public class RefreshTokenQueryHandler : IRequestHandler<RefreshTokenQuery, JwtToken>
    {
        private readonly IJwtService _jwtService;

        public RefreshTokenQueryHandler(IJwtService jwtService)
        {
            _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
        }

        public async Task<JwtToken> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
        {
            var token = new JwtToken(request.Token);
            if (!_jwtService.IsTokenValid(token))
            {
                throw new TokenInvalidException();
            }

            var tokenClaims = _jwtService
                .GetTokenClaims(token)
                .ToArray();

            return _jwtService.GenerateToken(tokenClaims);
        }
    }
}
