using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Blog.API.Controllers;
using Blog.Auth.Abstractions;
using Blog.Domain.Models.Aggregates.Post;
using Blog.Infrastructure;
using Blog.IntegrationTests.Common;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Moq;
using Xunit;
using MockFactory = Blog.Tests.Common.MockFactory;

namespace Blog.IntegrationTests.Controllers
{
    [Collection(nameof(BlogTestCollection))]
    public class PostControllerTests
    {
        private readonly BlogDbContext _blogContext;
        private readonly IMediator _mediator;
        private readonly IJwtService _jwtService;
        private readonly BlogApplicationFactory _factory;

        public PostControllerTests(BlogApplicationFactory factory)
        {
            _factory = factory;
            _blogContext = (BlogDbContext)factory.Services.GetService(typeof(BlogDbContext));
            _mediator = (IMediator)factory.Services.GetService(typeof(IMediator));
            _jwtService = (IJwtService)factory.Services.GetService(typeof(IJwtService));

            _blogContext!.Set<Post>().RemoveRange(_blogContext.Set<Post>());
            _blogContext.SaveChanges();
        }

        [Fact]
        public async Task When_GetCalledWithPageOneSizeTen_Expect_TenPostsReturned()
        {
            //Arrange
            await _blogContext.Set<Post>().AddRangeAsync
            (
                Enumerable.Range(0, 10).Select(_ => MockFactory.CreatePost())
            );

            await _blogContext.SaveChangesAsync();

            //Act
            var result = await _factory.GetPosts();

            //Assert
            result!.Length.Should().Be(10);
        }

        [Fact]
        public async Task
            When_GetCalledWithPageOneSizeTenAndNextGetCalledWithPageTwoOfSizeTen_Expect_AllFirstPagePostIdsDifferentThanSecondPagePostIds()
        {
            //Arrange
            await _blogContext.Set<Post>().AddRangeAsync
            (
                Enumerable.Range(0, 20).Select(_ => MockFactory.CreatePost())
            );

            await _blogContext.SaveChangesAsync();

            //Act
            var firstPage = await _factory.GetPosts();
            var secondPage = await _factory.GetPosts(2);

            //Assert
            firstPage.Length.Should().Be(10);
            secondPage.Length.Should().Be(10);
            firstPage.Select(x => x.Id)
                .All(x => secondPage.Select(y => y.Id).Any(y => y != x))
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task When_CreatePostCalledWithProperCommand_Expect_PostCreated()
        {
            //Act
            var result = await _factory.CreatePost(MockFactory.CreateCreatePostCommand());;

            //Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task When_PostWithSpecificIdExists_Expect_PostDtoWithSameIdReturned()
        {
            //Arrange
            var post = MockFactory.CreatePost();
            await _blogContext.Set<Post>().AddAsync(post);
            await _blogContext.SaveChangesAsync();

            //Act
            var result = await _factory.GetPostById(post.Id);

            //Assert
            result.Id.Should().Be(post.Id);
        }
        
        [Fact]
        public async Task When_TokenNotProvidedForUpdatePost_Expect_UnauthorizedReturned()
        {
            // Act
            var response = await _factory.UpdatePostWithoutHeaders(MockFactory.CreateUpdatePostCommand(Guid.NewGuid()));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task When_UpdateCalledAndPostDoesNotExists_Expect_NotFoundCodeReturned()
        {
            // Act
            var response = await _factory.UpdatePost(MockFactory.CreateUpdatePostCommand(Guid.NewGuid()));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task When_UpdateCalledAndPostExists_Expect_PostToBeUpdated()
        {
            //Arrange
            var post = MockFactory.CreatePost();
            await _blogContext.Set<Post>().AddAsync(post);
            await _blogContext.SaveChangesAsync();

            //Act
            await _factory.UpdatePost(MockFactory.CreateUpdatePostCommand(post.Id, "updatedTitle", "updatedContent"));

            //Assert
            var updated = await _factory.GetPostById(post.Id);
            updated.Title.Should().Be("updatedTitle");
            updated.Content.Should().Be("updatedContent");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task When_UpdateCalledAndPostTitleNotProvided_Expect_BadRequestStatusCodeReturned(string title)
        {
            //Arrange
            var post = MockFactory.CreatePost();
            await _blogContext.Set<Post>().AddAsync(post);
            await _blogContext.SaveChangesAsync();

            //Act
            var response = await _factory.UpdatePost(MockFactory.CreateUpdatePostCommand(post.Id, title, "updatedContent"));

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task When_UpdateCalledAndPostContentNotProvided_Expect_BadRequestResponseCode(string content)
        {
            //Arrange
            var post = MockFactory.CreatePost();
            await _blogContext.Set<Post>().AddAsync(post);
            await _blogContext.SaveChangesAsync();

            //Act
            var response = await _factory.UpdatePost(MockFactory.CreateUpdatePostCommand(post.Id, "title", content));

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task When_PostNotExistsAndDeleteCall_Expect_NotFoundStatusReturned()
        {
            //Act
            var response = await _factory.DeletePost(Guid.NewGuid());
            
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task When_PostIsDeleted_Expect_StatusCodeIsNoContent()
        {
            //Arrange
            var post = MockFactory.CreatePost();
            await _blogContext.Set<Post>().AddAsync(post);
            await _blogContext.SaveChangesAsync();

            //Act
            var response = await _factory.DeletePost(post.Id);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
        
        [Fact]
        public async Task When_PostIsDeleted_Expect_WhenGettingRecordByIdStatusIsNotFound()
        {
            //Arrange
            var post = MockFactory.CreatePost();
            await _blogContext.Set<Post>().AddAsync(post);
            await _blogContext.SaveChangesAsync();

            //Act
            await _factory.DeletePost(post.Id);

            //Assert
            var result = await _factory.GetPostByIdNoParse(post.Id);
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task When_GetByIdCalledForPostWithComments_Expect_PostGotLoadedComments()
        {
            //Arrange
            var controller = CreatePostControllerWithTokenHeader();
            var post = await controller.Create(MockFactory.CreateCreatePostCommand());
            var commentController = new CommentsController(_mediator);
            await commentController.CreateAsync(MockFactory.CreateCreateCommentCommand(postId: post.Id));

            //Act
            var retrievedPost = await _factory.GetPostById(post.Id);

            //Assert

            retrievedPost.Comments.Length.Should().Be(1);
        }

        private PostsController CreatePostControllerWithTokenHeader()
        {
            var controller = new PostsController(_mediator);
            var token = _jwtService
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