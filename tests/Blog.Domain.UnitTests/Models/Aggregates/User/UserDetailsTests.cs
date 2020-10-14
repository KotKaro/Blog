using Blog.Domain.Models.Aggregates.User;
using NUnit.Framework;

namespace Blog.Domain.UnitTests.Models.Aggregates.User
{
    [TestFixture]
    public class UserDetailsTests
    {
        [Test]
        public void When_TwoSameUserDetailsCompared_Expect_ComparisionResultBeTrue()
        {
            // Arrange
            var firstUserDetails = new UserDetails("test", "test");
            var secondUserDetails = new UserDetails("test", "test");

            // Act
            var result = firstUserDetails.Equals(secondUserDetails);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void When_TwoDiffrentUserDetailsCompared_Expect_ComparisionResultBeFalse()
        {
            // Arrange
            var firstUserDetails = new UserDetails("test", "test");
            var secondUserDetails = new UserDetails("test1", "test");

            // Act
            var result = firstUserDetails.Equals(secondUserDetails);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
