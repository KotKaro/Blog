using Blog.Application.DTO;
using Blog.Tests.Common;
using NUnit.Framework;

namespace Blog.Application.UnitTests.Mappers
{
    [TestFixture]
    public class CommentToCommentDtoMapperTests
    {
        [Test]
        public void When_PostMappedToPostDto_Expect_AllPropertiesSet()
        {
            //Arrange
            var mapper = Mapper.GetInstance();
            var comment = MockFactory.CreatComment();

            //Act
            var commentDto = mapper.Map<CommentDTO>(comment);

            //Assert
            Assert.AreEqual(comment.Id, commentDto.Id);
            Assert.AreEqual(comment.Content.Value, commentDto.Content);
            Assert.AreEqual(comment.Creator.Value, commentDto.Creator);
        }
    }
}
