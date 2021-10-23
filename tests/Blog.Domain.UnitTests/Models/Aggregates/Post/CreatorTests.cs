using System;
using Blog.Domain.Models.Aggregates.Post;
using Xunit;

namespace Blog.Domain.UnitTests.Models.Aggregates.Post
{
    public class CreatorTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
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
