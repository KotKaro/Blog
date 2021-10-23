using System.Security.Claims;
using System.Threading.Tasks;
using Blog.API.Controllers;
using Blog.Application.Mappers.Exceptions;
using Blog.Application.Queries.Login;
using Blog.Application.Queries.RefreshToken;
using Blog.Auth.Abstractions;
using Blog.Domain.Models.Aggregates.User;
using Blog.Infrastructure;
using Blog.IntegrationTests.Common;
using Blog.Tests.Common;
using MediatR;
using Xunit;

namespace Blog.IntegrationTests.Controllers
{
    [Collection(nameof(BlogTestCollection))]
    public class AuthControllerTests
    {
        private readonly AuthController _authController;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly BlogDbContext _blogDbContext;

        public AuthControllerTests(BlogApplicationFactory factory)
        {
            _passwordHasher = factory.Services.GetService(typeof(IPasswordHasher)) as IPasswordHasher;
            _jwtService = factory.Services.GetService(typeof(IJwtService)) as IJwtService;
            _blogDbContext = factory.Services.GetService(typeof(BlogDbContext)) as BlogDbContext;

            _authController = new AuthController(
                factory.Services.GetService(typeof(IMediator)) as IMediator
            );
            
            _blogDbContext!.Set<User>().RemoveRange(_blogDbContext.Set<User>());
        }

        [Fact]
        public async Task When_UsernameOfNotExistingUserProvided_Expect_LoginExceptionThrown()
        {
            await Assert.ThrowsAsync<LoginException>(async () =>
            {
                await _authController.Login(new LoginQuery
                {
                    Username = "test",
                    Password = _passwordHasher.Hash("test123")
                });
            });
        }

        [Fact]
        public async Task When_UsernameExistsButPasswordIsWrong_Expect_LoginExceptionThrown()
        {
            await _blogDbContext.Set<User>().AddAsync(MockFactory.CreateUser(new UserDetails("test", _passwordHasher.Hash("test123"))));
            await _blogDbContext.SaveChangesAsync();

            await Assert.ThrowsAsync<LoginException>(async () =>
            {
                await _authController.Login(new LoginQuery
                {
                    Username = "test",
                    Password = _passwordHasher.Hash("xyzTest123123")
                });
            });
        }

        [Fact]
        public async Task When_UsernameExistsAndPasswordMatch_Expect_TokenReturned()
        {
            const string password = "test123";
            await _blogDbContext.Set<User>().AddAsync(MockFactory.CreateUser(new UserDetails("test", _passwordHasher.Hash(password))));
            await _blogDbContext.SaveChangesAsync();

            var token = await _authController.Login(new LoginQuery
            {
                Username = "test",
                Password = password
            });

            Assert.NotNull(token);
        }

        [Fact]
        public async Task When_TokenInvalid_Expect_TokenInvalidExceptionThrown()
        {
            await Assert.ThrowsAsync<TokenInvalidException>(async () =>
            {
                await _authController.RefreshToken(new RefreshTokenQuery { Token = "123" });
            });
        }

        [Fact]
        public async Task When_TokenValid_Expect_NewTokenReturned()
        {
            var token = _jwtService.GenerateToken(new Claim("test", "test"));

            var newToken = await _authController.RefreshToken(new RefreshTokenQuery { Token = token.Value });

            Assert.NotNull(newToken);
        }
    }
}
