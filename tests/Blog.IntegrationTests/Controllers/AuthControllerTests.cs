using System.Security.Claims;
using System.Threading.Tasks;
using Autofac;
using Blog.API.Controllers;
using Blog.Application.Mappers.Exceptions;
using Blog.Application.Queries.Login;
using Blog.Application.Queries.RefreshToken;
using Blog.Auth.Abstractions;
using Blog.Domain.Models.Aggregates.User;
using Blog.Domain.Repositories;
using Blog.IntegrationTests.Common;
using Blog.Tests.Common;
using MediatR;
using NUnit.Framework;

namespace Blog.IntegrationTests.Controllers
{
    [TestFixture]
    public class AuthControllerTests : IntegrationTestBase
    {
        private AuthController _authController;
        private IUserRepository _userRepository;
        private IPasswordHasher _passwordHasher;
        private IJwtService _jwtService;


        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _userRepository = Container.Resolve<IUserRepository>();
            _passwordHasher = Container.Resolve<IPasswordHasher>();
            _jwtService = Container.Resolve<IJwtService>();

            _authController = new AuthController(
                Container.Resolve<IMediator>()
            );
        }

        [SetUp]
        public void SetUp()
        {
            BlogContext.Set<User>().RemoveRange(BlogContext.Set<User>());
        }

        [Test]
        public void When_UsernameOfUnexistingUserProvided_Expect_LoginExceptionThrown()
        {
            Assert.ThrowsAsync<LoginException>(async () =>
            {
                await _authController.Login(new LoginQuery
                {
                    Username = "test",
                    Password = _passwordHasher.Hash("test123")
                });
            });
        }

        [Test]
        public async Task When_UsernameExistsButPasswordIsWrong_Expect_LoginExceptionThrown()
        {
            const string password = "test123";
            await BlogContext.Set<User>().AddAsync(MockFactory.CreateUser(new UserDetails("test", _passwordHasher.Hash("test123"))));
            await BlogContext.SaveChangesAsync();

            Assert.ThrowsAsync<LoginException>(async () =>
            {
                await _authController.Login(new LoginQuery
                {
                    Username = "test",
                    Password = _passwordHasher.Hash("xyzTestxy123123")
                });
            });
        }

        [Test]
        public async Task When_UsernameExistsAndPasswordMatch_Expect_TokenReturned()
        {
            const string password = "test123";
            await BlogContext.Set<User>().AddAsync(MockFactory.CreateUser(new UserDetails("test", _passwordHasher.Hash(password))));
            await BlogContext.SaveChangesAsync();

            var token = await _authController.Login(new LoginQuery
            {
                Username = "test",
                Password = password
            });

            Assert.NotNull(token);
        }

        [Test]
        public void When_TokenInvalid_Expect_TokenInvalidExceptionThrown()
        {
            Assert.ThrowsAsync<TokenInvalidException>(async () =>
            {
                await _authController.RefreshToken(new RefreshTokenQuery { Token = "123" });
            });
        }

        [Test]
        public async Task When_TokenValid_Expect_NewTokenReturned()
        {
            var token = _jwtService.GenerateToken(new Claim("test", "test"));

            var newToken = await _authController.RefreshToken(new RefreshTokenQuery { Token = token.Value });

            Assert.NotNull(newToken);
        }

        [Test]
        public void ToBeDeleted()
        {
            var hash = _passwordHasher.Hash("test");


        }
    }
}
