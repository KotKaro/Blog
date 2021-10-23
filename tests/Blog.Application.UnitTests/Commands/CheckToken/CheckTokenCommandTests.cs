using System.Security.Claims;
using System.Threading;
using Blog.Application.Commands.CheckToken;
using Blog.Application.Mappers.Exceptions;
using Blog.Auth;
using Blog.Auth.Models;
using NUnit.Framework;

namespace Blog.Application.UnitTests.Commands.CheckToken
{
    [TestFixture]
    public class CheckTokenHandlerTests
    {
        [Test]
        public void When_InvalidTokenProvided_Expect_LoginExceptionThrown()
        {
            //Arrange
            var jwtService = new JwtService(new JwtContainerModel());
            var command = new Application.Commands.CheckToken.CheckToken
            {
                Token = "test123"
            };

            //Act
            var handler = new CheckTokenHandler(jwtService);
            
            //Assert
            Assert.ThrowsAsync<TokenInvalidException>(async () =>
            {
                await handler.Handle(command, CancellationToken.None);
            });
;        }

        [Test]
        public void When_ValidTokenProvided_Expect_NoExceptionThrown()
        {
            //Arrange
            var jwtService = new JwtService(new JwtContainerModel());
            var command = new Application.Commands.CheckToken.CheckToken
            {
                Token = jwtService.GenerateToken(new Claim("username", "John")).Value
            };

            //Act
            var handler = new CheckTokenHandler(jwtService);

            //Assert
            Assert.DoesNotThrowAsync(async () =>
            {
                await handler.Handle(command, CancellationToken.None);
            });
        }
    }
}
