using FluentAssertions;
using Microsoft.Extensions.Options;
using Xunit;

namespace Blog.Auth.UnitTests
{
    public class PasswordHasherTests
    {
        [Fact]
        public void When_PasswordHashGenerated_Expect_PasswordHashCheckReturnsTrue()
        {
            //Arrange
            var hasher = new PasswordHasher
            (
                new OptionsWrapper<HashingOptions>(new HashingOptions { Iterations = 1 })
            );
            
            //Act
            var passOne = hasher.Hash("test");
            
            //Arrange
            hasher.Check(passOne, "test").Verified.Should().BeTrue();
        }
    }
}