using System;
using System.Threading;
using System.Threading.Tasks;
using Blog.Application.Queries.GetPostById;
using Blog.Domain.Exceptions;
using Blog.Domain.Repositories;
using Blog.Tests.Common;
using FluentAssertions;
using Moq;
using Xunit;
using MockFactory = Blog.Tests.Common.MockFactory;

namespace Blog.Application.UnitTests.Commands.GetPostById
{
    public class GetPostByIdHandlerTests
    {
        private Mock<IPostRepository> _postRepositoryMock;

        public GetPostByIdHandlerTests()
        {
            _postRepositoryMock = new Mock<IPostRepository>();
        }

        [Fact]
        public void When_HandlerConstructedWithoutPostRepository_Expect_ArgumentNullExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                // ReSharper disable once ObjectCreationAsStatement
                new GetPostByIdQueryHandler(null, Mapper.GetInstance());
            });
        }

        [Fact]
        public void When_HandlerConstructedWithoutMapper_Expect_ArgumentNullExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                // ReSharper disable once ObjectCreationAsStatement
                new GetPostByIdQueryHandler(_postRepositoryMock.Object, null);
            });
        }

        [Fact]
        public async Task When_PostWithSpecificIdDoesNotExistsInRepository_Expect_RecordNotFoundExceptionThrown()
        {
            // Arrange
            var handler = new GetPostByIdQueryHandler(_postRepositoryMock.Object, Mapper.GetInstance());

            // Act + Assert
            await Assert.ThrowsAsync<RecordNotFoundException>(async () =>
            {
                await handler.Handle(MockFactory.CreateGetByIdQuery(), CancellationToken.None);
            });
        }

        [Fact]
        public async Task When_PostWithSpecificIdExistsInRepository_Expect_PostDtoBeenReturned()
        {
            // Arrange
            var id = Guid.NewGuid();
            _postRepositoryMock.Setup(x => x.GetByIdAsync(id))
                .Returns(Task.FromResult(MockFactory.CreatePost(id)));
            var handler = new GetPostByIdQueryHandler(_postRepositoryMock.Object, Mapper.GetInstance());

            // Act + Assert
            var result = await handler.Handle(MockFactory.CreateGetByIdQuery(id), CancellationToken.None);

            result.Id.Should().Be(id);
        }
    }
}
