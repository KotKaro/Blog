using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Blog.API.Controllers;
using Blog.Domain.Models.Aggregates.Post;
using Blog.IntegrationTests.Common;
using Blog.Tests.Common;
using MediatR;
using NUnit.Framework;

namespace Blog.IntegrationTests.Controllers
{
    [TestFixture]
    public class PostControllerTests : IntegrationTestBase
    {
        [SetUp]
        public void SetUp()
        {
            BlogContext.Set<Post>().RemoveRange(BlogContext.Set<Post>());
        }

        [Test]
        public async Task When_GetCalledWithPageOneSizeTen_Expect_TenPostsReturned()
        {
            //Arrange
            var mediator = Container.Resolve<IMediator>();
            var controller = new PostController(mediator);

            BlogContext.Set<Post>().AddRange
            (
                Enumerable.Range(0, 10).Select(x => MockFactory.CreatePost())
            );

            await BlogContext.SaveChangesAsync();

            //Act
            var result = await controller.GetAll();

            //Assert
            Assert.That(result.Length, Is.EqualTo(10));
        }

        [Test]
        public async Task When_GetCalledWithPageOneSizeTenAndNextGetCalledWithPageTwoOfSizeTen_Expect_AllFirstPagePostIdsDiffrentThanSecondPagePostIds()
        {
            //Arrange
            var mediator = Container.Resolve<IMediator>();
            var controller = new PostController(mediator);

            BlogContext.Set<Post>().AddRange
            (
                Enumerable.Range(0, 20).Select(x => MockFactory.CreatePost())
            );

            await BlogContext.SaveChangesAsync();

            //Act
            var firstPage = await controller.GetAll();
            var secondPage = await controller.GetAll(2);

            //Assert
            Assert.That(firstPage.Length, Is.EqualTo(10));
            Assert.That(secondPage.Length, Is.EqualTo(10));
            Assert.That(firstPage.Select(x => x.Id)
                .All(x => secondPage.Select(y => y.Id).Any(y => y != x))
            );
        }

        [Test]
        public async Task When_CreatePostCalledWithProperCommand_Expect_PostCreated()
        {
            //Arrange
            var mediator = Container.Resolve<IMediator>();
            var controller = new PostController(mediator);

            //Act
            var result = await controller.Create(MockFactory.CreateCreatePostCommand());

            //Assert
            var value = result.Value;
            Assert.That(value, Is.Not.Null);
        }

        [Test]
        public async Task When_PostWithSpecificIdExists_Expect_PostDtoWithSameIdReturned()
        {
            //Arrange
            var mediator = Container.Resolve<IMediator>();
            var controller = new PostController(mediator);

            var post = MockFactory.CreatePost();
            BlogContext.Set<Post>().Add(post);
            await BlogContext.SaveChangesAsync();

            //Act
            var result = await controller.GetById(post.Id);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(post.Id));
        }
    }
}
