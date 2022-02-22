using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Blog.IntegrationTests.Common;
using FluentAssertions;
using Xunit;
using MockFactory = Blog.Tests.Common.MockFactory;

namespace Blog.IntegrationTests.Controllers
{
    [Collection(nameof(BlogTestCollection))]
    public class PostControllerTests
    {
        private readonly BlogApplicationFactory _factory;

        public PostControllerTests(BlogApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task When_GetCalledWithPageOneSizeTen_Expect_TenPostsReturned()
        {
            var tasks = Enumerable.Range(0, 10)
                .Select(_ => _factory.CreatePost(MockFactory.CreateCreatePostCommand()))
                .ToArray();
            await Task.WhenAll(tasks);
          
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
            var tasks = Enumerable.Range(0, 20)
                .Select(_ => _factory.CreatePost(MockFactory.CreateCreatePostCommand()))
                .ToArray();
            await Task.WhenAll(tasks);
            
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
            //Arrange
            const string testTitle = "testTitle";
            const string testContent = "testContent";
            
            //Act
            var result = await _factory.CreatePost(MockFactory.CreateCreatePostCommand(testTitle, testContent));;

            //Assert
            result.Should().NotBeNull();
            result.Content.Should().Be(testContent);
            result.Title.Should().Be(testTitle);
        }

        [Fact]
        public async Task When_PostWithSpecificIdExists_Expect_PostDtoWithSameIdReturned()
        {
            //Arrange
            var post = await _factory.CreatePost(MockFactory.CreateCreatePostCommand());

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
            var post = await _factory.CreatePost(MockFactory.CreateCreatePostCommand());

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
            var post = await _factory.CreatePost(MockFactory.CreateCreatePostCommand());

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
            var post = await _factory.CreatePost(MockFactory.CreateCreatePostCommand());

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
            var post = await _factory.CreatePost(MockFactory.CreateCreatePostCommand());

            //Act
            var response = await _factory.DeletePost(post.Id);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
        
        [Fact]
        public async Task When_PostIsDeleted_Expect_WhenGettingRecordByIdStatusIsNotFound()
        {
            //Arrange
            var post = await _factory.CreatePost(MockFactory.CreateCreatePostCommand());

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
            var post = await _factory.CreatePost(MockFactory.CreateCreatePostCommand());
            await _factory.CreateComment(MockFactory.CreateCreateCommentCommand(postId: post.Id));
            
            //Act
            var retrievedPost = await _factory.GetPostById(post.Id);

            //Assert
            retrievedPost.Comments.Length.Should().Be(1);
        }
    }
}