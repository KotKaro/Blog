using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Blog.Application.Commands.CheckToken;
using Blog.Application.Mappers.Exceptions;
using Blog.Auth;
using Blog.Auth.Models;
using FluentAssertions;
using Xunit;

namespace Blog.Application.UnitTests.Commands.CheckToken
{
    public class CheckTokenHandlerTests
    {
        [Fact]
        public async Task When_InvalidTokenProvided_Expect_LoginExceptionThrown()
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
            await Assert.ThrowsAsync<TokenInvalidException>(async () =>
            {
                await handler.Handle(command, CancellationToken.None);
            });
;        }

        [Fact]
        public async Task When_ValidTokenProvided_Expect_UnitAsResultReturned()
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
            var result = await handler.Handle(command, CancellationToken.None);
            result.Should().NotBeNull();
        }
    }
}
