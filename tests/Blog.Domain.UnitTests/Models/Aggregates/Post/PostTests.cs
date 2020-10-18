using Blog.Domain.Models.Aggregates.Post;
using NUnit.Framework;
using System;

namespace Blog.Domain.UnitTests.Models.Aggregates.Post
{
    [TestFixture]
    public class PostTests
    {
        [Test]
        public void When_PostCreated_Expect_TodayDateToBeSetAsCreationDate()
        {
            //Arrange + Act
            var post = new Domain.Models.Aggregates.Post.Post(
                Guid.NewGuid(),
                new Title("title"),
                new Content("content")
            );

            //Assert
            Assert.AreEqual(DateTime.Today, post.CreationDate.Value);
        }
    }
}
