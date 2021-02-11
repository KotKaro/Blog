using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Autofac;
using Blog.API.Controllers;
using Blog.Application.Mappers.Exceptions;
using Blog.Auth.Abstractions;
using Blog.Domain.Exceptions;
using Blog.Domain.Models.Aggregates.Post;
using Blog.IntegrationTests.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Moq;
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
            _commentsController = CreateCommentsControllerWithTokenHeader(Container);
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

        [Test]
        public async Task When_CommentDeleteCalledAndTokenNotProvided_Expect_TokenInvalidExceptionThrown()
        {
            // Arrange
            await BlogContext.Set<Post>().AddAsync(MockFactory.CreatePost());
            await BlogContext.SaveChangesAsync();
            var post = await BlogContext.Set<Post>().FirstOrDefaultAsync();
            var controller = new CommentsController(Container.Resolve<IMediator>());
            await controller.CreateAsync(MockFactory.CreateCreateCommentCommand(postId: post.Id));
            await UnitOfWork.SaveEntitiesAsync();

            // Act + Assert
            Assert.ThrowsAsync<TokenInvalidException>(async () =>
            {
                await controller.DeleteAsync(post.Comments.First().Id);
            });
        }

        public static CommentsController CreateCommentsControllerWithTokenHeader(IContainer container)
        {
            var mediator = container.Resolve<IMediator>();
            var controller = new CommentsController(mediator);
            var token = container.Resolve<IJwtService>()
                .GenerateToken(new Claim("username", "John"))
                .Value;

            var httpContextMock = new Mock<HttpContext>();
            var httpRequestMock = new Mock<HttpRequest>();
            var headers = new Dictionary<string, StringValues> { { "jwt-token", token } };

            httpRequestMock.Setup(x => x.Headers)
                .Returns(new HeaderDictionary(headers));

            httpContextMock.Setup(x => x.Request)
                .Returns(httpRequestMock.Object);

            controller.ControllerContext = new ControllerContext(
                new ActionContext(httpContextMock.Object, new RouteData(), new ControllerActionDescriptor())
            );

            return controller;
        }
    }
}
