using System;
using System.Linq;
using System.Threading;
using Blog.Application.Queries.GetPosts;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using Blog.Domain.Repositories;
using Blog.Tests.Common;
using MockFactory = Blog.Tests.Common.MockFactory;

namespace Blog.Application.UnitTests.Queries.GetPosts
{
    [TestFixture]
    public class GetPostsQueryHandlerTests
    {
        private Mock<IPostRepository> _postRepositoryMock;


        [SetUp]
        public void SetUp()
        {
            _postRepositoryMock = new Mock<IPostRepository>();
        }

        [Test]
        public void When_GetPostQueryHandlerConstructedWithoutPostRepository_Expect_ArgumentNullExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                // ReSharper disable once ObjectCreationAsStatement
                new GetPostsQueryHandler(null, Mapper.GetInstance());
            });
        }

        [Test]
        public void When_GetPostQueryHandlerConstructedWithoutMapper_Expect_ArgumentNullExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                // ReSharper disable once ObjectCreationAsStatement
                new GetPostsQueryHandler(_postRepositoryMock.Object, null);
            });
        }

        [Test]
        public async Task When_HandledAndTenElementsFromFirstPageRquestedAndTenElementExists_Expect_TenElementsReturned()
        {
            //Arrange
            _postRepositoryMock.Setup(x => x.GetAllAsync(1, 10))
                .Returns(Task.FromResult(Enumerable.Range(0, 10).Select(_ => MockFactory.CreatePost())));

            var sut = new GetPostsQueryHandler(_postRepositoryMock.Object, Mapper.GetInstance());

            //Act
            var result = await sut.Handle(MockFactory.CreateGetPostsQuery(), CancellationToken.None);

            //Assert
            Assert.That(result.Length, Is.EqualTo(10));
        }
    }
}
