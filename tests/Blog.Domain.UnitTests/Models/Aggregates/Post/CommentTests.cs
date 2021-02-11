using System;
using Blog.Domain.Models.Aggregates.Post;
using NUnit.Framework;

namespace Blog.Domain.UnitTests.Models.Aggregates.Post
{
    [TestFixture]
    public class CommentTests
    {
        [Test]
        public void When_CommentCreatedWithoutCreator_Expect_ArgumentNullException()
        {
            // Act + Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // ReSharper disable once UnusedVariable
                var comment = new Comment(Guid.NewGuid(), null, new Content("test"));
            });
        }

        [Test]
        public void When_CommentCreatedWithoutContent_Expect_ArgumentNullException()
        {
            // Act + Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // ReSharper disable once UnusedVariable
                var comment = new Comment(Guid.NewGuid(), new Creator("test"), null);
            });
        }
    }
}
