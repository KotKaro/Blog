using System;
using System.Threading;
using NUnit.Framework;
using System.Threading.Tasks;
using Blog.Application.Commands.CreatePost;
using Blog.Domain.Models.Aggregates.Post;
using Blog.Domain.Repositories;
using Moq;
using MockFactory = Blog.Tests.Common.MockFactory;

namespace Blog.Application.UnitTests.Commands.CreatePost
{
    [TestFixture]
    public class CreatePostHandlerTests
    {
        private Mock<IPostRepository> _postRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            _postRepositoryMock = new Mock<IPostRepository>(MockBehavior.Loose);
        }

        [Test]
        public void When_HandlerConstructedWithoutRepository_Expect_ArgumentNullExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                // ReSharper disable once ObjectCreationAsStatement
                new CreatePostHandler(null);
            });
        }

        [Test]
        public void When_EmptyTitleProvided_Expect_ArgumentExceptionThrown()
        {
            // Arrange
            var createPostCommandHandler = new CreatePostHandler(_postRepositoryMock.Object);

            //Act + Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await createPostCommandHandler.Handle(MockFactory.CreateCreatePostCommand(""), CancellationToken.None);
            });
        }

        [Test]
        public void When_EmptyContentProvided_Expect_ArgumentExceptionThrown()
        {
            // Arrange
            var createPostCommandHandler = new CreatePostHandler(_postRepositoryMock.Object);

            //Act + Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await createPostCommandHandler.Handle(MockFactory.CreateCreatePostCommand(content: ""), CancellationToken.None);
            });
        }

        [Test]
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