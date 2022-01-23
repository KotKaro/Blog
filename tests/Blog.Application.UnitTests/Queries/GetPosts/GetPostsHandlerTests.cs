using System.Linq;
using System.Threading;
using Blog.Application.Queries.GetPosts;
using Moq;
using System.Threading.Tasks;
using AutoFixture;
using Blog.Domain.Repositories.PostReadRepository;
using FluentAssertions;
using Xunit;
using MockFactory = Blog.Tests.Common.MockFactory;

namespace Blog.Application.UnitTests.Queries.GetPosts
{
    public class GetPostsHandlerTests
    {
        private readonly Mock<IPostReadRepository> _postRepositoryMock;
        private readonly Fixture _fixture;

        public GetPostsHandlerTests()
        {
            _postRepositoryMock = new Mock<IPostReadRepository>();
            _fixture = new Fixture();
        }


        [Fact]
        public async Task When_HandledAndTenElementsFromFirstPageRequestedAndTenElementExists_Expect_TenElementsReturned()
        {
            //Arrange
            _postRepositoryMock.Setup(x => x.GetAllAsync(1, 10))
                .ReturnsAsync(_fixture.CreateMany<PostDTO>(10).ToArray);

            var sut = new GetPostsQueryHandler(_postRepositoryMock.Object);

            //Act
            var result = await sut.Handle(MockFactory.CreateGetPostsQuery(), CancellationToken.None);

            //Assert
            result.Length.Should().Be(10);
        }
    }
}
