using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Blog.API.Controllers;
using Blog.Domain.Exceptions;
using Blog.Domain.Models.Aggregates.Post;
using Blog.IntegrationTests.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using MockFactory = Blog.Tests.Common.MockFactory;

namespace Blog.IntegrationTests.Controllers
{
    [TestFixture]
    public class CommentsControllerTests : IntegrationTestBase
    {
        private CommentsController _commentsController;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var mediator = Container.Resolve<IMediator>();
            _commentsController = new CommentsController(mediator);
        }

        [SetUp]
        public void SetUp()
        {
            BlogContext.Set<Comment>().RemoveRange(BlogContext.Set<Comment>());
            BlogContext.Set<Post>().RemoveRange(BlogContext.Set<Post>());
        }

        [Test]
        public void When_CommentCreatedAndPostDoesNotExists_Expect_RecordNotFoundExceptionThrown()
        {
            // Act + Assert
            Assert.ThrowsAsync<RecordNotFoundException>(async () =>
            {
                await _commentsController.CreateAsync(MockFactory.CreateCreateCommentCommand());
            });
        }

        [Test]
        public async Task When_CommentCreatedAndPostExist_Expect_PostToBeAdded()
        {
            // Arrange
            await BlogContext.Set<Post>().AddAsync(MockFactory.CreatePost());
            await BlogContext.SaveChangesAsync();
            var post = await BlogContext.Set<Post>().FirstOrDefaultAsync();

            // Act
            await _commentsController.CreateAsync(MockFactory.CreateCreateCommentCommand(postId: post.Id));
            await UnitOfWork.SaveEntitiesAsync();

            // Assert
            Assert.That(post.Comments.Count, Is.EqualTo(1));
        }

        [Test]
        public void When_CommentDeleteCalledAndCommentDoesNotExists_Expect_RecordNotFoundExceptionThrown()
        {
            // Act + Assert
            Assert.ThrowsAsync<RecordNotFoundException>(async () =>
            {
                await _commentsController.DeleteAsync(Guid.NewGuid());
            });
        }

        [Test]
        public async Task When_CommentDeleteCalledAndCommentExist_Expect_PostToBeDeleted()
        {
            // Arrange
            await BlogContext.Set<Post>().AddAsync(MockFactory.CreatePost());
            await BlogContext.SaveChangesAsync();
            var post = await BlogContext.Set<Post>().FirstOrDefaultAsync();
            await _commentsController.CreateAsync(MockFactory.CreateCreateCommentCommand(postId: post.Id));
            await UnitOfWork.SaveEntitiesAsync();

            // Act
            await _commentsController.DeleteAsync(post.Comments.First().Id);
            await UnitOfWork.SaveEntitiesAsync();

            // Assert
            Assert.That(post.Comments.Count, Is.EqualTo(0));
        }
    }
}
