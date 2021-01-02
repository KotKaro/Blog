using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace Blog.Auth.UnitTests
{
    [TestFixture]
    public class PasswordHasherTests
    {
        [Test]
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
            Assert.IsTrue(hasher.Check(passOne, "test").Verified);
        }
    }
}