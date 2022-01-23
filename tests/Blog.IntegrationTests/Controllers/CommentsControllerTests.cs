using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Blog.Domain.Models.Aggregates.Post;
using Blog.Infrastructure;
using Blog.IntegrationTests.Common;
using FluentAssertions;
using Xunit;
using MockFactory = Blog.Tests.Common.MockFactory;

namespace Blog.IntegrationTests.Controllers
{
    [Collection(nameof(BlogTestCollection))]
    public class CommentsControllerTests
    {
        private readonly BlogApplicationFactory _factory;

        public CommentsControllerTests(BlogApplicationFactory factory)
        {
            _factory = factory;
            var blogDbContext = (BlogDbContext)factory.Services.GetService(typeof(BlogDbContext));
            blogDbContext!.Set<Comment>().RemoveRange(blogDbContext.Set<Comment>());
            blogDbContext.Set<Post>().RemoveRange(blogDbContext.Set<Post>());
        }

        [Fact]
        public async Task When_CommentCreatedAndPostDoesNotExists_Expect_NotFoundStatusCodeReturned()
        {
            // Act
            var response = await _factory.CreateComment(MockFactory.CreateCreateCommentCommand());
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task When_CommentCreatedAndPostExist_Expect_PostToBeAdded()
        {
            // Arrange
            var post = await _factory.CreatePost(MockFactory.CreateCreatePostCommand());

            // Act
            await _factory.CreateComment(MockFactory.CreateCreateCommentCommand(postId: post.Id));

            // Assert
            post = await _factory.GetPostById(post.Id);
            post.Comments.Length.Should().Be(1);
        }

        [Fact]
        public async Task When_CommentDeleteCalledAndCommentDoesNotExists_Expect_NotFoundStatusCodeReturned()
        {
            // Act
            var response = await _factory.DeleteComment(Guid.NewGuid());
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task When_PostGotSingleCommentAndCommentIsDeleted_Expect_RetrievedPostGotZeroComments()
        {
            // Arrange
            var post = await _factory.CreatePost(MockFactory.CreateCreatePostCommand());
            await _factory.CreateComment(MockFactory.CreateCreateCommentCommand(postId: post.Id));
            post = await _factory.GetPostById(post.Id);
            
            // Act
            await _factory.DeleteComment(post.Comments.First().Id);

            // Assert
            post = await _factory.GetPostById(post.Id);
            post.Comments.Length.Should().Be(0);
        }

        [Fact]
        public async Task When_CommentDeleteCalledAndTokenNotProvided_Expect_TokenInvalidExceptionThrown()
        {
            // Arrange
            var post = await _factory.CreatePost(MockFactory.CreateCreatePostCommand());
            await _factory.CreateComment(MockFactory.CreateCreateCommentCommand(postId: post.Id));
            post = await _factory.GetPostById(post.Id);

            // Act
            var response = await _factory.DeleteCommentWithoutHeaders(post.Comments.First().Id);
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
