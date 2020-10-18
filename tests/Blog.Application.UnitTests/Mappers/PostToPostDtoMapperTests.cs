using Blog.Application.DTO;
using Blog.Tests.Common;
using NUnit.Framework;
using MockFactory = Blog.Tests.Common.MockFactory;

namespace Blog.Application.UnitTests.Mappers
{
    [TestFixture]
    public class PostToPostDtoMapperTests
    {
        [Test]
        public void When_PostMappedToPostDto_Expect_AllPropertiesSet()
        {
            //Arrange
            var mapper = Mapper.GetInstance();
            var post = MockFactory.CreatePost();

            //Act
            var postDto = mapper.Map<PostDTO>(post);

            //Assert
            Assert.AreEqual(post.Id, postDto.Id);
            Assert.AreEqual(post.Title.Value, postDto.Title);
            Assert.AreEqual(post.Content.Value, postDto.Content);
            Assert.AreEqual(post.CreationDate.Value, postDto.CreationDate);
        }
    }
}
