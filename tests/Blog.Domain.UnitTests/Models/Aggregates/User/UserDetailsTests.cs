using Blog.Domain.Models.Aggregates.User;
using FluentAssertions;
using Xunit;

namespace Blog.Domain.UnitTests.Models.Aggregates.User
{
    public class UserDetailsTests
    {
        [Fact]
        public void When_TwoSameUserDetailsCompared_Expect_ComparisionResultBeTrue()
        {
            // Arrange
            var firstUserDetails = new UserDetails("test", "test");
            var secondUserDetails = new UserDetails("test", "test");

            // Act
            var result = firstUserDetails.Equals(secondUserDetails);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void When_TwoDifferentUserDetailsCompared_Expect_ComparisionResultBeFalse()
        {
            // Arrange
            var firstUserDetails = new UserDetails("test", "test");
            var secondUserDetails = new UserDetails("test1", "test");

            // Act
            var result = firstUserDetails.Equals(secondUserDetails);

            // Assert
            result.Should().BeFalse();
        }
    }
}
