using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Autofac;
using Blog.API.Controllers;
using Blog.Auth.Abstractions;
using Blog.Domain.Exceptions;
using Blog.Domain.Models.Aggregates.Post;
using Blog.IntegrationTests.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;
using MockFactory = Blog.Tests.Common.MockFactory;

namespace Blog.IntegrationTests.Controllers
{
    [TestFixture]
    public class PostControllerTests : IntegrationTestBase
    {
        [SetUp]
        public void SetUp()
        {
            BlogContext.Set<Post>().RemoveRange(BlogContext.Set<Post>());
        }

        [Test]
        public async Task When_GetCalledWithPageOneSizeTen_Expect_TenPostsReturned()
        {
            //Arrange
            var mediator = Container.Resolve<IMediator>();
            var controller = new PostsController(mediator);

            await BlogContext.Set<Post>().AddRangeAsync
            (
                Enumerable.Range(0, 10).Select(x => MockFactory.CreatePost())
            );

            await BlogContext.SaveChangesAsync();

            //Act
            var result = await controller.GetAll();

            //Assert
            Assert.That(result.Length, Is.EqualTo(10));
        }

        [Test]
        public async Task When_GetCalledWithPageOneSizeTenAndNextGetCalledWithPageTwoOfSizeTen_Expect_AllFirstPagePostIdsDifferentThanSecondPagePostIds()
        {
            //Arrange
            var mediator = Container.Resolve<IMediator>();
            var controller = new PostsController(mediator);

            await BlogContext.Set<Post>().AddRangeAsync
            (
                Enumerable.Range(0, 20).Select(x => MockFactory.CreatePost())
            );

            await BlogContext.SaveChangesAsync();

            //Act
            var firstPage = await controller.GetAll();
            var secondPage = await controller.GetAll(2);

            //Assert
            Assert.That(firstPage.Length, Is.EqualTo(10));
            Assert.That(secondPage.Length, Is.EqualTo(10));
            Assert.That(firstPage.Select(x => x.Id)
                .All(x => secondPage.Select(y => y.Id).Any(y => y != x))
            );
        }

        [Test]
        public async Task When_CreatePostCalledWithProperCommand_Expect_PostCreated()
        {
            //Arrange
            var controller = CreatePostControllerWithTokenHeader(Container);

            //Act
            var result = await controller.Create(MockFactory.CreateCreatePostCommand());

            //Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task When_PostWithSpecificIdExists_Expect_PostDtoWithSameIdReturned()
        {
            //Arrange
            var mediator = Container.Resolve<IMediator>();
            var controller = new PostsController(mediator);

            var post = MockFactory.CreatePost();
            await BlogContext.Set<Post>().AddAsync(post);
            await BlogContext.SaveChangesAsync();

            //Act
            var result = await controller.GetById(post.Id);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(post.Id));
        }

        [Test]
        public void When_UpdateCalledAndPostDoesNotExists_Expect_RecordNotFoundExceptionThrown()
        {
            //Arrange
            var controller = CreatePostControllerWithTokenHeader(Container);

            Assert.ThrowsAsync<RecordNotFoundException>(async () =>
            {
                await controller.Update(MockFactory.CreateUpdatePostCommand(Guid.NewGuid()));
            });
        }

        [Test]
        public async Task When_UpdateCalledAndPostExists_Expect_PostToBeUpdated()
        {
            //Arrange
            var controller = CreatePostControllerWithTokenHeader(Container);

            var post = MockFactory.CreatePost();
            await BlogContext.Set<Post>().AddAsync(post);
            await BlogContext.SaveChangesAsync();

            //Act
            await controller.Update(MockFactory.CreateUpdatePostCommand(post.Id, "updatedTitle", "updatedContent"));

            //Assert
            var updated = await controller.GetById(post.Id);
            Assert.That(updated.Title, Is.EqualTo("updatedTitle"));
            Assert.That(updated.Content, Is.EqualTo("updatedContent"));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public async Task When_UpdateCalledAndPostTitleNotProvided_Expect_ArgumentExceptionToBeThrown(string title)
        {
            //Arrange
            var controller = CreatePostControllerWithTokenHeader(Container);

            var post = MockFactory.CreatePost();
            await BlogContext.Set<Post>().AddAsync(post);
            await BlogContext.SaveChangesAsync();

            //Act + Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await controller.Update(MockFactory.CreateUpdatePostCommand(post.Id, title, "updatedContent"));
            });
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public async Task When_UpdateCalledAndPostContentNotProvided_Expect_ArgumentExceptionToBeThrown(string content)
        {
            //Arrange
            var controller = CreatePostControllerWithTokenHeader(Container);

            var post = MockFactory.CreatePost();
            await BlogContext.Set<Post>().AddAsync(post);
            await BlogContext.SaveChangesAsync();

            //Act + Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await controller.Update(MockFactory.CreateUpdatePostCommand(post.Id, "title", content));
            });
        }

        [Test]
        public void When_PostNotExistsAndDeleteCall_Expect_RecordNotFoundExceptionThrown()
        {
            //Arrange
            var controller = CreatePostControllerWithTokenHeader(Container);

            //Act + Assert
            Assert.ThrowsAsync<RecordNotFoundException>(async () =>
            {
                await controller.DeleteAsync(Guid.NewGuid());
            });
        }

        [Test]
        public async Task When_PostIsDeleted_Expect_GetByIdThrowsRecordNotFoundException()
        {
            //Arrange
            var post = MockFactory.CreatePost();
            await BlogContext.Set<Post>().AddAsync(post);
            await BlogContext.SaveChangesAsync();

            var controller = CreatePostControllerWithTokenHeader(Container);

            //Act
            await controller.DeleteAsync(post.Id);

            //Assert
            Assert.ThrowsAsync<RecordNotFoundException>(async () =>
            {
                await controller.GetById(Guid.NewGuid());
            });
        }

        [Test]
        public async Task When_GetByIdCalledForPostWithComments_Expect_PostGotLoadedComments()
        {
            //Arrange
            var controller = CreatePostControllerWithTokenHeader(Container);
            var post = await controller.Create(MockFactory.CreateCreatePostCommand());

            var commentController = new CommentsController(Container.Resolve<IMediator>());
            await commentController.CreateAsync(MockFactory.CreateCreateCommentCommand(postId: post.Id));

            await BlogContext.SaveChangesAsync();


            //Act
            var retrievedPost = await controller.GetById(post.Id);

            //Assert
            Assert.That(retrievedPost.Comments.Length, Is.EqualTo(1));
        }

        private static PostsController CreatePostControllerWithTokenHeader(IContainer container)
        {
            var mediator = container.Resolve<IMediator>();
            var controller = new PostsController(mediator);
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
