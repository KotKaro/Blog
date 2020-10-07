using System;
using System.Threading;
using Blog.Application.Commands.GetPostById;
using NUnit.Framework;
using System.Threading.Tasks;
using Blog.Domain.Exceptions;
using Blog.Domain.Repositories;
using Blog.Tests.Common;
using Moq;
using MockFactory = Blog.Tests.Common.MockFactory;

namespace Blog.Application.UnitTests.Commands.GetPostById
{
    [TestFixture]
    public class GetPostByIdCommandHandlerTests
    {
        private Mock<IPostRepository> _postRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            _postRepositoryMock = new Mock<IPostRepository>();
        }

        [Test]
        public void When_HandlerConstructedWithoutPostRepository_Expect_ArgumentNullExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                // ReSharper disable once ObjectCreationAsStatement
                new GetPostByIdCommandHandler(null, Mapper.GetInstance());
            });
        }

        [Test]
        public void When_HandlerConstructedWithoutMapper_Expect_ArgumentNullExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                // ReSharper disable once ObjectCreationAsStatement
                new GetPostByIdCommandHandler(_postRepositoryMock.Object, null);
            });
        }

        [Test]
        public void When_PostWithSpecificIdDoesNotExistsInRepository_Expect_RecordNotFoundExceptionThrown()
        {
            // Arrange
            var handler = new GetPostByIdCommandHandler(_postRepositoryMock.Object, Mapper.GetInstance());

            // Act + Assert
            Assert.ThrowsAsync<RecordNotFoundException>(async () =>
            {
                await handler.Handle(MockFactory.CreateGetByIdCommand(), CancellationToken.None);
            });
        }

        [Test]
        public async Task When_PostWithSpecificIdExistsInRepository_Expect_PostDtoBeenReturned()
        {
            // Arrange
            var id = Guid.NewGuid();
            _postRepositoryMock.Setup(x => x.GetByIdAsync(id))
                .Returns(Task.FromResult(MockFactory.CreatePost(id)));
            var handler = new GetPostByIdCommandHandler(_postRepositoryMock.Object, Mapper.GetInstance());

            // Act + Assert
            var result = await handler.Handle(MockFactory.CreateGetByIdCommand(id), CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(id));
        }
    }
}
