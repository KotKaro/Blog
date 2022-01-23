using System;
using System.Threading;
using System.Threading.Tasks;
using Blog.Application.Commands.CreatePost;
using Blog.Domain.Exceptions;
using Blog.Domain.Models.Aggregates.Post;
using Blog.Domain.Repositories;
using Moq;
using Xunit;
using MockFactory = Blog.Tests.Common.MockFactory;

namespace Blog.Application.UnitTests.Commands.CreatePost
{
    public class CreatePostHandlerTests
    {
        private readonly Mock<IPostRepository> _postRepositoryMock;

        public CreatePostHandlerTests()
        {
            _postRepositoryMock = new Mock<IPostRepository>(MockBehavior.Loose);
        }

        [Fact]
        public void When_HandlerConstructedWithoutRepository_Expect_ArgumentNullExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                // ReSharper disable once ObjectCreationAsStatement
                new CreatePostHandler(null);
            });
        }

        [Fact]
        public async Task When_EmptyTitleProvided_Expect_InvalidValueExceptionThrown()
        {
            // Arrange
            var createPostCommandHandler = new CreatePostHandler(_postRepositoryMock.Object);

            //Act + Assert
            await Assert.ThrowsAsync<InvalidValueException>(async () =>
            {
                await createPostCommandHandler.Handle(MockFactory.CreateCreatePostCommand(""), CancellationToken.None);
            });
        }

        [Fact]
        public async Task When_EmptyContentProvided_Expect_InvalidValueExceptionThrown()
        {
            // Arrange
            var createPostCommandHandler = new CreatePostHandler(_postRepositoryMock.Object);

            //Act + Assert
            await Assert.ThrowsAsync<InvalidValueException>(async () =>
            {
                await createPostCommandHandler.Handle(MockFactory.CreateCreatePostCommand(content: ""), CancellationToken.None);
            });
        }

        [Fact]
        public async Task When_CorrectCommandPassed_Expect_RepositoryAddAsyncToBeCalledWithProperValues()
        {
            // Arrange
            var createPostCommandHandler = new CreatePostHandler(_postRepositoryMock.Object);

            //Act
            var command = MockFactory.CreateCreatePostCommand();
            await createPostCommandHandler.Handle(command, CancellationToken.None);

            //Assert
            _postRepositoryMock.Verify(
                r => r.AddAsync(It.Is<Post>(p => p.Content.Value == command.Content && p.Title.Value == command.Title))
            );
        }
    }
}
