using Blog.Domain.Repositories.PostReadRepository;
using Blog.Tests.Common;
using FluentAssertions;
using Xunit;

namespace Blog.Application.UnitTests.Mappers
{
    public class CommentToCommentDtoMapperTests
    {
        [Fact]
        public void When_PostMappedToPostDto_Expect_AllPropertiesSet()
        {
            //Arrange
            var mapper = Mapper.GetInstance();
            var comment = MockFactory.CreatComment();

            //Act
            var commentDto = mapper.Map<CommentDTO>(comment);

            //Assert
            comment.Id.Should().Be(commentDto.Id);
            comment.Content.Value.Should().Be(commentDto.Content);
            comment.Creator.Value.Should().Be(commentDto.Creator);
        }
    }
}
