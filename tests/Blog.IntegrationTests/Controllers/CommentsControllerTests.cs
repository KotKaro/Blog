using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blog.API.Controllers;
using Blog.Application.Mappers.Exceptions;
using Blog.Auth.Abstractions;
using Blog.Domain.DataAccess;
using Blog.Domain.Exceptions;
using Blog.Domain.Models.Aggregates.Post;
using Blog.Infrastructure;
using Blog.IntegrationTests.Common;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Moq;
using Xunit;
using MockFactory = Blog.Tests.Common.MockFactory;

namespace Blog.IntegrationTests.Controllers
{
    [Collection(nameof(BlogTestCollection))]
    public class CommentsControllerTests
    {
        private readonly CommentsController _commentsController;
        private readonly BlogDbContext _blogDbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public CommentsControllerTests(BlogApplicationFactory factory)
        {
            _blogDbContext = (BlogDbContext)factory.Services.GetService(typeof(BlogDbContext));
            _unitOfWork = (IUnitOfWork)factory.Services.GetService(typeof(IUnitOfWork));
            _mediator = (IMediator)factory.Services.GetService(typeof(IMediator));
            
            _commentsController = CreateCommentsControllerWithTokenHeader(factory.Services);
            _blogDbContext!.Set<Comment>().RemoveRange(_blogDbContext.Set<Comment>());
            _blogDbContext.Set<Post>().RemoveRange(_blogDbContext.Set<Post>());
        }


        [Fact]
        public async Task When_CommentCreatedAndPostDoesNotExists_Expect_RecordNotFoundExceptionThrown()
        {
            // Act + Assert
            await Assert.ThrowsAsync<RecordNotFoundException>(async () =>
            {
                await _commentsController.CreateAsync(MockFactory.CreateCreateCommentCommand());
            });
        }

        [Fact]
        public async Task When_CommentCreatedAndPostExist_Expect_PostToBeAdded()
        {
            // Arrange
            await _blogDbContext.Set<Post>().AddAsync(MockFactory.CreatePost());
            await _blogDbContext.SaveChangesAsync();
            var post = await _blogDbContext.Set<Post>().FirstOrDefaultAsync();

            // Act
            await _commentsController.CreateAsync(MockFactory.CreateCreateCommentCommand(postId: post.Id));
            await _unitOfWork.SaveEntitiesAsync();

            // Assert
            post.Comments.Count.Should().Be(1);
        }

        [Fact]
        public async Task When_CommentDeleteCalledAndCommentDoesNotExists_Expect_RecordNotFoundExceptionThrown()
        {
            // Act + Assert
            await Assert.ThrowsAsync<RecordNotFoundException>(async () =>
            {
                await _commentsController.DeleteAsync(Guid.NewGuid());
            });
        }

        [Fact]
        public async Task When_CommentDeleteCalledAndCommentExist_Expect_PostToBeDeleted()
        {
            // Arrange
            await _blogDbContext.Set<Post>().AddAsync(MockFactory.CreatePost());
            await _blogDbContext.SaveChangesAsync();
            var post = await _blogDbContext.Set<Post>().FirstOrDefaultAsync();
            await _commentsController.CreateAsync(MockFactory.CreateCreateCommentCommand(postId: post.Id));
            await _unitOfWork.SaveEntitiesAsync();

            // Act
            await _commentsController.DeleteAsync(post.Comments.First().Id);
            await _unitOfWork.SaveEntitiesAsync();

            // Assert
            post.Comments.Count.Should().Be(0);
        }

        [Fact]
        public async Task When_CommentDeleteCalledAndTokenNotProvided_Expect_TokenInvalidExceptionThrown()
        {
            // Arrange
            await _blogDbContext.Set<Post>().AddAsync(MockFactory.CreatePost());
            await _blogDbContext.SaveChangesAsync();
            var post = await _blogDbContext.Set<Post>().FirstOrDefaultAsync();
            var controller = new CommentsController(_mediator);
            await controller.CreateAsync(MockFactory.CreateCreateCommentCommand(postId: post.Id));
            await _unitOfWork.SaveEntitiesAsync();

            // Act + Assert
            await Assert.ThrowsAsync<TokenInvalidException>(async () =>
            {
                await controller.DeleteAsync(post.Comments.First().Id);
            });
        }

        private CommentsController CreateCommentsControllerWithTokenHeader(IServiceProvider container)
        {
            var controller = new CommentsController(_mediator);
            var token = ((IJwtService)container.GetService(typeof(IJwtService)))
                !.GenerateToken(new Claim("username", "John"))
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
