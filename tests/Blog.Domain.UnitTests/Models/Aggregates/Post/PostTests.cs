using Blog.Domain.Models.Aggregates.Post;
using System;
using Blog.Tests.Common;
using FluentAssertions;
using Xunit;

namespace Blog.Domain.UnitTests.Models.Aggregates.Post
{
    public class PostTests
    {
        [Fact]
        public void When_PostCreated_Expect_TodayDateToBeSetAsCreationDate()
        {
            //Arrange + Act
            var post = new Domain.Models.Aggregates.Post.Post(
                Guid.NewGuid(),
                new Title("title"),
                new Content("content")
            );

            //Assert
            post.CreationDate.Value.Should().Be(DateTime.Today);
        }

        [Fact]
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

        [Fact]
        public void When_AddPostCalled_Expect_PostGotOneComment()
        {
            //Arrange
            var post = MockFactory.CreatePost();

            //Act
            post.AddComment(MockFactory.CreatComment());

            //Assert
            post.Comments.Count.Should().Be(1);
        }
    }
}
