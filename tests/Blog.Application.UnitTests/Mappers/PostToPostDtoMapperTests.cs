using Blog.Application.DTO;
using Blog.Tests.Common;
using FluentAssertions;
using Xunit;
using MockFactory = Blog.Tests.Common.MockFactory;

namespace Blog.Application.UnitTests.Mappers
{
    public class PostToPostDtoMapperTests
    {
        [Fact]
        public void When_PostMappedToPostDto_Expect_AllPropertiesSet()
        {
            //Arrange
            var mapper = Mapper.GetInstance();
            var post = MockFactory.CreatePost();

            //Act
            var postDto = mapper.Map<PostDTO>(post);

            //Assert
            post.Id.Should().Be(postDto.Id);
            post.Title.Value.Should().Be(postDto.Title);
            post.Content.Value.Should().Be(postDto.Content);
            post.CreationDate.Value.Should().Be(postDto.CreationDate);
        }
    }
}
