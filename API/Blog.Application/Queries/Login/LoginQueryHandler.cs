using System;
using System.Threading;
using System.Threading.Tasks;
using Blog.Application.Mappers.Exceptions;
using Blog.Auth.Abstractions;
using Blog.Domain.Repositories;
using MediatR;

namespace Blog.Application.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, JwtToken>
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly IUserRepository _userRepository;
        private readonly IClaimFactory _claimFactory;

        public LoginQueryHandler(
            IPasswordHasher passwordHasher,
            IJwtService jwtService,
            IUserRepository userRepository,
            IClaimFactory claimFactory
        )
        {
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _claimFactory = claimFactory ?? throw new ArgumentNullException(nameof(claimFactory));
        }

        public async Task<JwtToken> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username);

            if (user == null)
            {
                throw new LoginException();
            }

            var checkResult = _passwordHasher.Check(user.UserDetails.Password, request.Password);

            if (!checkResult.Verified)
            {
                throw new LoginException();
            }

            var userNameClaim = _claimFactory.CreateUserClaim(user.UserDetails.Username);
            return _jwtService.GenerateToken(userNameClaim);
        }
    }
}
