using Blog.Domain.Models.Aggregates.Post;
using NUnit.Framework;
using System;
using Blog.Tests.Common;

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

        [Test]
        public void When_AddedPostIsNull_Expect_ArgumentNullExceptionThrown()
        {
            //Arrange
            var post = MockFactory.CreatePost();

            //Act + Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                post.AddComment(null);
            });

        }

        [Test]
        public void When_AddPostCalled_Expect_PostGotOneComment()
        {
            //Arrange
            var post = MockFactory.CreatePost();

            //Act
            post.AddComment(MockFactory.CreatComment());

            //Assert
            Assert.That(post.Comments.Count, Is.EqualTo(1));
        }
    }
}
