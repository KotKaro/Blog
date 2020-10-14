using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Blog.API.Controllers;
using Blog.Domain.Exceptions;
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
            var controller = new PostsController(mediator);

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
            var controller = new PostsController(mediator);

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
            var controller = new PostsController(mediator);

            //Act
            var result = await controller.Create(MockFactory.CreateCreatePostCommand());

            //Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task When_PostWithSpecificIdExists_Expect_PostDtoWithSameIdReturned()
        {
            //Arrange
            var mediator = Container.Resolve<IMediator>();
            var controller = new PostsController(mediator);

            var post = MockFactory.CreatePost();
            BlogContext.Set<Post>().Add(post);
            await BlogContext.SaveChangesAsync();

            //Act
            var result = await controller.GetById(post.Id);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(post.Id));
        }

        [Test]
        public async Task When_UpdateCalledAndPostDoesNotExists_Expect_RecordNotFoundExceptionThrown()
        {
            //Arrange
            var mediator = Container.Resolve<IMediator>();
            var controller = new PostsController(mediator);

            Assert.ThrowsAsync<RecordNotFoundException>(async () =>
            {
                await controller.Update(MockFactory.CreateUpdatePostCommand(Guid.NewGuid()));
            });
        }

        [Test]
        public async Task When_UpdateCalledAndPostExists_Expect_PostToBeUpdated()
        {
            //Arrange
            var mediator = Container.Resolve<IMediator>();
            var controller = new PostsController(mediator);

            var post = MockFactory.CreatePost();
            await BlogContext.Set<Post>().AddAsync(post);
            await BlogContext.SaveChangesAsync();

            //Act
            await controller.Update(MockFactory.CreateUpdatePostCommand(post.Id, "updatedTitle", "updatedContent"));

            //Assert
            var updated = await controller.GetById(post.Id);
            Assert.That(updated.Title, Is.EqualTo("updatedTitle"));
            Assert.That(updated.Content, Is.EqualTo("updatedContent"));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public async Task When_UpdateCalledAndPostTitleNotProvided_Expect_ArgumentExceptionToBeThrown(string title)
        {
            //Arrange
            var mediator = Container.Resolve<IMediator>();
            var controller = new PostsController(mediator);

            var post = MockFactory.CreatePost();
            await BlogContext.Set<Post>().AddAsync(post);
            await BlogContext.SaveChangesAsync();

            //Act + Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await controller.Update(MockFactory.CreateUpdatePostCommand(post.Id, title, "updatedContent"));
            });
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public async Task When_UpdateCalledAndPostContentNotProvided_Expect_ArgumentExceptionToBeThrown(string content)
        {
            //Arrange
            var mediator = Container.Resolve<IMediator>();
            var controller = new PostsController(mediator);

            var post = MockFactory.CreatePost();
            await BlogContext.Set<Post>().AddAsync(post);
            await BlogContext.SaveChangesAsync();

            //Act + Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await controller.Update(MockFactory.CreateUpdatePostCommand(post.Id, "title", content));
            });
        }
    }
}
