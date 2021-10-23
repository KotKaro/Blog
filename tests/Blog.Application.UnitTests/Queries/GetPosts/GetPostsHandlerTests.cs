using System;
using System.Linq;
using System.Threading;
using Blog.Application.Queries.GetPosts;
using Moq;
using System.Threading.Tasks;
using Blog.Domain.Repositories;
using Blog.Tests.Common;
using FluentAssertions;
using Xunit;
using MockFactory = Blog.Tests.Common.MockFactory;

namespace Blog.Application.UnitTests.Queries.GetPosts
{
    public class GetPostsHandlerTests
    {
        private readonly Mock<IPostRepository> _postRepositoryMock;

        public GetPostsHandlerTests()
        {
            _postRepositoryMock = new Mock<IPostRepository>();
        }

        [Fact]
        public void When_GetPostQueryHandlerConstructedWithoutPostRepository_Expect_ArgumentNullExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                // ReSharper disable once ObjectCreationAsStatement
                new GetPostsQueryHandler(null, Mapper.GetInstance());
            });
        }

        [Fact]
        public void When_GetPostQueryHandlerConstructedWithoutMapper_Expect_ArgumentNullExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                // ReSharper disable once ObjectCreationAsStatement
                new GetPostsQueryHandler(_postRepositoryMock.Object, null);
            });
        }

        [Fact]
        public async Task When_HandledAndTenElementsFromFirstPageRequestedAndTenElementExists_Expect_TenElementsReturned()
        {
            //Arrange
            _postRepositoryMock.Setup(x => x.GetAllAsync(1, 10))
                .Returns(Task.FromResult(Enumerable.Range(0, 10).Select(_ => MockFactory.CreatePost())));

            var sut = new GetPostsQueryHandler(_postRepositoryMock.Object, Mapper.GetInstance());

            //Act
            var result = await sut.Handle(MockFactory.CreateGetPostsQuery(), CancellationToken.None);

            //Assert
            result.Length.Should().Be(10);
        }
    }
}
