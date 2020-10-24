using System;
using Blog.Domain.Models.Aggregates.Post;
using NUnit.Framework;

namespace Blog.Domain.UnitTests.Models.Aggregates.Post
{
    [TestFixture]
    public class CreatorTests
    {
        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void When_CreatorCreatedWithWrongName_Expect_ArgumentExceptionThrown(string text)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                // ReSharper disable once ObjectCreationAsStatement
                new Creator(text);
            });
        }
    }
}
